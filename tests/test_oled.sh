#!/bin/sh
# Drive the windowed REPL headlessly, decode the final OLED frame, and check the output.
# Needs the venv python with fonttools: PYTHON=path tests/test_oled.sh
cd "$(dirname "$0")/.." || exit 1
PY=${PYTHON:-python3}
F=$(mktemp)
SDL_VIDEODRIVER=dummy OLED_TYPE=': sq dup * ; 
cls 7 sq . 
' OLED_QUIT_MS=2500 OLED_DUMP_FILE=$F tools/oled >/dev/null 2>&1
OUT=$($PY tools/decode_screen.py "$F"); rm -f "$F"
echo "$OUT" | grep -q '^49  ok' && echo "oled repl: PASSED" || { echo "oled repl: FAILED"; echo "$OUT"; exit 1; }
