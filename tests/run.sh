#!/bin/sh
# Run Forth tests on the host build. Usage: tests/run.sh
cd "$(dirname "$0")/.." || exit 1
for t in tests/test_*.fs; do
  echo "== $t"; third_party/ueforth/out/posix/ueforth < "$t" 2>&1 | grep -E 'FAIL|ALL PASSED|FAILURES|NOT FOUND|ERROR'
done
