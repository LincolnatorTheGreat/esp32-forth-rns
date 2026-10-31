\ Entry point for the windowed REPL: run via tools/oled (needs the oled-host SDL window).
include load.fs
include fb-window.fs
include console.fs
' fb-flush-win is fb-flush
console-on
