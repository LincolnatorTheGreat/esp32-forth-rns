# Unit 4 drills: strings and text

A string on the stack is two cells: `( addr len )`. Answers: `course/answers/unit4.md`.

1. `s" hello" nip .`
2. `s" hello" type`
3. `s" hello" swap 1+ swap 1- type`
4. `s" hello" 2 - type`
5. `char A .`   then   `char Apple .`
6. `bl .`
7. `: greet ." Hi " ;  greet greet`
8. `s" abc" drop c@ emit`
9. `s" abc" drop 2 + c@ .`
10. `65 emit 66 emit cr 67 emit`
11. `: shout ( a n -- ) 0 ?do dup i + c@ 32 - emit loop drop ;  s" hey" shout`
12. `3 spaces ." x"`
13. `: star [char] * emit ;  star star`
14. `s" ab" s" xyz" rot .s`
15. `create name 3 c, char a c, char b c, char c c,  name count type`
16. `: last-char ( a n -- c ) + 1- c@ ;  s" forth" last-char emit`
