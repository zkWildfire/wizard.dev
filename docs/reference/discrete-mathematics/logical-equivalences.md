# Logical Equivalences
## Terminology
### Logical Equivalence
Two statements are said to be **logically equivalent** if they have the same
truth table. For example, the statements $p \land q$ and $q \land p$ are
logically equivalent because they have the same truth table.

### Tautology
A statement is said to be a **tautology** if it is always true, or in other
words, if it has a truth table that is always true. For example, the statement
$p \lor \neg p$ is a tautology because it evaluates to true for all rows in
its truth table.

| $p$ | $\neg p$ | $p \lor \neg p$ |
|:---:|:--------:|:---------------:|
| $T$ | $F$ | $T$ |
| $F$ | $T$ | $T$ |

### Contradiction
A statement is said to be a **contradiction** if it is always false, or in other
words, if it has a truth table that is always false. For example, the statement
$p \land \neg p$ is a contradiction because it evaluates to false for all rows
in its truth table.

| $p$ | $\neg p$ | $p \land \neg p$ |
|:---:|:--------:|:----------------:|
| $T$ | $F$ | $F$ |
| $F$ | $T$ | $F$ |

### Contingency
A statement is said to be a **contingency** if it is neither a tautology nor a
contradiction, or in other words, if it has a truth table that is neither always
true nor always false. For example, the statement $p \lor q$ is a contingency
because it evaluates to true for some rows in its truth table and false for
others.

| $p$ | $q$ | $p \lor q$ |
|:---:|:---:|:----------:|
| $T$ | $T$ | $T$ |
| $T$ | $F$ | $T$ |
| $F$ | $T$ | $T$ |
| $F$ | $F$ | $F$ |

## Symbols
### $A = B$
Using a standard equals sign means that the two sides are **character for
character** equivalent, or "stringly equivalent".

For example, both sides of $p \land q = q \land p$ have the same truth tables.
However, the statement $p \land q = q \land p$ is not true because the two sides
are not character for character equivalent.

### $A \equiv B$
The triple equals sign signifies **logical equivalence**, or in other words, the
two sides have the same truth tables. This is typically what you want to use
when proving logical equivalences.

Note that $A \equiv B$ and $(A \iff B) \equiv T$ are the same thing.

## Rules
### Identity
* $p \land T \equiv p$
* $p \lor F \equiv p$

### Domination
* $p \lor T \equiv T$
* $p \land F \equiv F$

### Idempotent
* $p \lor p \equiv p$
* $p \land p \equiv p$

### Negation
* $p \lor \neg p \equiv T$
* $p \land \neg p \equiv F$

### Double Negation
* $\neg \neg p \equiv p$

### Commutative
* $p \lor q \equiv q \lor p$
* $p \land q \equiv q \land p$

### Associative
* $(p \lor q) \lor r \equiv p \lor (q \lor r)$
* $(p \land q) \land r \equiv p \land (q \land r)$

### Distributive
* $p \land (q \lor r) \equiv (p \land q) \lor (p \land r)$
* $p \lor (q \land r) \equiv (p \lor q) \land (p \lor r)$

### Absorption
* $p \lor (p \land q) \equiv p$
* $p \land (p \lor q) \equiv p$

### De Morgan's Laws
* $\neg (p \lor q) \equiv \neg p \land \neg q$
* $\neg (p \land q) \equiv \neg p \lor \neg q$

### Law of Implication
* $p \implies q \equiv \neg p \lor q$

### Contrapositive
* $p \implies q \equiv \neg q \implies \neg p$

### Biconditional
* $p \iff q \equiv (p \implies q) \land (q \implies p)$

## Additional Equivalences
These are other equivalences that are listed in the Rosen textbook. Proofs for
each of these equivalences can be found in appendix 1.

### Equivalences Involving Conditional Statements
* $p \land q \equiv \neg (p \implies \neg q)$
* $(p \implies q) \land (p \implies r) \equiv p \implies (q \land r)$
* $(p \implies r) \land (q \implies r) \equiv (p \lor q) \implies r$
* $(p \implies q) \lor (p \implies r) \equiv p \implies (q \lor r)$
* $(p \implies r) \lor (q \implies r) \equiv (p \land q) \implies r$

### Equivalences Involving Biconditional Statements
* $p \iff q \equiv \neg p \iff \neg q$
* $p \iff q \equiv (p \land q) \lor (\neg p \land \neg q)$
* $\neg (p \iff q) \equiv p \iff \neg q$
