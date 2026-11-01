# Unit 7 answers

Encodings below were derived from the format tables in `esp32/optional/assemblers/xtensa-assembler.fs` (ueforth's own assembler). Exercises 9–10 are where you confirm them on silicon.

1. `$802340`: op2 = 8, op1 = 0, r = 2, s = 3, t = 4, op0 = 0. Little-endian in memory: `40 23 80`.
2. `$FCC222`: imm8 = $FC (−4), r = $C, s = 2, t = 2, op0 = 2. In memory: `22 C2 FC`.
3. `ADD a2, a3, a4` and `ADDI a2, a2, -4`.
4. Xtensa has no condition flags; branches compare registers directly. `BEQI a3, 10, done` compares with an immediate and branches in one instruction (also `BEQ`, `BLT`, `BNEZ`, …). `BBSI` / `BBCI` branch on one bit being set or clear — handy for GPIO and status registers.
5. With `L32R`: it loads a 32-bit word from a literal pool at a PC-relative address *below* the instruction. The assembler or compiler places the constants in front of the function.
6. `CALL8` rotates the window by 8 registers, so the caller's `a10` becomes the callee's `a2` (the caller put arguments in a10, a11, …) and the caller's `a8` becomes the callee's `a0`, the return address. `ENTRY a1, 32` finishes the rotation, moves the stack pointer down by 32 bytes for the new frame. When a call would reuse physical registers still owned by an older frame, a *window overflow exception* spills them to that frame's stack area; returning into a spilled frame triggers *window underflow* to reload them. The 6502 never had this problem because it had almost no registers to save.
7. Something like:
   ```
   l32i  w,  ip, 0      ; w = *ip
   addi  ip, ip, 4      ; ip++
   l32i  t,  w,  0      ; t = *w   (the code field)
   jx    t              ; goto t
   ```
   GCC copies it to the end of each primitive (each `OP_x:` label ends in `NEXT`), so each primitive jumps straight to the next one instead of going back through a dispatch loop; that saves a branch per word and helps branch prediction on bigger CPUs.
8. `sp` arrives in `a2` (the first argument after `CALL8` rotation); the return value goes back in `a2`. For `+`:
   ```
   entry a1, 16
   l32i  a4, a2, 0      ; b = top
   addi  a2, a2, -4     ; drop one cell
   l32i  a5, a2, 0      ; a
   add   a5, a5, a4
   s32i  a5, a2, 0      ; a+b is the new top
   retw                  ; a2 is the new sp
   ```
   (Seven with `entry`; check the operand order your assembler expects before typing it.)
9–11. Report what you find; the objdump exercise is the one most worth bringing back for discussion.
