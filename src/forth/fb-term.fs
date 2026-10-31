\ Host-only display flush: draws the framebuffer in the terminal using
\ half-block characters (2 pixel rows per text row, 128 cols x 32 rows).
\ Replaced by an I2C SSD1306 flush on hardware.

: u8emit ( b3 b2 b1 -- ) emit emit emit ;
: half-block ( top bottom -- )
  2dup and if 2drop $88 $96 $E2 u8emit exit then
  over if 2drop $80 $96 $E2 u8emit exit then
  if $84 $96 $E2 u8emit exit then
  32 emit ;

: fb-flush-term ( -- )
  27 emit ." [H"
  FB-H 0 do
    FB-W 0 do
      i j       fb-pixel@
      i j 1+    fb-pixel@
      half-block
    loop cr
  2 +loop ;

' fb-flush-term is fb-flush
