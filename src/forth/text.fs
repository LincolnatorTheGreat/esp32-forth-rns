\ 3x5 text on the framebuffer. Cell is 4x6 pixels -> 32 x 10 character grid.

4 constant CELL-W
6 constant CELL-H
FB-W CELL-W / constant COLS
FB-H CELL-H / constant ROWS

variable gx  variable gy  variable gbits
variable sx  variable sy

: draw-glyph ( c x y -- )
  gy ! gx ! glyph@ gbits !
  5 0 do
    3 0 do
      gbits @ 14 j 3 * i + - rshift 1 and
      gx @ i + gy @ j + rot fb-pixel!
    loop
  loop ;

: draw-str ( addr len x y -- )
  sy ! sx !
  0 do
    dup i + c@ sx @ sy @ draw-glyph
    CELL-W sx +!
  loop drop ;

: cell-str ( addr len col row -- ) CELL-H * >r CELL-W * r> draw-str ;
