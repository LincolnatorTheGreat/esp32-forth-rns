\ Unit 4 exercises: strings, and text.fs rewritten.  Grade: course/run.sh 4
\ Uses the repo's own font and framebuffer, so this unit stands alone.
include src/forth/font3x5.fs
include src/forth/fb.fs

: same?      ( a1 n1 a2 n2 -- flag )  ;   \ true if the strings are equal
: count-char ( a n c -- k )  ;            \ how many times c appears
: upcase!    ( a n -- )  ;                \ a..z -> A..Z in place

\ --- text.fs rewritten. Cell is 4x6 pixels: a 32 x 10 grid. ------------
\ draw-glyph must use NO variables: keep everything on the stack.
\ Hint: factor it. A word that draws one 3-pixel row is most of the work.
\ (3dup exists in ueforth.)
: draw-glyph ( c x y -- )  ;              \ pixel position of the top-left
: draw-str   ( a n x y -- )  ;            \ 4 pixels per character
: cell-str   ( a n col row -- )  ;        \ grid position
