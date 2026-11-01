# Unit 5 drills: execution tokens and vectors

An execution token (xt) is a number that stands for a word, like a function pointer.
Answers: `course/answers/unit5.md`.

1. `' dup .`
2. `3 ' dup execute .s`
3. `: sq dup * ;  5 ' sq execute .`
4. `defer act  ' 1+ is act  5 act .`
5. `' 1- is act  5 act .`
6. `: run ( n xt -- ) execute ;  7 ' . run`
7. `: plus ['] + ;  3 4 plus execute .`
8. `defer nothing-yet  nothing-yet`
9. `create ops ' + , ' - , ' * ,  6 3 ops 2 cells + @ execute .`
10. `' dup ' over = .`
11. `:noname 2 * ; constant dbl  21 dbl execute .`
12. `defer say  ' type is say  s" hi" say`
13. `: hello ." hello" ;  ' hello >body .`   (predict: does this make sense?)
14. `: twice-do ( xt -- ) dup execute execute ;  ' cr twice-do`
