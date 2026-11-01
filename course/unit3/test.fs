\ Unit 3 tests
create t3 1 , 2 , 3 , 40 ,
T{ t3 3 cells-sum -> 6 }T
T{ t3 4 cells-sum -> 46 }T
T{ t3 0 cells-sum -> 0 }T

: lit-count ( -- n )  0 FB-H 0 do FB-W 0 do i j fb-pixel@ if 1+ then loop loop ;

T{ fb-clear lit-count -> 0 }T
T{ 3 10 fb-addr -> fb 131 + 4 }T
T{ 127 63 fb-addr -> fb 1023 + 128 }T
T{ 3 5 1 fb-pixel! 3 5 fb-pixel@ -> -1 }T
T{ 3 5 0 fb-pixel! 3 5 fb-pixel@ -> 0 }T
T{ 127 63 1 fb-pixel! fb 1023 + c@ -> $80 }T
T{ 200 5 1 fb-pixel! 200 5 fb-pixel@ -> 0 }T
T{ -1 5 1 fb-pixel! -1 5 fb-pixel@ -> 0 }T
T{ 5 70 fb-pixel@ -> 0 }T
T{ fb-clear 10 20 5 hline lit-count -> 5 }T
T{ 14 20 fb-pixel@ 15 20 fb-pixel@ -> -1 0 }T
T{ fb-clear 0 0 10 0 line lit-count -> 11 }T
T{ fb-clear 0 0 7 7 line 3 3 fb-pixel@ lit-count -> -1 8 }T
T{ fb-clear 10 5 0 0 line 10 5 fb-pixel@ 0 0 fb-pixel@ -> -1 -1 }T
T{ fb-clear 0 0 3 9 line lit-count -> 10 }T
T{ fb-clear 64 32 10 circle 74 32 fb-pixel@ 54 32 fb-pixel@ -> -1 -1 }T
T{ 64 42 fb-pixel@ 64 22 fb-pixel@ 64 32 fb-pixel@ -> -1 -1 0 }T
report bye
