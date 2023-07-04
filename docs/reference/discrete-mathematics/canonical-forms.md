# Canonical Forms
## Terminology
### Literal
A **literal** is a Boolean variable or its complement.

### Minterm
A **minterm** of the Boolean variables $x_1, x_2, \dots, x_n$ is a Boolean
product $y_1y_2 \dots y_n$ where each $y_i$ is either $x_i$ or $\overline{x_i}$.
Hence, a minterm is a product of $n$ literals, with one literal for each
variable.

Since a minterm is a product, it will have the value 1 for one and only one
combination of values of its variables.

## Canonical Forms
!!! warning
    Canonical form is **not** the same as minimal form.

### Disjunctive Normal Form
Disjunctive normal form, otherwise known as "sum of products canonical form" or
**minterm expansion**, is a form of a boolean expression that consists of a sum
of products of literals. For example:

$$
F = A'B'C + A'BC + AB'C + ABC' + ABC
$$

### Conjunctive Normal Form
Conjunctive normal form, otherwise known as "product of sums canonical form" or
**maxterm expansion**, is a form of a boolean expression that consists of a
product of sums of literals. For example:
$$
F = (A + B + C)(A + B' + C)(A' + B + C)
$$

## Functional Completeness
Given some set of operators $F$, the set is said to be **functionally complete**
if every Boolean function can be expressed using only operators from $F$.

A functionally complete set can be further reduced if one of its operators can
be expressed as some combination of the other operators. For example, given the
set $\{ \cdot, +, ' \}$, the set can be reduced to $\{ \cdot, ' \}$. Similarly,
the set $\{ \cdot, +, ' \}$ can be reduced to $\{ +, ' \}$. However, the set
cannot be reduced to $\{ \cdot, + \}$ because the complement operator cannot be
expressed as a combination of the other two operators.

Note that a functionally complete set containing only one operator is also
possible. The NAND operator and the NOR operator both form functionally complete
sets with only one operator.
