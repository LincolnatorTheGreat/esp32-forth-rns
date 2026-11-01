# Unit 6 drill answers

1. `7`. `create ,` runs when you *define* `seven`; `does> @` runs every time you *call* it, with the body address on the stack.
2. `15`.
3. Prints `3` and leaves `1 2`. Each call increments the stored cell and returns the new value.
4. Prints `compiling! ` while `t` is being defined. An immediate word runs at compile time instead of being compiled.
5. Nothing. `noisy` ran during compilation and left nothing in `t`.
6. `7`. `[` switches to interpreting, `3 4 +` runs then, `]` switches back, and `literal` compiles the 7.
7. `36`. `square` is a macro: it compiles `dup *` straight into `t3`.
8. `: t3 dup * ;` (the phone shows numbered ops). There is no call to `square` in `t3`.
9. `yes `. `my-if` compiled a real `if` into `t4`.
10. `0`. By the time `state @` runs, `[` has already switched to interpreting.
11. `0`. `t6` runs at the prompt, where state is interpreting.
12. `4`. `.y` is a field accessor written by hand; `field` in work.fs generates these.
13. `7`. The body of a `create`d word is its data.
14. `99`. You just rewrote a "constant". Nothing in memory is read-only.
