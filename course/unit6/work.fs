\ Unit 6 exercises: defining words and compile time.  Grade: course/run.sh 6
include src/forth/fb.fs

\ Defining words: each one is  create <build the data>  does> <runtime>
: konst    ( n "name" -- )  ;           \ name ( -- n )
: counter: ( "name" -- )  ;             \ name ( -- n ) returns 1, 2, 3, ...
: array    ( n "name" -- )  ;           \ name ( i -- addr ) of cell i
: field    ( offset size "name" -- offset' )  ;  \ name ( addr -- addr+offset )

\ Compiling words: these run while another word is being compiled
: unless   ( compile: -- orig ) ( run: flag -- )  ;  immediate  \ "if not"
: square   ( compile: -- ) ( run: n -- n*n )  ;  immediate     \ compiles dup *

\ --- Project: sprites -----------------------------------------------------
\ $3C $42 $81 $81 $81 $81 $42 $3C bitmap: ring
\ stores 8 row bytes in order (top row first); ring ( -- addr ).
: bitmap: ( b0 b1 b2 b3 b4 b5 b6 b7 "name" -- )  ;
: draw-sprite ( addr x y -- )  ;        \ 8x8; bit 7 of each byte is the left pixel
