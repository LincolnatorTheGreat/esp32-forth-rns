# Unit 7 exercises: the Xtensa LX7 underneath

Paper exercises first, then three on the real board. Answers: `course/answers/unit7.md`.
Keep the ISA reference open: https://github.com/espressif/xtensa-isa-doc/releases/download/latest/Xtensa.pdf

## On paper

1. **Encode `ADD a2, a3, a4`.** It's an RRR-format instruction: `op2[23:20] op1[19:16] r[15:12] s[11:8] t[7:4] op0[3:0]`, with op2 = 8, op1 = 0, op0 = 0, result in r, sources in s and t. Give the 24-bit value and the three bytes in memory order.
2. **Encode `ADDI a2, a2, -4`.** RRI8 format: `imm8[23:16] r[15:12] s[11:8] t[7:4] op0[3:0]`, with op0 = 2, r = $C, destination in t, source in s.
3. **Decode** the bytes `40 23 80` and `22 C2 FC` back into instructions. (You just made them.)
4. **No flags register.** Rewrite this 6502 idea in Xtensa terms: `CMP #10 / BEQ done`. Which single Xtensa instruction does the compare and the branch? Which one tests one bit of a register and branches?
5. **Constants.** `MOVI` holds a 12-bit signed immediate. How does code load `$3FF48000` (a peripheral address)? Look up `L32R` and say where its data lives.
6. **Register windows.** A function executes `CALL8` to call another. In the callee, which of the caller's registers is the callee's `a2`? Its `a0`? What does the callee's `ENTRY a1, 32` do, and what happens when the calls nest so deep that the 64 physical registers run out?
7. **NEXT in machine code.** ueforth's inner interpreter is `w = *ip++; goto **w;` in C. With `ip` and `w` in registers, write the four Xtensa instructions you expect GCC to emit. (Hint: two loads, an add, an indirect jump.)
8. **The code-word contract.** From `common/calls.h`: a code word is called as a C function `sp = f(sp, &fp)` after the top of stack is flushed to memory, with the stack growing upward. In the windowed ABI, which register holds `sp` on entry, and which register must hold the new `sp` at `RETW`? Sketch the six instructions of a code word for `+`.

## On the board

These need ESP32Forth built for the Heltec V3 with the optional assemblers module (`esp32/optional/assemblers/` copied next to the `.ino` before building).

9. Load the assembler (`xtensa-assembler`), assemble your `ADD` from exercise 1 into code space, and `dump` it. Do the bytes match your hand encoding?
10. Write the `+` code word from exercise 8 with `code … end-code`, test it against the real `+`, and disassemble it.
11. Build ESP32Forth with the Arduino toolchain, keep the `.elf`, and run `xtensa-esp32s3-elf-objdump -d` on it. Find `forth_run` and locate several copies of your four-instruction NEXT. Why is it copied after every primitive instead of jumped to?
