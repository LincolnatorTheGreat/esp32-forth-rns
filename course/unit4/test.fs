\ Unit 4 tests
: t-same1 s" abc" s" abc" same? ;
: t-same2 s" abc" s" abd" same? ;
: t-same3 s" abc" s" ab" same? ;
: t-same4 s" " s" " same? ;
T{ t-same1 -> -1 }T
T{ t-same2 -> 0 }T
T{ t-same3 -> 0 }T
T{ t-same4 -> -1 }T
: t-cc1 s" mississippi" [char] s count-char ;
: t-cc2 s" forth" [char] z count-char ;
T{ t-cc1 -> 4 }T
T{ t-cc2 -> 0 }T
create ubuf 16 allot
: t-up s" Hello, x9!" ubuf swap cmove  ubuf 10 upcase!  ubuf 10 s" HELLO, X9!" same? ;
T{ t-up -> -1 }T

: lit-count ( -- n )  0 FB-H 0 do FB-W 0 do i j fb-pixel@ if 1+ then loop loop ;
T{ fb-clear char A 0 0 draw-glyph 0 0 fb-pixel@ 1 0 fb-pixel@ 2 0 fb-pixel@ -> -1 -1 -1 }T
T{ 1 1 fb-pixel@ 1 3 fb-pixel@ 0 4 fb-pixel@ 2 4 fb-pixel@ -> 0 0 -1 -1 }T
T{ lit-count -> 12 }T
T{ fb-clear char A 10 20 draw-glyph 10 20 fb-pixel@ 11 21 fb-pixel@ lit-count -> -1 0 12 }T
: t-str s" AA" 0 0 draw-str ;
T{ fb-clear t-str 4 0 fb-pixel@ 3 0 fb-pixel@ lit-count -> -1 0 24 }T
: t-cell s" A" 2 1 cell-str ;
T{ fb-clear t-cell 8 6 fb-pixel@ 9 7 fb-pixel@ lit-count -> -1 0 12 }T
report bye
