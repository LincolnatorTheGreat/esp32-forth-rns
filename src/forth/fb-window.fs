\ Window display flush: sends the framebuffer to the oled-host SDL window as
\ ESC "OLED" + 1024 raw bytes on stdout. Uses default-type so it bypasses the
\ console's redirected TYPE.

create frame-mark 27 c, 79 c, 76 c, 69 c, 68 c,

: fb-flush-win ( -- ) frame-mark 5 default-type fb FB-BYTES default-type ;
