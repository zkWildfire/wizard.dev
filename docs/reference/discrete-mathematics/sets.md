# Sets
## Definition
Sets are unordered collection collections of objects called **elements**. To
indicate that an object exists in a set, the symbol $\in$ is used. For example,
if $x$ is an element of the set $A$, then $x \in A$. If an element $x$ is not
in a set $A$, then $x \notin A$.

## Common Sets
| Name of the Set | Symbol | Definition |
|:---------------:|:------:|:----------:|
| Natural numbers | $\mathbb{N}$ | $\{1, 2, 3, \dots\}$ |
| Integers | $\mathbb{Z}$ | $\{\dots, -2, -1, 0, 1, 2, \dots\}$ |
| Rational numbers | $\mathbb{Q}$ | $\{\frac{a}{b} \mid a, b \in \mathbb{Z}, b \neq 0\}$ |
| Real numbers | $\mathbb{R}$ | |
| $[n]$ | N/A | $\{1, 2, \dots, n\}$ where $n$ is a natural number |
| The empty set | $\emptyset$ | $\{\}$ |

## Building Sets from Predicates
Let $S$ be the set of all $x$ for which $P(x)$ is true. Then, we can write
$S = \{x : P(x)\}$ to define the set $S$. Note that "for all $x$" implies that
$x$ is an element of the universe of discourse rather than some arbitrary set.

To define a set $S$ that contains all elements from some other set $A$ that
satisfy some predicate $P(x)$, we can write $S = \{x \in A : P(x)\}$.

## Set Operations
<div class="table-no-scrollbars"></div>

| Name | Symbol | Definition |
|:----:|:------:|:----------:|
| Union | $\cup$ | $A \cup B = \{x : (x \in A) \lor (x \in B)\}$ |
| Intersection | $\cap$ | $A \cap B = \{x : (x \in A) \land (x \in B)\}$ |
| Set Difference | $-$ | $A - B = \{x : (x \in A) \land \neg (x \in B)\}$ |
| Symmetric Difference | $\oplus$ | $A \oplus B = (x \in A) \oplus (x \in B)$ |
| Complement | $^c$ | \overline{A} = $A^c = \{x : x \notin A\}$ (w.r.t. universe $U$)|

## De Morgan's Laws
$$
\overline{A \cup B} = \overline{A} \cap \overline{B}
$$

$$
\overline{A \cap B} = \overline{A} \cup \overline{B}
$$
