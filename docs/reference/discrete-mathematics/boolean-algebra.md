# Boolean Algebra
## Terminology
### Combinational Logic
Logic where the output is only dependent on the current input. This type of
logic is also known as "time-independent logic".

$$
\text{output} = F(\text{input})
$$

### Sequential Logic
Logic where the output is dependent on the current input and the previous
inputs. As a result, sequential logic has "state" (memory) whereas combinational
logic does not.

$$
\text{output} = F(\text{output}_{t-1}, \text{input}_t)
$$

### Boolean Algebra
A branch of algebra consisting of:

* A set of elements $B = \{0, 1\}$
* Binary operations $\{ +, \cdot \}$ (OR, AND)
* An unary operation $\{'\}$ (NOT)
* A set of axioms ([see below](#axioms))

<!--
    Force a line break before "complement". Otherwise, the line break gets added
    between the apostrophe and the closing parenthesis.
 -->
The three primary operations are also referred to as "sum" ($+$), "product"
($\cdot$), and<br>"complement" ($'$).

### Boolean Function
A function that maps a set of binary values to a single binary value.

Formally, let $B = \{0, 1\}$. Then, $B^n = \{ (x_1, x_2, \dots, x_n) \mid x_i \in B for 1 \leq i \leq n \}$.
A function from $B^n$ to $B$ is called a **boolean function of degree $n$**.

### Boolean Expression
An expression that consists of boolean variables, constants, and operators.

Formally, $0, 1, x_1, x_2, \dots, x_n$ are boolean expressions. If $E_1$ and $E_2$
are boolean expressions, then $E_1 + E_2$, $E_1 \cdot E_2$, and $E_1'$ are also
boolean expressions.

### Duality
The **dual** of a boolean expression is obtained by interchanging Boolean sums
and products, and interchanging $0$s and $1$s.

For example:

* The dual of $x(y + 0)$ is $x + (y \cdot 1)$
* The dual of $\overline{x} \cdot 1 + (\overline{y} + z)$ is $(\overline{x} + 0)(\overline{y} \cdot z)$

The dual of a Boolean function $F$ represented by a Boolean expression is the
function represented by the dual of the expression. This other function is
typically denoted as $F^d$ and does not depend on the particular Boolean
expression used to represent $F$.

### Duality Principle
An identity between functions represented by Boolean expressions remains valid
when the duals of both sides of the identity is taken. This result is called
the **Duality Principle**.

## Boolean Order of Operations
1. Complement ($'$)
2. Product ($\cdot$)
3. Sum ($+$)

This list ignores parentheses, which are used to group operations.

## Axioms
### Closure
For all $x, y \in B$:

* $x + y \in B$
* $x \cdot y \in B$

### Commutativity
> These are also referred to as "Commutative Laws".

For all $x, y \in B$:

* $x + y = y + x$
* $x \cdot y = y \cdot x$

### Associativity
> These are also referred to as "Associative Laws".

For all $x, y, z \in B$:

* $(x + y) + z = x + (y + z)$
* $(x \cdot y) \cdot z = x \cdot (y \cdot z)$

### Distributivity
> These are also referred to as "Distributive Laws".

For all $x, y, z \in B$:

* $x + (y \cdot z) = (x + y) \cdot (x + z)$
* $x \cdot (y + z) = (x \cdot y) + (x \cdot z)$

### Identity
> These are also referred to as "Identity Laws".

For all $x \in B$:

* $x + 0 = x$
* $x \cdot 1 = x$

### Complementarity
For all $x \in B$:

* $x + x' = 1$ (also referred to as the "Unit Property")
* $x \cdot x' = 0$ (also referred to as the "Zero Property")

### Null
> These are also referred to as "Domination Laws".

For all $x \in B$:

* $x + 1 = 1$
* $x \cdot 0 = 0$

### Idempotency
> These are also referred to as "Idempotent Laws".

For all $x \in B$:

* $x + x = x$
* $x \cdot x = x$

### Involution
> This is also known as the "Law of the Double Complement".

For all $x \in B$:

* $(x')' = x$

## Simplification Rules
### Uniting
For all $x, y \in B$:

* $x \cdot y + x \cdot y' = x$
* $(x + y) \cdot (x + y') = x$

### Absorption
> These are also referred to as "Absorption Laws".

For all $x, y \in B$:

* $x + x \cdot y = x$
* $x \cdot (x + y) = x$
* $(x + y') \cdot y = x \cdot y$
* $(x \cdot y') + y = x + y$

### Factoring
For all $x, y, z \in B$:

* $(x + y) \cdot (x' + z) = x \cdot z + x' \cdot y$
* $(x \cdot y) + (x' \cdot z) = (x + z) \cdot (x' + y)$

### Consensus
For all $x \in B$:

* $(x \cdot y) + (y \cdot z) + (x' \cdot z) = (x \cdot y) + (x' \cdot z)$
* $(x + y) \cdot (y + z) \cdot (x' + z) = (x + y) \cdot (x' + z)$

### De Morgan's Laws
For all $x, y \in B$:

* $(x + y + \dots)' = x' \cdot y' \cdot \dots$
* $(x \cdot y \cdot \dots)' = x' + y' + \dots$
