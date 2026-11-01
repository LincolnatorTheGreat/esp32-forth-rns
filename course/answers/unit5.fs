\ Unit 5 answer key
: twice    ( x xt -- x' )  dup >r execute r> execute ;
: map!     ( addr n xt -- )
  -rot 0 ?do                                  ( xt addr )
    2dup i cells + @ swap execute  over i cells + !
  loop 2drop ;
: count-if ( addr n xt -- k )
  swap >r  0 -rot  r>                         ( k addr xt n )
  0 ?do  over i cells + @ over execute  if rot 1+ -rot then  loop
  2drop ;

create ops  ' + ,  ' - ,  ' * ,  ' / ,
: calc ( a b op -- n )  cells ops + @ execute ;

300 constant HOLD-MS
: taps  ( -- a n )  s" .etaoinsrhldcumf" ;
: holds ( -- a n )  s" .ETAOINSRHLDCUMF" ;

: chord>char ( mask hold? -- c )  if holds else taps then drop + c@ ;
: decode     ( mask ms -- c )  HOLD-MS >= chord>char ;
: decode-all ( events n buf -- )
  swap 0 ?do                                  ( events buf )
    over i 2* cells + dup @ swap cell+ @      ( events buf mask ms )
    decode over i + c!
  loop 2drop ;
