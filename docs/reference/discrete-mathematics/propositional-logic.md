# Propositional Logic
## Terminology
### Proposition
Propositions are statements that are:

* Either true or false
* "Well-formed"

With respect to being "well-formed", it's important to note that propositions
may not contain variables. For example, the following statements are not
propositions:

```c linenums="0"
// Rosen, Chapter 1 Example 2
x + 1 = 2

// CSE311 19au Lecture 1 (modified)
x + 2 = 1236
```

In the case of the second example, the statement is not a proposition because
`x` is variable. If we instead constrain `x` to a single value by making it an
alias rather than a variable, then the statement can be turned into a
proposition:

```c linenums="0"
// CSE311 19au Lecture 1
x + 2 = 1236, where x is your PIN number
```

English sentences that are not statements that may be true or false are also
not propositions:
```c linenums="0"
// Rosen, Chapter 1 Example 2
What time is it?
Read this carefully
```

### Propositional Variables
Propositional variables are variables that represent a proposition, and
therefore may only be true or false. Propositional variables are used to compose
more complex propositions, e.g. $p \land q$, $p \implies q$, etc. Propositional
variables are typically lower-case letters, e.g. $p$, $q$, $r$, etc.

### Logical Connectives
Logical connectives are used to combine propositions into more complex
propositions. The most common logical connectives are listed below:

| Name | Symbol | Example |
|:-----|:------:|:-------:|
| Negation | $\neg$ | $\neg p$ |
| Conjunction | $\land$ | $p \land q$ |
| Disjunction | $\lor$ | $p \lor q$ |
| Exclusive Disjunction (XOR) | $\oplus$ | $p \oplus q$ |
| Implication | $\implies$ | $p \implies q$ |
| Biconditional | $\iff$ | $p \iff q$ |

### Implications
Implications are propositions of the form $p \implies q$, where $p$ and $q$ are
propositions. It's important to not think of implications as true statements,
but rather as a claim where the speaker may or may not be lying. For example,
given the implication "If it is raining, then I have my umbrella", the
implication is true if it is raining and I have my umbrella. However, if it is
raining and I don't have my umbrella, then the implication is false.

In an implication of the form $p \implies q$, $p$ is called the **hypothesis**
and $q$ is called the **conclusion** or **consequence**. Note that if the
hypothesis is false, then the implication is true regardless of the truth value
of the conclusion.

A useful way of thinking about implications is to consider them to be an
obligation or contract. For example, if a professor says "If you get 100% on
the final, then you will get an A", you would expect to get an A if you get
100% on the final. If you don't get 100% on the final, then the professor is
not obligated to give you an A. You could still get an A, but the professor is
not obligated to give you one. If however you get 100% on the final and the
professor does not give you an A, then the professor has broken the contract
(aka the implication is false).

### Inconsistencies
When writing system specifications, a specification is said to be **inconsistent**
if it is impossible for all of the propositions in the specification to be true
at the same time. For example, the following specification is inconsistent:

$$
\begin{align}
p \land \neg p
\end{align}
$$

Note that a specification is not inconsistent if there exists a combination
of propositional variables that makes one or more statements false. A
specification is only inconsistent if there does not exist a combination of
propositional variables that makes all statements true.

## Truth Tables
Truth tables are used to determine the truth value of a proposition given the
truth values of its propositional variables. Truth tables for common
propositions are listed below:

### Negation
| $p$ | $\neg p$ |
|:---:|:--------:|
| $T$ | $F$ |
| $F$ | $T$ |

### Conjunction
| $p$ | $q$ | $p \land q$ |
|:---:|:---:|:-----------:|
| $T$ | $T$ | $T$ |
| $T$ | $F$ | $F$ |
| $F$ | $T$ | $F$ |
| $F$ | $F$ | $F$ |

### Disjunction
| $p$ | $q$ | $p \lor q$ |
|:---:|:---:|:----------:|
| $T$ | $T$ | $T$ |
| $T$ | $F$ | $T$ |
| $F$ | $T$ | $T$ |
| $F$ | $F$ | $F$ |

