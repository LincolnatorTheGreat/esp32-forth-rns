\ Unit 8: your own outer interpreter (interpret only, no compiling).  Grade: course/run.sh 8
\ ueforth's find is ( addr len -- xt | 0 ).  No /string, no 0> : write what you need.

: >digit ( c -- n | -1 )  ;               \ '0'..'9' -> 0..9, anything else -1
: >num   ( a n -- n true | 0 false )  ;   \ unsigned decimal only
: skip   ( a n -- a' n' )  ;              \ step past leading spaces
: scan   ( a n -- a' n' )  ;              \ step past non-spaces
: next-word ( a n -- a' n' word-a word-n )  ;
: interpret-word ( a n -- )  ;            \ find + execute, else number, else print ?

\ Careful: the words you execute use the data stack too.
\ Where must the interpreter keep its own position in the string?
: my-interpret ( a n -- )  ;
