\ Unit 2 answer key
include src/forth/font3x5.fs

: signum ( n -- -1|0|1 )  dup 0< swap 0 > - ;   \ (n<0) - (n>0), flags are -1/0
: within ( n lo hi -- flag )  over - >r - r> u< ; \ one unsigned compare does both ends
: clamp  ( n lo hi -- n' )  rot min max ;
: sum-to ( n -- sum )  0 swap 1+ 1 ?do i + loop ;
: gcd    ( a b -- g )  begin dup while swap over mod repeat drop ;
: bit@   ( u n -- 0|1 )  rshift 1 and ;
: bits   ( u -- n )  0 swap begin dup while dup 1 and rot + swap 1 rshift repeat drop ;
: collatz-steps ( n -- steps )
  0 swap begin dup 1 > while
    dup 1 and if 3 * 1+ else 2/ then
    swap 1+ swap
  repeat drop ;

: show-glyph ( c -- )
  glyph@
  5 0 do
    cr 3 0 do
      dup 14 j 3 * i + - rshift 1 and
      if [char] # else [char] . then emit
    loop
  loop drop cr ;
