\ Unit 2 exercises: control flow.  Grade with:  course/run.sh 2
include src/forth/font3x5.fs        \ for glyph@ in the project

: signum ( n -- -1|0|1 )  ;              \ bonus: no if, use two comparisons
: within ( n lo hi -- flag )  ;          \ true when lo <= n < hi
: clamp  ( n lo hi -- n' )  ;            \ force n into lo..hi
: sum-to ( n -- 1+2+..+n )  ;            \ 0 sum-to is 0
: gcd    ( a b -- g )  ;                 \ Euclid: repeat (a b) -> (b a mod b)
: bit@   ( u n -- 0|1 )  ;               \ bit n of u
: bits   ( u -- n )  ;                   \ how many bits are set
: collatz-steps ( n -- steps )  ;        \ even: n/2, odd: 3n+1, until n = 1

\ --- Project: print a glyph of your 3x5 font as # and . ---------------
\ glyph@ ( c -- u ) gives 15 bits, bit 14 = top-left, rows of 3.
\ char A show-glyph   should print:
\   ###
\   #.#
\   ###
\   #.#
\   #.#
: show-glyph ( c -- )  ;
