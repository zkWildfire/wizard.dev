# Predicate Logic
## Predicates
A **predicate** is a statement that involves a variable and evaluates to true
or false. For example, $x > 5$ is a predicate because it involves the variable
$x$ and evaluates to true or false depending on the value of $x$. A predicate is
neither true nor false if their variables' values are not specified.

The statement $P(x)$ is also said to be the value of the propositional function
$P$ at $x$. Once a value has been assigned to $x$, the statement $P(x)$ becomes
a proposition and has a truth value.

## Quantifiers
### Universal Quantifier
The **universal quantifier** is denoted by $\forall$ and is read as "for all".
It is used to express that a predicate is true for all values of the variable,
e.g. "for all possible values of $x$, $P$ of $x$."

### Existential Quantifier
The **existential quantifier** is denoted by $\exists$ and is read as "there
exists". It is used to express that a predicate is true for at least one value
of the variable, e.g. "there exists a value of $x$ such that $P$ of $x$."

### Uniqueness Quantifier
The **uniqueness quantifier** is denoted by $\exists!$ or $\exists_1$ and is
read as "there exists a unique". It is used to express that a predicate is true
for exactly one value of the variable, e.g. "there exists a unique value of $x$
such that $P$ of $x$."

### Precedence of Quantifiers
Quantifiers have higher precedence than all logical operators. As a result,
the expression $\forall x P(x) \land Q(x)$ is equivalent to
$(\forall x P(x)) \land Q(x)$ rather than $\forall x (P(x) \land Q(x))$.

## Domain of Discourse
The **domain of discourse** is the set of values that the variable in a
predicate can take. For example, the domain of discourse for the predicate
$x > 5$ could be the set of all real numbers or the set of all integers. The
values in the domain of discourse are also referred to as elements.

Generally, an implicit assumption is made that all domains of discourse for
quantifiers are non-empty. However, note that if a domain of discourse is empty,
then $\exists x P(x)$ is false and $\forall x P(x)$ is true.

## Quantifier Truthiness
### Single Quantifier
| Quantifier | When True | When False |
|:----------:|:---------:|:----------:|
| $\forall x P(x)$ | $P(x)$ is true for all $x$ in the domain | $P(x)$ is false for at least one $x$ in the domain |
| $\exists x P(x)$ | $P(x)$ is true for at least one $x$ in the domain | $P(x)$ is false for all $x$ in the domain |

### Multiple Quantifiers
| Quantifiers | When True | When False |
|:-----------:|:---------:|:----------:|
| $\forall x \forall y P(x, y)$ | For every pair of $x, y$, $P(x, y)$ is true | At least one pair of $x, y$ exists such that $P(x, y)$ is false |
| $\forall x \exists y P(x, y)$ | For each value of $x$, we can find a specific $y$ for each $x$ where $P(x, y)$ is true | Some value $x$ exists that does not have a corresponding $y$ where $P(x, y)$ is true |
| $\exists x \forall y P(x, y)$ | We can find a specific $x$ for which $P(x, y)$ is true for all $y$ | For every value of $x$, there exists a $y$ for which $P(x, y)$ is false |
| $\exists x \exists y P(x, y)$ | At least one pair of $x, y$ exists such that $P(x, y)$ is true | For every pair of $x, y$, $P(x, y)$ is false |

## Logical Equivalences Using Quantifiers
Statements involving predicates and quantifiers are **logically equivalent**
if and only if they have the same truth values no matter which predicates are
substituted into those statements and no matter which domain of discourse is
used for the variables in those propositional functions.

## Negations of Quantifiers
**In every domain, exactly one of a statement and its negation should be true**.

For example, consider the predicate $\forall x P(x)$. The negation of this
predicate is **not** $\forall x \neg P(x)$, but rather $\exists x \neg P(x)$.
This is because the original predicate is false if a single value of $x$ makes
$P(x)$ false. The second predicate (the one that is not the negation of the
original predicate) would also be false if only a single value of $x$ makes
$P(x)$ false. Contrast that with the last predicate, which will be true any
time the original predicate is false.

## De Morgan's Laws for Quantifiers
| Law | Negation |
|:---:|:--------:|
| $\neg \forall x P(x)$ | $\exists x \neg P(x)$ |
| $\neg \exists x P(x)$ | $\forall x \neg P(x)$ |

## Binding Variables
A variable is **bound** when a quantifier is used on the variable. A variable
is said to be **free** if it is not bound by a quantifier or set equal to a
specific value. All variables in a propositional function must be bound or set
equal to a specific value to turn the function into a proposition.

The part of a logical expression to which a quantifier is applied is called the
**scope** of the quantifier. This is a nearly identical concept to the scope of
a variable in programming languages.

Variable names are also insignificant with respect to equivalency. For
example, $\forall x \exists y P(x, y) \equiv \forall a \exists b P(a, b)$.

## Scope of Quantifiers
Consider the expressions $\exists x (P(x) \land Q(x))$ and
$\exists x P(x) \land \exists x Q(x)$. The first expression states that there
exists some $x$ for which $P(x)$ and $Q(x)$ are both true. The second expression
on the other hand states that there exists some $x$ for which $P(x)$ is true
and there exists some $x$ for which $Q(x)$ is true. The second expression is
**not** identical to the first expression because it does not require that the
value of $x$ for which $P(x)$ is true is the same as the value of $x$ for which
$Q(x)$ is true.

!!! warning
    Using the same variable name like in the second example is highly
    discouraged because it can lead to confusion. The second example should
    have been written as $\exists x P(x) \land \exists y Q(y)$ to clarify
    that the two variables are not necessarily the same.

!!! info
    Just because two variables have different names does not mean that they have
    different values. For example, the inner part of the expression
    $\exists x \exists y (P(x) \land P(y))$ will be true any time $x = y$. $x$
    and $y$ are allowed to vary independently, but they are also allowed to have
    the same value simultaneously.

## Nested Quantifiers
Quantifiers can be nested to express more complex statements. A quantifier is
said to be **nested** if it sits within the scope of another quantifier.

## Order of Quantifiers
**Order is important** in expressions containing nested quantifiers. Quantifiers
may only be reordered if they are of the same type, e.g. both universal or both
existential. For example:
$$
\forall x \forall y P(x, y) \equiv \forall y \forall x P(x, y)
$$
$$
\exists x \exists y P(x, y) \equiv \exists y \exists x P(x, y)
$$

However, different types of quantifiers cannot be reordered:
$$
\forall x \exists y P(x, y) \not\equiv \exists y \forall x P(x, y)
$$

Also, quantifiers can *sometimes* be moved around without changing the meaning
of the expression. For example:
$$
\forall x(P(x) \land \exists y Q(x, y)) \equiv \forall x \exists y (P(x) \land Q(x, y))
$$
