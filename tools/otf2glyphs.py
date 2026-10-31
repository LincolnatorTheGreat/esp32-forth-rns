#!/usr/bin/env python3
"""Rasterize the 3x5 ASCII OTF into per-character bitmaps.

Pixel size is unitsPerEm/6 (333.33 units); each glyph occupies up to 3 columns
and 6 rows (row 0 = top). Samples cell centers with a nonzero-winding test.
Usage: otf2glyphs.py FONT.otf [--art]
"""
import sys
from fontTools.ttLib import TTFont
from fontTools.pens.recordingPen import RecordingPen

COLS, ROWS = 3, 6

def contours(rec):
    cur, out = [], []
    for op, args in rec.value:
        if op == "moveTo":
            cur = [args[0]]
        elif op == "lineTo":
            cur.append(args[0])
        elif op == "closePath":
            out.append(cur); cur = []
        else:
            raise SystemExit(f"unsupported outline op {op}")
    return out

def winding(pt, polys):
    x, y = pt; w = 0
    for poly in polys:
        for (x0, y0), (x1, y1) in zip(poly, poly[1:] + poly[:1]):
            if y0 <= y < y1 and (x1 - x0) * (y - y0) - (x - x0) * (y1 - y0) > 0:
                w += 1
            elif y1 <= y < y0 and (x1 - x0) * (y - y0) - (x - x0) * (y1 - y0) < 0:
                w -= 1
    return w

def glyph_rows(font, ch):
    gs, upm = font.getGlyphSet(), font["head"].unitsPerEm
    px = upm / ROWS
    rec = RecordingPen(); gs[font.getBestCmap()[ord(ch)]].draw(rec)
    polys = contours(rec)
    rows = []
    for r in range(ROWS):
        y = upm - (r + 0.5) * px
        rows.append("".join("#" if winding(((c + 0.5) * px, y), polys) else "." for c in range(COLS)))
    return rows

def load(path):
    f = TTFont(path)
    return {chr(c): glyph_rows(f, chr(c)) for c in range(32, 127) if c in f.getBestCmap()}

if __name__ == "__main__":
    g = load(sys.argv[1])
    missing = [chr(c) for c in range(32, 127) if chr(c) not in g]
    print(f"{len(g)} glyphs, missing: {missing}", file=sys.stderr)
    top = sum(1 for r in g.values() if r[0] != "..."); bot = sum(1 for r in g.values() if r[5] != "...")
    print(f"glyphs using row0: {top}, row5: {bot}", file=sys.stderr)
    if "--art" in sys.argv:
        for c in "AaGgpqy,;_Q@":
            print(repr(c)); print("\n".join(g[c]))
