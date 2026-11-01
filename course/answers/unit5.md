# Unit 5 drill answers

1. A number: the word's execution token. On the host it is a memory address (the word's code field); on the phone it is just an index.
2. `<2> 3 3`.
3. `25`.
4. `6`.
5. `4`. Changing a deferred word changes every word that already calls it. That is how `console.fs` reroutes `type` and `key`.
6. `7`.
7. `7`. `[']` is `'` done at compile time: it compiles the xt as a literal.
8. An error. A `defer` starts with no target; ueforth's `defer` is literally `create 0 , does> @ dup 0= throw execute`, so it throws on 0.
9. `18`. Index 2 of the table is `*`.
10. `0`. Different words, different tokens.
11. `42`. `:noname` compiles a word with no name and leaves its xt.
12. `hi`.
13. (host) An address: in ueforth, `>body` of a colon word points at its compiled list of execution tokens, the "thread". You'll walk one in Unit 8. The phone refuses, because it doesn't store colon words in memory.
14. Two blank lines.
