\ Unit 6 answer key
include src/forth/fb.fs

: konst    ( n "name" -- )  create , does> @ ;
: counter: ( "name" -- )  create 0 , does> 1 over +! @ ;
: array    ( n "name" -- )  create cells allot does> swap cells + ;
: field    ( offset size "name" -- offset' )  create over , + does> @ + ;

: unless  postpone 0= postpone if ; immediate
: square  postpone dup postpone * ; immediate

: bitmap: ( b0 .. b7 "name" -- )
  create here 8 allot 7 +            ( b0 .. b7 addr-of-last-row )
  8 0 do swap over c! 1- loop drop ;
: draw-sprite ( addr x y -- )
  8 0 do 8 0 do
    3dup rot j + c@ 7 i - rshift 1 and   ( a x y x y bit )
    rot i + rot j + rot fb-pixel!
  loop loop drop 2drop ;
