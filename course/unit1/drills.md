# Unit 1 drills: stack mechanics

For each line, write down what it prints (or what `.s` shows) **before** you run it.
Then run it in the terminal REPL and mark each miss. Answers: `course/answers/unit1.md`.

1. `1 2 3 swap . . .`
2. `1 2 over . . .`
3. `1 2 3 -rot .s`
4. `1 2 3 rot rot .s`
5. `5 2 - .`   then   `2 5 - .`
6. `17 5 /mod . .`
7. `-17 5 /mod . .`
8. `100 3 / 3 * .`   then   `100 3 * 3 / .`
9. `7 dup dup * * .`
10. `1 2 2dup + + + .`
11. `3 4 nip .`   then   `3 4 tuck .s`
12. `-1 u.`
13. `hex ff decimal .`   then   `255 hex . decimal`
14. `2000000000 3 * 4 / .`   then   `2000000000 3 4 */ .`
15. `1 2 3 4 .s 2drop .s 2drop .s`
16. `: 3rd ( a b c -- a b c a ) >r over r> swap ;  10 20 30 3rd .s`

Two of these behave differently on the ESP32 than on your 64-bit Linux host. Find them.
