\ Unit 1 answer key. Other correct answers exist; the tests decide.
: tuck  ( a b -- b a b )  swap over ;
: 2swap ( a b c d -- c d a b )  rot >r rot r> ;
: 2over ( a b c d -- a b c d a b )  >r >r 2dup r> r> 2swap ;
: third ( a b c -- a b c a )  >r over r> swap ;

: cube  ( n -- n^3 )  dup dup * * ;
: avg   ( a b -- avg )  + 2/ ;

: sfm>rpm ( sfm dia-thou -- rpm )  >r 38197 r> 10 * */ ;
: ipm     ( rpm flutes chip-tenk -- ipm-thou )  * 10 */ ;
: mm>thou ( mm/100 -- thou )  3937 * 5000 + 10000 / ;
