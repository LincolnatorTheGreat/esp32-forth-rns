#!/bin/sh
# Grade a course unit on the host ueforth.
#   course/run.sh 1        tests course/unit1/work.fs (your code)
#   course/run.sh 1 key    tests course/answers/unit1.fs (the answer key)
cd "$(dirname "$0")/.." || exit 1
U=$1
[ -n "$U" ] || { echo "usage: course/run.sh UNIT [key]"; exit 1; }
W=course/unit$U/work.fs
[ "$2" = key ] && W=course/answers/unit$U.fs
FORTH=${FORTH:-third_party/ueforth/out/posix/ueforth}
cat course/lib/tester.fs "$W" course/unit$U/test.fs | "$FORTH" 2>&1 \
  | grep -E 'FAIL|tests,|ERROR|NOT FOUND|UNDERFLOW|OVERFLOW'
