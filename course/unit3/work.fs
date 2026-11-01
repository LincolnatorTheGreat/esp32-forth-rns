\ Unit 3 exercises: memory, and fb.fs rewritten from scratch.  Grade: course/run.sh 3
\ Don't open src/forth/fb.fs until your version passes.

: cells-sum ( addr n -- sum )  ;     \ add up n cells starting at addr

\ --- Framebuffer: 128x64, 1 bit per pixel, SSD1306 page layout ----------
\ byte index = (y / 8) * 128 + x      bit = y and 7
128 constant FB-W
64 constant FB-H
FB-W FB-H * 8 / constant FB-BYTES
create fb FB-BYTES allot
defer fb-flush

: fb-clear  ( -- )  ;
: fb-addr   ( x y -- addr mask )  ;         \ byte address and bit mask
: fb-pixel! ( x y on -- )  ;                \ on = 0 clears; off-screen x,y ignored
: fb-pixel@ ( x y -- flag )  ;              \ -1 if lit; off-screen reads 0

\ --- Drawing --------------------------------------------------------------
: hline  ( x y len -- )  ;
: line   ( x0 y0 x1 y1 -- )  ;              \ Bresenham; both endpoints lit
: circle ( x y r -- )  ;                    \ midpoint circle

\ To look at your drawing:
\   host:  include src/forth/fb-term.fs   then  fb-flush
\   phone: fb fb-show
