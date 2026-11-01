\ Unit 4 answer key
include src/forth/font3x5.fs
include src/forth/fb.fs

: same? ( a1 n1 a2 n2 -- flag )
  rot over <> if drop 2drop 0 exit then        ( a1 a2 n )
  0 ?do
    over i + c@  over i + c@  <> if 2drop 0 unloop exit then
  loop 2drop -1 ;

: count-char ( a n c -- k )
  swap >r  0 -rot  r>                          ( k a c n )
  0 ?do  over i + c@ over =  if rot 1+ -rot then  loop
  2drop ;

: lower? ( c -- flag )  dup [char] a >= swap [char] z <= and ;
: upcase! ( a n -- )
  0 ?do  dup i + c@  dup lower? if 32 - over i + c! else drop then  loop
  drop ;

\ No variables: factor into "one row" and "five rows".
: row-bits ( bits row -- 3bits )  4 swap - 3 * rshift 7 and ;
: draw-row ( x y 3bits -- )
  3 0 do  3dup  2 i - rshift 1 and  rot i + -rot  fb-pixel!  loop
  drop 2drop ;
: draw-glyph ( c x y -- )
  rot glyph@                                   ( x y bits )
  5 0 do  3dup  i row-bits  swap i + swap  draw-row  loop
  drop 2drop ;
: draw-str ( a n x y -- )
  rot 0 ?do                                    ( a x y )
    3dup rot i + c@ -rot draw-glyph
    swap 4 + swap
  loop 2drop drop ;
: cell-str ( a n col row -- )  6 * swap 4 * swap draw-str ;
