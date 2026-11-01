# Unit 1 drill answers

Each drill assumes an empty stack. ueforth prints ` ok`, then the stack, after every line.

1. `2 3 1`. Stack after swap is `1 3 2`; `.` pops the top first.
2. `1 2 1`.
3. `<3> 3 1 2`. `-rot` is ( a b c -- c a b ): it sends the top down two.
4. `<3> 3 1 2`. Two `rot`s equal one `-rot`.
5. `3`, then `-3`. The second operand is the top of the stack.
6. `3 2`. `/mod` leaves ( rem quot ), quotient on top, so it prints first.
7. `-4 3`. ueforth's division is *floored*: −17 = 5 × (−4) + 3. A C-style truncating division would give −3 and −2.
8. `99`, then `100`. Divide last when you can.
9. `343`.
10. `6`. After `2dup` the stack is `1 2 1 2`.
11. `4`, then `ERROR: tuck NOT FOUND!`. ueforth has `nip` but no `tuck`; you write it in work.fs.
12. ESP32: `4294967295`. Host (64-bit cells): `18446744073709551615`. **Difference #1.**
13. `255`, then `FF`. `hex` changes how *both* input and output are read.
14. ESP32: `426258176` then `1500000000`. Host: `1500000000` both times. `2000000000 3 *` overflows a 32-bit cell; `*/` keeps a double-width intermediate. **Difference #2.** Use `*/` for scaling on the board.
15. `<4> 1 2 3 4`, `<2> 1 2`, `<0>`.
16. `<4> 10 20 30 10`. Watch `>r over r> swap` move: `10 20` → `10 20 10` → `10 20 10 30` → `10 20 30 10`.

Tip: the phone REPL has a Clear button. On the host, define `: clear begin depth while drop repeat ;`.
