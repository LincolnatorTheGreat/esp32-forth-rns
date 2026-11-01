# Unit 3 drill answers

1. `42`. `variable` makes a word that pushes its own address; `!` stores into it.
2. `47`. `+!` is a read-modify-write: `( n addr -- )`.
3. Phone / ESP32: `4`. Host: `8`.
4. Phone / ESP32: `4`. Host: `8`. `,` stores one cell at `here` and advances `here` by a cell.
5. `1`. `c,` stores one byte.
6. `30`. Cell index 2 is at byte offset `2 cells`.
7. `20`. `cell+` is `1 cells +`.
8. `78`. Both the ESP32 and x86 are little-endian: the low byte sits at the lowest address.
9. `12`. On the host the cell is 8 bytes, so `x 3 +` still lands on `$12`; the upper four bytes are zeros.
10. `AAAAAAAA`. `fill ( addr n byte -- )`, and 65 is `A`.
11. `-1`. For a `create` word, the body is the data field, and that is exactly what the word pushes.
12. The bytes `0A 00 00 00 14 00 00 00 1E 00 00 00` (host: 8 bytes per value, so 24 bytes with zeros). The address column is wherever `arr` landed.
13. Host: around `40`, not `8`. Between `a`'s data and `b`'s data sits `b`'s dictionary header: its name, link to the previous word, flags and code field. The phone REPL keeps headers outside memory and prints `4`. You will take ueforth's header apart in Unit 8.
14. Host: `see v` shows a variable; `see twice` shows `: twice dup + ;`. The phone prints its own simplified form with op numbers, including `exit` at the end, which is what `;` compiles.
15. `0`. `align` rounds `here` up to a cell boundary. (On the host, `here 7 and` would be the stricter check.)
16. `hello`. A counted string: one length byte, then the characters. `count ( addr -- addr+1 len )` unpacks it.
