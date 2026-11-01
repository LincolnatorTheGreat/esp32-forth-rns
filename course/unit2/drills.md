# Unit 2 drills: control flow

Predict what each line prints, then run it. `do` takes `( limit start -- )`.
Answers: `course/answers/unit2.md`.

1. `: a 5 0 do i . loop ;  a`
2. `: b 0 5 do i . loop ;  b`   (predict only; on the host be ready to press Ctrl-C)
3. `: c 10 0 do i . 3 +loop ;  c`
4. `: d 0 10 do i . -3 +loop ;  d`
5. `: e 3 0 do 2 0 do j . i . loop loop ;  e`
6. `: f 10 0 do i 4 = if leave then i . loop ;  f`
7. `: g 0 begin dup . 1+ dup 3 = until drop ;  g`
8. `: h if ." yes " else ." no " then ;  5 h  0 h  -1 h`
9. `: s 3 begin dup while dup . 1- repeat drop ;  s`
10. `5 3 > 2 1 > and .`
11. `1 2 and .`   then   `1 0<> 2 0<> and .`
12. `: z 0 0 ?do ." x" loop ;  z`
13. `: y 0 0 do ." x" leave loop ;  y`
14. `: w 3 0 do i loop ;  w .s`
15. `: v 4 1 do i 2 mod 0= if i . then loop ;  v`
16. `: u 2 0 do 5 >r i . r> drop loop ;  u`
