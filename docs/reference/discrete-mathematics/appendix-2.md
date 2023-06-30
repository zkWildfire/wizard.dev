# Appendix 2: Truth Tables for Logical Equivalences
## Overview
For some logical equivalences (e.g. XORs), no named equivalences were defined
for the operation. As a result, truth tables are used to prove equivalences
instead of proofs. These truth tables are tracked here.

| $p$ | $q$ | $p \oplus q$ |
|:---:|:---:|:------------:|
| $T$ | $T$ | $F$ |
| $T$ | $F$ | $T$ |
| $F$ | $T$ | $T$ |
| $F$ | $F$ | $F$ |

## Equivalences Involving XOR
### $p \oplus q \equiv (p \lor q) \land \neg (p \land q)$
| $p$ | $q$ | $p \lor q$ | $p \land q$ | $\neg (p \land q)$ | $(p \lor q) \land \neg (p \land q)$ |
|:---:|:---:|:----------:|:-----------:|:------------------:|:-----------------------------------:|
| $T$ | $T$ | $T$ | $T$ | $F$ | $F$ |
| $T$ | $F$ | $T$ | $F$ | $T$ | $T$ |
| $F$ | $T$ | $T$ | $F$ | $T$ | $T$ |
| $F$ | $F$ | $F$ | $F$ | $T$ | $F$ |

### $p \oplus q \equiv (p \land \neg q) \lor (\neg p \land q)$
| $p$ | $q$ | $\neg q$ | $p \land \neg q$ | $\neg p$ | $\neg p \land q$ | $(p \land \neg q) \lor (\neg p \land q)$ |
|:---:|:---:|:--------:|:----------------:|:--------:|:----------------:|:----------------------------------------:|
| $T$ | $T$ | $F$ | $F$ | $F$ | $F$ | $F$ |
| $T$ | $F$ | $T$ | $T$ | $F$ | $F$ | $T$ |
| $F$ | $T$ | $F$ | $F$ | $T$ | $T$ | $T$ |
| $F$ | $F$ | $T$ | $F$ | $T$ | $F$ | $F$ |
