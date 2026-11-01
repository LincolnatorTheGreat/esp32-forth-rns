\ Unit 6 tests
42 konst k42
T{ k42 k42 -> 42 42 }T
counter: hits
T{ hits hits hits -> 1 2 3 }T
5 array arr5
T{ 7 2 arr5 !  2 arr5 @ -> 7 }T
T{ 1 arr5 0 arr5 - -> 1 cells }T
0 4 field .a  8 field .b  constant /rec
T{ /rec -> 12 }T
T{ 100 .a 100 .b -> 100 104 }T
: t-un ( flag -- n ) unless 1 else 2 then ;
T{ 0 t-un -> 1 }T
T{ 5 t-un -> 2 }T
: t-sq ( n -- n*n ) square ;
T{ 6 t-sq -> 36 }T
T{ -3 t-sq -> 9 }T
$80 0 0 0 0 0 0 $01 bitmap: corners
T{ corners c@ corners 7 + c@ -> $80 $01 }T
T{ fb-clear corners 10 20 draw-sprite 10 20 fb-pixel@ 17 27 fb-pixel@ 11 20 fb-pixel@ -> -1 -1 0 }T
report bye
