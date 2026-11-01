\ Unit 5 tests
T{ 3 ' 1+ twice -> 5 }T
T{ 3 ' dup twice -> 3 3 3 }T
create nums 1 , 2 , 3 , 4 ,
: dbl 2* ;
T{ nums 4 ' dbl map! nums @ nums 3 cells + @ -> 2 8 }T
: even? 1 and 0= ;
T{ nums 4 ' even? count-if -> 4 }T
: big? 4 > ;
T{ nums 4 ' big? count-if -> 2 }T
T{ nums 0 ' big? count-if -> 0 }T
T{ 7 3 0 calc -> 10 }T
T{ 7 3 1 calc -> 4 }T
T{ 7 3 2 calc -> 21 }T
T{ 7 3 3 calc -> 2 }T
T{ 1 0 chord>char -> char e }T
T{ 1 -1 chord>char -> char E }T
T{ 15 0 chord>char -> char f }T
T{ 2 100 decode -> char t }T
T{ 2 300 decode -> char T }T
T{ 2 299 decode -> char t }T
create evs  1 , 100 ,  2 , 500 ,  3 , 50 ,  9 , 1000 ,
create out 8 allot
: t-all evs 4 out decode-all  out 4 ;
: chars4 ( a n -- c1 c2 c3 c4 ) drop dup c@ swap 1+ dup c@ swap 1+ dup c@ swap 1+ c@ ;
T{ t-all chars4 -> char e char T char a char H }T
report bye
