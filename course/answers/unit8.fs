\ Unit 8 answer key
: >digit ( c -- n | -1 )  [char] 0 -  dup 10 u< 0= if drop -1 then ;
: >num ( a n -- n true | 0 false )
  dup 0= if 2drop 0 0 exit then
  0 -rot 0 ?do                               ( acc a )
    dup i + c@ >digit  dup 0< if 2drop drop 0 0 unloop exit then
    rot 10 * + swap
  loop drop -1 ;
: skip ( a n -- a' n' )
  begin dup 0 > if over c@ bl = else 0 then while 1- swap 1+ swap repeat ;
: scan ( a n -- a' n' )
  begin dup 0 > if over c@ bl <> else 0 then while 1- swap 1+ swap repeat ;
: next-word ( a n -- a' n' wa wn )  skip over >r scan over r@ - r> swap ;
: interpret-word ( a n -- )
  2dup find ?dup if nip nip execute exit then
  >num 0= if drop ." ? " then ;

\ The position lives in variables, like ueforth's >in and #tib, because
\ the words being interpreted are free to use the whole data stack.
variable src  variable #src
: my-interpret ( a n -- )
  #src ! src !
  begin  src @ #src @ skip #src ! src !  #src @ while
    src @ #src @ next-word >r >r  #src ! src !  r> r> interpret-word
  repeat ;
