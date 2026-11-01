\ Unit 2 tests
T{ 7 signum -> 1 }T
T{ -7 signum -> -1 }T
T{ 0 signum -> 0 }T
T{ 5 0 10 within -> -1 }T
T{ 0 0 10 within -> -1 }T
T{ 10 0 10 within -> 0 }T
T{ -1 0 10 within -> 0 }T
T{ 15 0 10 clamp -> 10 }T
T{ -5 0 10 clamp -> 0 }T
T{ 5 0 10 clamp -> 5 }T
T{ 10 sum-to -> 55 }T
T{ 0 sum-to -> 0 }T
T{ 48 18 gcd -> 6 }T
T{ 17 5 gcd -> 1 }T
T{ $A5 0 bit@ -> 1 }T
T{ $A5 1 bit@ -> 0 }T
T{ $A5 7 bit@ -> 1 }T
T{ 255 bits -> 8 }T
T{ 0 bits -> 0 }T
T{ $A5 bits -> 4 }T
T{ 27 collatz-steps -> 111 }T
T{ 6 collatz-steps -> 8 }T
T{ 1 collatz-steps -> 0 }T
report bye
