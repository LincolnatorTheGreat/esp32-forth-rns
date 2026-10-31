\ REPL console on the framebuffer: a 32x10 text grid drawn with the 3x5 font.
\ Redirects TYPE and KEY so the ordinary Forth prompt/echo/output appear on the OLED.

COLS ROWS * constant CON-SIZE
create cbuf CON-SIZE allot
variable ccol  variable crow  variable dirty

: con-clear ( -- ) cbuf CON-SIZE 32 fill 0 ccol ! 0 crow ! ;

: con-scroll ( -- )
  cbuf COLS + cbuf CON-SIZE COLS - cmove
  cbuf CON-SIZE COLS - + COLS 32 fill ;

: con-nl ( -- )
  crow @ 1+ dup ROWS = if drop ROWS 1- con-scroll then crow ! ;

: con-put ( c -- )
  dup 13 = if drop 0 ccol ! exit then
  dup 10 = if drop con-nl exit then
  dup 8  = if drop ccol @ 0 > if -1 ccol +! then exit then
  dup 32 < if drop exit then
  ccol @ COLS = if 0 ccol ! con-nl then
  cbuf crow @ COLS * + ccol @ + c!  1 ccol +! ;

: con-render ( -- )
  fb-clear
  ROWS 0 do cbuf i COLS * + COLS 0 i cell-str loop
  ccol @ CELL-W *  crow @ CELL-H * 5 +      ( x y )   \ cursor: underline in the spare row
  3 0 do 2dup swap i + swap 1 fb-pixel! loop 2drop
  fb-flush ;

: con-type ( a n -- ) 0 ?do dup i + c@ con-put loop drop 1 dirty ! ;

: con-key ( -- c )
  dirty @ if con-render 0 dirty ! then
  default-key dup -1 = if drop bye then ;

: console-on ( -- )
  con-clear ['] con-type is type  ['] con-key is key  1 dirty ! ;

: cls ( -- ) con-clear 1 dirty ! ;
