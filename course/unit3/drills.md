# Unit 3 drills: memory and the dictionary

Run these in order in one session; later lines reuse earlier names.
Answers: `course/answers/unit3.md`. Lines marked (host) give a different answer on the phone REPL; the answer key explains why.

1. `variable v  42 v !  v @ .`
2. `5 v +!  v @ .`
3. `cell .`
4. `here 10 , here swap - .`
5. `here 7 c, here swap - .`
6. `create arr 10 , 20 , 30 ,  arr 2 cells + @ .`
7. `arr cell+ @ .`
8. `create x $12345678 ,  x c@ hex . decimal`
9. `x 3 + c@ hex . decimal`
10. `create buf 8 allot  buf 8 65 fill  buf 8 type`
11. `' arr >body  arr = .`
12. `arr 3 cells dump`
13. `variable a  variable b  b a - .`   (host)
14. `see v`   then   `: twice dup + ;  see twice`   (host)
15. `align here 3 and .`
16. `create msg 5 c, char h c, char e c, char l c, char l c, char o c,  msg count type`
