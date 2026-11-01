\ Unit 5 exercises: execution tokens, tables, and a chord decoder.  Grade: course/run.sh 5

: twice    ( x xt -- x' )  ;            \ apply xt two times
: map!     ( addr n xt -- )  ;          \ replace each of n cells with xt applied to it
: count-if ( addr n xt -- k )  ;        \ xt is ( x -- flag ); count the trues

\ A jump table: op 0 is +, 1 is -, 2 is *, 3 is /
create ops   ( fill this in with four execution tokens )
: calc ( a b op -- n )  ;

\ --- Milestone 2: chord decoder ------------------------------------------
\ Four buttons give a 4-bit mask (button 0 = bit 0). A short press types
\ from the tap table, a press held HOLD-MS or longer from the hold table.
\ These tables are placeholders; swap in your real MessageEase layout later.
300 constant HOLD-MS
: taps  ( -- a n )  s" .etaoinsrhldcumf" ;   \ index = mask; mask 0 unused
: holds ( -- a n )  s" .ETAOINSRHLDCUMF" ;

: chord>char ( mask hold? -- c )  ;
: decode     ( mask ms -- c )  ;            \ ms = how long the chord was held
\ events: pairs of cells (mask, ms). Write one character per event into buf.
: decode-all ( events n buf -- )  ;
