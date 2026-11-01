# Unit 4 drill answers

1. `5`. `s"` leaves `( addr len )`; `nip` drops the address.
2. `hello`.
3. `ello`. Move the start forward one and shorten by one: that's how you "slice" a string.
4. `hel`. Shortening the length trims the end.
5. `65`, then `65`. `char` takes the first character of the next word.
6. `32`.
7. `Hi Hi`.
8. `a`.
9. `99`, the code for `c`.
10. `AB`, a newline, then `C`.
11. `HEY`. Lowercase and uppercase ASCII differ by exactly 32.
12. Three spaces, then `x`.
13. `**`. `[char]` is `char` done at compile time.
14. `<4> a1 a2 3 2`, with two addresses first. `rot` pulled `a2` up past the lengths, splitting both strings apart. Two strings on the stack are four cells: plan the shuffling before you write it.
15. `abc`.
16. `h`. `addr len +` points one past the end.

Note on the host: `s"` at the prompt copies the string into the dictionary at `here`, so each one permanently uses a little memory. Inside a definition the string is compiled into the word.
