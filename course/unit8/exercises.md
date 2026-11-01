# Unit 8 reading and exploration: inside ueforth

All of this happens on the host REPL, with the ueforth source open (`third_party/ueforth`).

1. **The inner interpreter.** Read `common/interp.h`. Find `NEXT`, the register variables (`ip rp sp tos w fp`), and why `tos` is a separate register. What does `EXECUTE` do in one line?
2. **The bootstrap tiers.** Read `common/tier0_opcodes.h`, then `common/tier1a_forth.fs`. List five words that are C opcodes and five defined in Forth on top of them. Find the stack comment on `-rot` in `tier1a_forth.fs` and check it against what the word actually does.
3. **Control flow is just Forth.** In `common/boot.fs`, read `DO`, `?DO`, `LOOP`, `+LOOP`, `LEAVE` and `UNLOOP`. They juggle the return stack with `r>` and `>r` around their own return address. Trace the return stack, cell by cell, for `: t 3 0 do i . loop ;`. Then explain your Unit 2 drill 16 (`5 >r i`) from the source.
4. **`defer` and `value`.** In `boot.fs`, find the one-line definitions of `defer`, `is`, `value` and `to`.
5. **A header, byte by byte.** Define `variable a variable b` and `: twice dup + ;`. Use `' twice`, `>name`, `>link`, `>params`, `>size` and `dump` to map one dictionary entry: name bytes, link, flags, length, code field, body. Then explain Unit 3 drill 13's number.
6. **Walk a thread.** `' twice >body 3 cells dump`. Match each cell to `' dup`, `' +` and `' exit`. Then do the same for `: k 1000 + ;` and find how the literal is stored.
7. **Write your own outer interpreter** in `course/unit8/work.fs` (`course/run.sh 8`). It finds and executes words and converts decimal numbers. The one design question is in the file's last comment.
8. **Milestone 3b: a custom line editor** for the OLED console: cursor left/right, insert, backspace, and a one-line history recall. Use your own `my-interpret` as its back end once it works.
