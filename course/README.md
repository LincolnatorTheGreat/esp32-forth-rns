# Forth course

Nine units, from stack drills to reading ueforth's interpreter and the Xtensa code under it.
The plan for each unit lives in the course document; this folder holds the material you work in.

| Path | What it is |
|---|---|
| `unitN/drills.md` | predict-then-run lines (Units 1–6) |
| `unitN/work.fs` | word stubs with stack comments; you fill these in |
| `unitN/test.fs` | the grader for that unit |
| `unit7/exercises.md`, `unit8/exercises.md` | Xtensa primer and ueforth internals |
| `answers/` | answer keys: look after a real attempt |
| `lib/tester.fs` | `T{ ... -> ... }T`, a small Hayes-style tester |
| `run.sh` | `course/run.sh N` grades your unit N; `course/run.sh N key` grades the answer key |

Run everything from the repository root. Build the host REPL first (see the top-level README).

The answer keys were checked against the tests on a JavaScript imitation of ueforth with 32-bit cells.
Run `for n in 1 2 3 4 5 6 8; do course/run.sh $n key; done` once on your host build; every line should end `0 failed`.
