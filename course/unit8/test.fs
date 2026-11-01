\ Unit 8 tests
T{ char 7 >digit char x >digit -> 7 -1 }T
: t-n1 s" 1234" >num ;
: t-n2 s" 12a" >num ;
T{ t-n1 -> 1234 -1 }T
T{ t-n2 nip -> 0 }T
: t-sk s"    ab" skip nip ;
T{ t-sk -> 2 }T
: t-nw s" hi there" next-word nip nip nip ;
T{ t-nw -> 2 }T
: t-mi1 s" 3 4 + dup *" my-interpret ;
: t-mi2 s"   2 3 swap -  " my-interpret ;
: t-mi3 s" 10 20 30 rot" my-interpret ;
T{ t-mi1 -> 49 }T
T{ t-mi2 -> 1 }T
T{ t-mi3 -> 20 30 10 }T
report bye
