include src/forth/load.fs
fb-clear
s" ESP32 FORTH RNS" 0 0 cell-str
s" 3x5 font test" 0 1 cell-str
s" abcdefghijklmnopqrstuvwxyz" 0 3 cell-str
s" 0123456789 !?.,:;+-*/=<>" 0 4 cell-str
fb-flush
