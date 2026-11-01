# Unit 6 drills: defining words and compile time

Each line runs in two phases: what happens while it compiles, and what happens when it runs.
Predict both. Answers: `course/answers/unit6.md`.

1. `: konst create , does> @ ;  7 konst seven  seven .`
2. `: fives create 5 , does> @ * ;  fives times5  3 times5 .`
3. `: counter create 0 , does> 1 over +! @ ;  counter c1  c1 c1 c1 .`   (and what's left?)
4. `: noisy ." compiling! " ; immediate  : t noisy ;`
5. `t`
6. `: t2 [ 3 4 + ] literal ;  t2 .`
7. `: square postpone dup postpone * ; immediate  : t3 square ;  6 t3 .`
8. `see t3`
9. `: my-if postpone if ; immediate  : t4 my-if ." yes " then ;  1 t4  0 t4`
10. `: t5 [ state @ ] literal ;  t5 .`
11. `: t6 state @ ;  t6 .`
12. `create pt 3 , 4 ,  : .y cell+ @ ;  pt .y .`
13. `' seven >body @ .`
14. `99 ' seven >body !  seven .`
