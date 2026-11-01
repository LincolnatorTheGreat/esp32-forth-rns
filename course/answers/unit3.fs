\ Unit 3 answer key
: cells-sum ( addr n -- sum )  0 swap 0 ?do over i cells + @ + loop nip ;

128 constant FB-W
64 constant FB-H
FB-W FB-H * 8 / constant FB-BYTES
create fb FB-BYTES allot
defer fb-flush

: fb-clear  ( -- )  fb FB-BYTES 0 fill ;
: fb-addr   ( x y -- addr mask )
  dup 3 rshift FB-W * rot + fb +      ( y addr )
  swap 7 and 1 swap lshift ;
: on-screen? ( x y -- flag )  FB-H u< swap FB-W u< and ;
: fb-pixel! ( x y on -- )
  >r 2dup on-screen? 0= if 2drop r> drop exit then
  fb-addr over c@ swap               ( addr byte mask )
  r> if or else invert and then
  swap c! ;
: fb-pixel@ ( x y -- flag )
  2dup on-screen? 0= if 2drop 0 exit then
  fb-addr swap c@ and 0<> ;

: hline  ( x y len -- )  0 ?do over i + over 1 fb-pixel! loop 2drop ;

\ Bresenham, the integer stepping a CNC control uses for G01
variable x0  variable y0  variable x1  variable y1
variable dx  variable dy  variable sx  variable sy  variable err
: dir ( n -- -1|1 )  0< if -1 else 1 then ;
: line ( x0 y0 x1 y1 -- )
  y1 ! x1 ! y0 ! x0 !
  x1 @ x0 @ - dup abs dx ! dir sx !
  y1 @ y0 @ - dup abs negate dy ! dir sy !
  dx @ dy @ + err !
  begin
    x0 @ y0 @ 1 fb-pixel!
    x0 @ x1 @ = y0 @ y1 @ = and 0=
  while
    err @ 2*                                   ( e2 )
    dup dy @ >= if dy @ err +! sx @ x0 +! then
    dx @ <= if dx @ err +! sy @ y0 +! then
  repeat ;

\ Midpoint circle, the integer stepping behind G02/G03
variable cx  variable cy  variable px  variable py  variable d
: c+ ( dx dy -- )  cy @ + swap cx @ + swap 1 fb-pixel! ;
: plot8 ( -- )
  px @ py @ c+         px @ negate py @ c+
  px @ py @ negate c+  px @ negate py @ negate c+
  py @ px @ c+         py @ negate px @ c+
  py @ px @ negate c+  py @ negate px @ negate c+ ;
: circle ( x y r -- )
  px ! cy ! cx !  0 py !  1 px @ - d !
  begin px @ py @ >= while
    plot8  1 py +!
    d @ 0< if py @ 2* 1+ d +!
           else -1 px +!  py @ px @ - 2* 1+ d +! then
  repeat ;
