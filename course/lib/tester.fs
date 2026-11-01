\ course/lib/tester.fs -- a tiny Hayes-style tester.
\ Usage:   T{ 1 2 tuck -> 2 1 2 }T
\ Everything before -> is the code under test; after -> the expected stack.
\ A failing line is printed with FAIL:, and REPORT prints the totals.
\ A line that stops with an ERROR never reaches }T, so it counts as failed.
\ Read this file after Unit 3: every word in it uses only Units 1-3.

variable start-depth   variable #results
variable #fails        variable #tests   variable #passes
create results 32 cells allot

: T{ ( -- )  depth start-depth !  1 #tests +! ;

: -> ( x1..xn -- )            \ save the actual results, top of stack first
  depth start-depth @ -  0 max  dup #results !
  0 ?do  results i cells + !  loop ;

: tclear ( ... -- )  begin depth start-depth @ > while drop repeat ;
: tfail ( -- )  ." FAIL: " tib #tib @ type cr  1 #fails +! ;

: }T ( y1..yn -- )            \ compare expected values with the saved ones
  depth start-depth @ -  #results @ <> if tclear tfail exit then
  #results @ 0 ?do
    results i cells + @ <> if tclear tfail unloop exit then
  loop  1 #passes +! ;

: report ( -- )  cr #tests @ . ." tests, " #tests @ #passes @ - . ." failed" cr ;

0 #tests !  0 #fails !  0 #passes !
