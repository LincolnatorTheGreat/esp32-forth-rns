# Unit 2 drill answers

1. `0 1 2 3 4`. Start at 0, stop before the limit 5.
2. Prints `5 6 7 8 …` and runs about 4.3 billion times on the ESP32 (2³² − 5), effectively forever on the 64-bit host. `loop` exits only when the index *crosses* the boundary between limit − 1 and limit, and counting up from 5 reaches 0 only by wrapping around.
3. `0 3 6 9`.
4. `10 7 4 1`. With a negative step, `+loop` exits when the index crosses below the limit; it would include 0 itself if the steps landed on it (`0 10 do i . -1 +loop` prints 10 down to 0).
5. `0 0 0 1 1 0 1 1 2 0 2 1`. `j` is the outer index.
6. `0 1 2 3`. `leave` jumps past `loop` immediately.
7. `0 1 2`. `until` loops back while the flag is false.
8. `yes no yes`. `if` treats any non-zero value as true.
9. `3 2 1`.
10. `-1`. Both comparisons give −1, and −1 AND −1 = −1.
11. `0`, then `-1`. `1` and `2` are both "true" to `if`, but 1 AND 2 = 0. Normalize with `0<>` before using `and`/`or` on numbers that aren't flags.
12. Nothing. `?do` skips the body when limit = start.
13. `x`. Plain `do` always runs the body at least once; without the `leave`, `0 0 do` would run 2³² times.
14. `<3> 0 1 2`. Each pass leaves its index on the data stack.
15. `2`. Only even indexes in 1, 2, 3.
16. `5 5`. Inside the loop `i` reads the top of the return stack, which is now your 5, not the index. Never use `i` or `j` while you have something pushed with `>r`.
