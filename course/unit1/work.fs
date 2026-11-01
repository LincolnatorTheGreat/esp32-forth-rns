\ Unit 1 exercises: stack mechanics.  Grade with:  course/run.sh 1
\ Fill in each body. Keep the stack comment; write it first when you add words.
\ Allowed: dup drop swap over rot -rot nip >r r> and arithmetic. No variables.

\ --- Missing standard words (ueforth lacks these) -----------------------
: tuck  ( a b -- b a b )  ;
: 2swap ( a b c d -- c d a b )  ;
: 2over ( a b c d -- a b c d a b )  ;
: third ( a b c -- a b c a )  ;        \ a stand-in for "2 pick"

\ --- Arithmetic ---------------------------------------------------------
: cube  ( n -- n^3 )  ;
: avg   ( a b -- avg )  ;              \ rounds toward minus infinity

\ --- Project: feeds and speeds in integer fixed-point ---------------------
\ Units: diameters in thousandths of an inch (0.500" = 500),
\ chip load in ten-thousandths (0.0020" = 20), feed in thousandths/min.
\ rpm = 12 * sfm / (pi * dia_inches) = 3819.7 * sfm / dia_thou
: sfm>rpm ( sfm dia-thou -- rpm )  ;              \ rounded to nearest; hint: */
: ipm     ( rpm flutes chip-tenk -- ipm-thou )  ;  \ rpm * flutes * chip / 10
: mm>thou ( mm/100 -- thou )  ;                   \ 12.70 mm = 1270 -> 500, rounded to nearest
