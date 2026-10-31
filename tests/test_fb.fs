include src/forth/load.fs
variable fails
: check ( flag name-addr len -- ) rot if 2drop else ." FAIL: " type cr 1 fails +! then ;
: t-pixels
  fb-clear
  3 5 1 fb-pixel!  3 5 fb-pixel@ s" set pixel" check
  3 5 0 fb-pixel!  3 5 fb-pixel@ 0= s" clear pixel" check
  127 63 1 fb-pixel! 127 63 fb-pixel@ s" corner" check
  200 5 1 fb-pixel! 200 5 fb-pixel@ 0= s" out of range ignored" check
  fb 1023 + c@ $80 = s" page layout" check ;
: t-glyph
  65 glyph@ $7BED = s" glyph A" check
  32 glyph@ 0= s" glyph space" check ;
: t-text
  fb-clear s" A" 0 0 cell-str
  0 0 fb-pixel@ 1 0 fb-pixel@ and 2 0 fb-pixel@ and s" A top row" check
  1 1 fb-pixel@ 0= s" A hole" check ;
0 fails !  t-pixels t-glyph t-text
fails @ 0= if ." ALL PASSED" else ." FAILURES: " fails @ . then cr bye
