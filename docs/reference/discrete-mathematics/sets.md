# Sets
## Terminology
### Sets
Sets are unordered collection collections of objects called **elements**. To
indicate that an object exists in a set, the symbol $\in$ is used. For example,
if $x$ is an element of the set $A$, then $x \in A$. If an element $x$ is not
in a set $A$, then $x \notin A$.

### Subsets
A set $A$ is a subset of a set $B$ if and only if every element of $A$ is also
an element of $B$. This is denoted $A \subseteq B$. If $A$ is a subset of $B$
and $A \neq B$, then $A$ is a **proper subset** (also called a **strict
subset**) of $B$, denoted $A \subset B$.

### Cardinality
The cardinality of a set $A$, denoted $|A|$, is the number of elements in $A$.

### Disjoint Sets
Two sets $A$ and $B$ are disjoint if and only if $A \cap B = \emptyset$.

### Power Set
The power set of a set $A$, denoted $P(A)$, is the set of all subsets of $A$.

$$
P(A) = \{S \mid S \subseteq A\}
$$

### Cartesian Product
The Cartesian product of two sets $A$ and $B$, denoted $A \times B$, is the set
of all ordered pairs $(a, b)$ where $a \in A$ and $b \in B$.

### Characteristic Vectors
The characteristic vector of a set $A$ is a vector $v$ where $v_i = 1$ if
$i \in A$ and $v_i = 0$ if $i \notin A$. In other words, the characteristic
vector is a vector of bits whose length is equal to the size of the universe of
discourse and whose entries are 1 if the corresponding element is in the set
and 0 otherwise.

## Set Builder Notation
Set builder notation is a way to specify the elements that make up a set. The
syntax for set builder notation looks like the following:

$$
S = \{x \mid P(x)\}
$$

In set builder notation, the set being created is on the left side of the
equals sign. The variable $x$ is the variable that is being quantified over; in
other words, it determines the universe of discourse. The predicate $P(x)$
specifies the condition that elements must satisfy in order to be in the set.

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
$S = \{x \mid P(x)\}$ to define the set $S$. Note that "for all $x$" implies that
$x$ is an element of the universe of discourse rather than some arbitrary set.

To define a set $S$ that contains all elements from some other set $A$ that
satisfy some predicate $P(x)$, we can write $S = \{x \in A \mid P(x)\}$.

## Set Operations
### Basic Operations
<div class="table-no-scrollbars"></div>

| Name | Symbol | Definition |
|:----:|:------:|:----------:|
| Union | $\cup$ | $A \cup B = \{x \mid (x \in A) \lor (x \in B)\}$ |
| Intersection | $\cap$ | $A \cap B = \{x \mid (x \in A) \land (x \in B)\}$ |
| Set Difference | $-$ | $A - B = \{x \mid (x \in A) \land \neg (x \in B)\}$ |
| Symmetric Difference | $\oplus$ | $A \oplus B = (x \in A) \oplus (x \in B)$ |
| Complement | $^c$ | \overline{A} = $A^c = \{x \mid x \notin A\}$ (w.r.t. universe $U$)|

For equality, two sets are considered equal if and only if they have the same
elements. In other words, $A = B$ if and only if $A \subseteq B$ and
$B \subseteq A$. Order does not matter when comparing sets.

### Identity Laws
* $A \cup \emptyset = A$
* $A \cap U = A$

### Domination Laws
* $A \cup U = U$
* $A \cap \emptyset = \emptyset$

### Idempotent Laws
* $A \cup A = A$
* $A \cap A = A$

### Complementation Law
* $\overline{\overline{A}} = A$

### Commutative Laws
* $A \cup B = B \cup A$
* $A \cap B = B \cap A$

### Associative Laws
* $(A \cup B) \cup C = A \cup (B \cup C)$
* $(A \cap B) \cap C = A \cap (B \cap C)$

### Distributive Laws
* $A \cup (B \cap C) = (A \cup B) \cap (A \cup C)$
* $A \cap (B \cup C) = (A \cap B) \cup (A \cap C)$

### Absorption Laws
* $A \cup (A \cap B) = A$
* $A \cap (A \cup B) = A$

### Complement Laws
* $A \cup \overline{A} = U$
* $A \cap \overline{A} = \emptyset$

### De Morgan's Laws
* $\overline{A \cup B} = \overline{A} \cap \overline{B}$
* $\overline{A \cap B} = \overline{A} \cup \overline{B}$

## Russell's Paradox
Let $R$ be the set of all sets that do not contain themselves. Does $R$ contain
itself? If $R$ does not contain itself, then by definition, $R$ should contain
itself. If $R$ does contain itself, then by definition, $R$ should not contain
itself. This is a contradiction.

Russell's Paradox can be thought of as the set equivalent of the paradoxical
sentence "This sentence is false."

## Principle of Inclusion-Exclusion
Let $A$ and $B$ be finite sets. Then, $|A \cup B| = |A| + |B| - |A \cap B|$.

The value of $|A| + |B|$ will overcount the elements in $A \cap B$ by the
number of elements in $A \cap B$, so it's necessary to subtract $|A \cap B|$ to
get the correct value. The generalization of this principle to unions of
arbitrary sets is known as the **Principle of Inclusion-Exclusion**.

## Division Theorem
Let $a$ and $b$ be integers with $b > 0$. Then, there exist unique integers $q$
and $r$ such that $a = bq + r$ and $0 \leq r < b$.

## Using Set Notation With Quantifiers
The notation $\forall x \in A(P(x))$ is equivalent to
$\forall x(x \in A \rightarrow P(x))$. Similarly, the notation
$\exists x \in A(P(x))$ is equivalent to
$\exists x(x \in A \land P(x))$.
