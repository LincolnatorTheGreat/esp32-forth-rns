\ 128x64 1-bpp framebuffer in SSD1306 page layout:
\ byte index = (y / 8) * 128 + x, bit = y & 7. Flush is the only display-specific word.

128 constant FB-W
64 constant FB-H
FB-W FB-H * 8 / constant FB-BYTES
create fb FB-BYTES allot

: fb-clear ( -- ) fb FB-BYTES 0 fill ;

: fb-addr ( x y -- addr mask ) dup 3 rshift FB-W * rot + fb + swap 7 and 1 swap lshift ;

: fb-pixel@ ( x y -- flag )
  2dup FB-H u< swap FB-W u< and 0= if 2drop 0 exit then
  fb-addr swap c@ and 0<> ;

: fb-pixel! ( x y on -- )
  >r 2dup FB-H u< swap FB-W u< and 0= if 2drop r> drop exit then
  fb-addr over c@ swap
  r> if or else invert and then
  swap c! ;
