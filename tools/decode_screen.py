#!/usr/bin/env python3
"""Decode a raw 1024-byte OLED frame (SSD1306 page layout) into 10 text rows using the 3x5 font.
Usage: decode_screen.py FRAME.bin"""
import sys
sys.path.insert(0, __import__("os").path.dirname(__file__))
from otf2glyphs import load

FONT = __import__("os").path.join(__import__("os").path.dirname(__file__), "..", "assets", "font", "3x5-ascii.otf")
rev = {}
for ch, rows in load(FONT).items():
    rev.setdefault("".join(rows[:5]), ch)

def decode(data):
    px = lambda x, y: "#" if (data[(y >> 3) * 128 + x] >> (y & 7)) & 1 else "."
    lines = []
    for r in range(10):
        line = ""
        for c in range(32):
            key = "".join(px(c * 4 + dx, r * 6 + dy) for dy in range(5) for dx in range(3))
            line += rev.get(key, "?")
        lines.append(line.rstrip())
    return lines

if __name__ == "__main__":
    print("\n".join(decode(open(sys.argv[1], "rb").read())))