### Exclusive Disjunction (XOR)
| $p$ | $q$ | $p \oplus q$ |
|:---:|:---:|:-----------:|
| $T$ | $T$ | $F$ |
| $T$ | $F$ | $T$ |
| $F$ | $T$ | $T$ |
| $F$ | $F$ | $F$ |

### Implication
| $p$ | $q$ | $p \implies q$ |
|:---:|:---:|:--------------:|
| $T$ | $T$ | $T$ |
| $T$ | $F$ | $F$ |
| $F$ | $T$ | $T$ |
| $F$ | $F$ | $T$ |

### Biconditional
| $p$ | $q$ | $p \iff q$ |
|:---:|:---:|:----------:|
| $T$ | $T$ | $T$ |
| $T$ | $F$ | $F$ |
| $F$ | $T$ | $F$ |
| $F$ | $F$ | $T$ |

## Converse, Contrapositive, Inverse
Given an implication $p \implies q$, the **converse** of the implication is
$q \implies p$. The **contrapositive** of the implication is $\neg q \implies
\neg p$. The **inverse** of the implication is $\neg p \implies \neg q$. Or,
in table form:

| Name | Example |
|:----:|:-------:|
| Implication | $p \implies q$ |
| Converse | $q \implies p$ |
| Contrapositive | $\neg q \implies \neg p$ |
| Inverse | $\neg p \implies \neg q$ |

Note the truth tables of these four propositions:

| p | q | $p \implies q$<br>(Implication) | $q \implies p$<br>(Converse) | $\neg q \implies \neg p$<br>(Contrapositive) | $\neg p \implies \neg q$<br>(Inverse) |
|:-:|:-:|:--------------:|:--------------:|:------------------------:|:------------------------:|
| $T$ | $T$ | $T$ | $T$ | $T$ | $T$ |
| $T$ | $F$ | $F$ | $T$ | $F$ | $T$ |
| $F$ | $T$ | $T$ | $F$ | $T$ | $F$ |
| $F$ | $F$ | $T$ | $T$ | $T$ | $T$ |

Notice how the implication and its contrapositive have the same truth values,
and the converse and inverse have the same truth values.

## Translating English to Propositional Logic
!!! warning
    English is inherently imprecise and/or ambiguous. As a result, the rules
    listed in this section may not apply to all statements of the forms listed
    below. Carefully consider sentence context and/or prior knowledge when
    translating English to propositional logic.

### Implications
* if $p$, then $q$
* if $p$, $q$
* $p$ is sufficient for $q$
* $q$ if $p$
* $q$ when $p$
* a necessary condition for $p$ is $q$
* $q$ unless $\neg p$
* $p$ implies $q$
* $p$ only if $q$
* a sufficient condition for $q$ is $p$
* $q$ whenever $p$
* $q$ is necessary for $p$
* $q$ follows from $p$
* whenever $p$ is true, $q$ must be true

### Negation
* unless $p$

### Conjunctions
* $p$ and $q$

### Disjunctions
* $p$ or $q$

!!! warning
    In English, disjunctions are frequently ambiguous between inclusive and
    exclusive disjunctions. When translating disjunctions to propositional
    logic, the context of the sentence must always be taken into account to
    decide whether the disjunction is inclusive or exclusive.

### Biconditionals
* $p$ if and only if $q$
* $p$ implies $q$ and $q$ implies $p$
* $p$ is necessary and sufficient for $q$

### Miscellaneous
| English | Propositional Logic |
|:-------:|:------------------:|
| $p$ only if $q$ or not $r$ | $p \implies (q \lor \neg r)$ |
| not $p$ if $q$ unless $r$ | $(q \land \neg r) \implies \neg p$ |
| not $p$ when $q$ | $q \implies \neg p$ |
| unless $p$, $q$ | $\neg p \implies q$ |
| if $p$, $q$. otherwise, $r$. | $(p \implies q) \land (\neg p \implies r)$ |

## Sources
* [UW CSE311](https://cs.uw.edu/311)
* Discrete Mathematics and Its Applications (Rosen), Chapter 1.1
