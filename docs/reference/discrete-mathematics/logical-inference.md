# Logical Inference
## Terminology
### Argument 
A series of statements that end in a **conclusion**. An argument is considered
**valid** if the conclusion follows from the truth of the preceding statements,
referred to as **premises**.

### Axioms
Special inference rules that are assumed to be true.

### Proof
Valid arguments that establish the truth of mathematical statements.

### Theorem
A statement that has been proven to be true using a mathematical proof.

In mathematics, the name "theorem" is usually reserved for important statements.
Less important statements are sometimes called **propositions**. Theorems can
also be referred to as **facts** or **results**.

### Axioms
Axioms are statements that are assumed to be true. They are used as the starting
point for proofs. Axioms are also sometimes referred to as **postulates**.

### Lemma
A statement that is proven to be true in order to prove a larger statement.

### Corollary
A theorem that can be established directly from a theorem that has already been
proven.

### Conjecture
A statement that is believed to be true, but has not yet been proven. When a
conjecture is proven, it becomes a theorem.

## Inference Rules
### Syntax
Inference rules are written using a horizontal line with the premises above the
line and the conclusion below the line:

$$
\frac{P_1; P_2; \dots; P_n}{\therefore C_1, C_2, \dots, C_m}
$$
where $P_1, P_2, \dots, P_n$ are the premises and $C_1, C_2, \dots, C_m$ are
the conclusions. If all of the premises are true, then all of the conclusions
must also be true.

The statements above the line are also sometimes referred to as "requirements".
Also, note that the top typically uses semicolons to separate the premises,
while the bottom uses commas to separate the conclusions (though this is not
a strict requirement).

### Direct Proof Rule
The Direct Proof Rule is a special inference rule that allows statements of the
form $P \rightarrow Q$ to be proven. It is written as follows:

$$
\frac{p; p \implies q}{\therefore p \rightarrow q}
$$

The reason the top of the rule uses a double arrow instead of the single arrow
on the bottom is because that part of the rule is a bit more "abstract". What
the rule says is that if you can show that when $p$ is true, $q$ is also true,
then you can conclude that $p \rightarrow q$ is true.

This property holds because of the truth table of implications - if $p$ is
false, then $p \rightarrow q$ is true regardless of the truth value of $q$. If
$q$ is shown to be true whenever $p$ is true, then $p \rightarrow q$ is true
for all possible truth values of $p$ and $q$.

The Direct Proof Rule allows for sub-proofs to be used within a main proof. To
apply the rule, you first assume that $p$ is true, then show that $q$ follows
from that assumption. The steps used to prove $q$ from $p$ is called a
**sub-proof** and is typically indented by one level relative to the main proof.
Once the sub-proof is complete, you can conclude that $p \rightarrow q$ is true
and use it in the main proof.

!!! info
    If an exercise provides no facts and requires you to prove an implication,
    then there *must* be an application of the Direct Proof Rule in order to
    complete the proof.

### Propositional Logic Rules
#### Modus Ponens
> Also known as the **Law of Detachment**.

$$
\frac{p; p \rightarrow q}{\therefore q}
$$

#### Modus Tollens
$$
\frac{\neg q; p \rightarrow q}{\therefore \neg p}
$$

#### Hypothetical Syllogism
$$
\frac{p \rightarrow q; q \rightarrow r}{\therefore p \rightarrow r}
$$

#### Disjunctive Syllogism
$$
\frac{p \lor q; \neg p}{\therefore q}
$$

#### Resolution
$$
\frac{p \lor q; \neg p \lor r}{\therefore q \lor r}
$$

#### Intro/Elim Rules
| Operator | Intro Rule | Elim Rule |
|:--------:|:----------:|:---------:|
| $\land$ | $\frac{p; q}{\therefore p \land q}$ | $\frac{p \land q}{\therefore p}$;$\frac{p \land q}{\therefore q}$ |
| $\lor$ | $\frac{p}{\therefore p \lor q}$;$\frac{q}{\therefore p \lor q}$ | $\frac{p \lor q; \neg p}{\therefore q}$ |

Note that these rules are also known by these names:

| Operator | Intro Rule | Elim Rule |
|:--------:|:----------:|:---------:|
| $\land$ | Conjunction | Simplification |
| $\lor$ | Addition | Disjunctive Syllogism |

### Predicate Logic Rules
#### Intro/Elim Rules
<div class="table-no-scrollbars"></div>

| Operator | Intro Rule | Elim Rule |
|:--------:|:----------:|:---------:|
| $\forall$ | $\frac{\text{"Let a be arbitrary"} \dots P(a)}{\therefore \forall x P(x)}$ | $\frac{\forall x P(x)}{\therefore P(a) \text{ for any } a}$ |
| $\exists$ | $\frac{P(c) \text{ for some } c}{\therefore \exists x P(x)}$ | $\frac{\exists x P(x)}{\therefore P(c) \text{ for some special } c}$ |

For the Intro $\forall$ rule, $a$ must be an arbitrary value in the domain of
$P$ and no other value may depend on $a$ (see the [Dependencies](#dependencies)
section for more information on what it means for a variable to depend on
another variable).

For the Elim $\forall$ rule, the reason $c$ is referred to as "special" is
because the name represents a value for which $P(c)$ is true. No other
information is known about $c$ other than that it satisfies $P(c)$, and so
the variable name must be a new name that has not been used before. Any
dependencies of $c$ must also be listed.

Note that these rules are also known by these names:

| Operator | Intro Rule | Elim Rule |
|:--------:|:----------:|:---------:|
| $\forall$ | Universal Generalization | Universal Instantiation |
| $\exists$ | Existential Generalization | Existential Instantiation |

### De Morgan's Laws
$$
\begin{align}
\neg \forall x P(x) &\equiv \exists x \neg P(x) \\
\neg \exists x P(x) &\equiv \forall x \neg P(x)
\end{align}
$$

### Universal Modus Ponens
Modus Ponens is used with the Universal Instantiation so frequently that it
has its own name. It is written as follows:

$$
\frac{\forall x (P(x) \rightarrow Q(x)); P(a)}{\therefore Q(a)}
$$

Where $a$ is a particular element in the domain of $P$ and $Q$.

## Usage of Inference Rules
| Expression Type | Inference Rules | Equivalencies |
|:---------------:|:---------------:|:-------------:|
| Propositional Logic | Whole formulas only | Any sub-formula |
| Predicate Logic | Whole formulas only | Any sub-formula |

For predicate logic, the only equivalency listed on this page are De Morgan's
Laws.

## Dependencies
When using the Elim $\exists$ rule, the new variable being introduced may be
**dependent** on other variables. For example, consider this **incorrect**
proof:

!!! danger
    This proof is incorrect!

> Over the integer domain, show that $\forall x \exists y (y \ge x)$ is true
> but $\exists y \forall x (y \ge x)$ is false.

<!--
    Enable mathjax equation numbering by placing the LaTeX block in an HTML
    element. It's not entirely clear why equation numbering gets enabled by
    placing the LaTeX block in an HTML element, but it works.
-->
<div>

$$
\begin{align}
\forall x \exists y (y \ge x) && \text{Given} \\
\text{Let } a \text{ be an arbitrary integer} \\
\exists y (y \ge a) && \text{Elim } \forall \text{: 1} \\
b \ge a && \text{Elim } \exists: b \text{ special depends on } a\\
\forall x (b \ge x) && \text{Intro } \forall: 2,4 \\
\exists y \forall x (y \ge x) && \text{Intro } \exists: 5
\end{align}
$$

</div>

This proof is incorrect because of step 5. The variable $b$ depends on $a$,
so it's illegal to remove $a$ while $b$ still exists in the proof. Think of
this like calling `delete` on an object ($a$) in C++ while another object ($b$)
still has a raw pointer to the deleted object.

## Types of Proofs
### Proof by Contraposition
A Proof by Contraposition is a type of indirect proof that shows that a
proposition of the form $p \rightarrow q$ is true by showing that $\neg q
\rightarrow \neg p$ is true. This is done by assuming $\neg q$ and showing that
$\neg p$ follows from that assumption.

### Proof by Contradiction
A Proof by Contradiction is another type of indirect proof that shows that a
proposition of the form $p \rightarrow q$ is true by finding a contradiction
$q$ such that $\neg p \rightarrow q$ is true. To see why this works, consider
the following truth table:

| $p$ | $\neg p$ | $q$ | $\neg p \rightarrow q$ |
|:---:|:--------:|:---:|:----------------------:|
| T | F | F | T |
| F | T | F | F |

> Since $q$ is a contradiction, its value is always false.

If we know that $\neg p \rightarrow q$ is true, then $\neg p$ must be false. By
the law of the excluded middle, $p$ must be true.

### Proof By Cases
A Proof by Cases is a proof that is split into multiple cases. Instead of
proving a theorem as a sequence of cases, each case is proven separately.
This type of proof relies on the following tautology:

$$
((p_1 \lor p_2 \lor \dots \lor p_n) \rightarrow q) \leftrightarrow ((p_1 \rightarrow q) \land (p_2 \rightarrow q) \land \dots \land (p_n \rightarrow q))
$$

A Proof by Cases proof must cover all possible cases that may arise.

### Exhaustive Proof
Exhaustive proofs are a special case of Proof by Cases. An exhaustive proof
is a Proof by Cases proof where all possible cases are covered by value, e.g.
instead of proving a theorem for all even numbers and all odd numbers, the
theorem is proven for each integer individually.

### Existence Proof
An existence proof is a proof that shows that a particular object exists. In
other words, they are proofs of propositions of the form $\exists x P(x)$
where $P(x)$ is a predicate.

If the proof relies on finding an element $a$ such that $P(a)$ is true, then
the proof is called a **constructive existence proof**. If the proof relies
on showing that there exists an element $a$ such that $P(a)$ is true, but
never actually finds $a$, then the proof is called a **nonconstructive
existence proof**.

### Uniqueness Proof
A uniqueness proof is a proof that proves exactly one element exists with
some property. Uniqueness proofs are composed of two parts:

* Existence: Show that an element $x$ with the desired property exists.
* Uniqueness: Show that if $y \ne x$, then $y$ does not have the desired
  property.

Note that showing that there is a unique element $x$ such that $P(x)$ is true
is the same as proving this statement:

$$
\exists x (P(x) \land \forall y (y \ne x \rightarrow \neg P(y)))
$$

## Proof Strategies
### Forward and Backward Reasoning
**Forward reasoning** is the process of starting with the given facts and
applying inference rules to reach the desired conclusion. This is the most
common proof strategy.

**Backward reasoning** is the process of starting with the desired conclusion
and applying inference rules in reverse to reach the given facts. This is
useful since it may not be obvious how to reach the desired conclusion from
the given facts, but it may be obvious how to reach the given facts from the
desired conclusion.

Forward reasoning and backward reasoning are not mutually exclusive. It is
often useful to use both strategies simultaneously when constructing a proof
in order to determine how to reach the desired conclusion from the given facts.

### Adapting Existing Proofs
Sometimes, steps from existing proofs may also be applicable to a new proof.
Other times, seeing the approaches taken in existing proofs may provide
inspiration for how to approach a new proof.

### Looking For Counterexamples
Finding a counterexample can be a quick way to disprove a conjecture. However,
even if you can't find a counterexample, insights gained from looking for
counterexamples may help you find a proof for the conjecture.

## Fallacies
### Fallacy of Affirming the Conclusion
The fallacy of affirming the conclusion is a fallacy that occurs when a
premise is assumed to be true because the conclusion is true:

$$
\frac{p \rightarrow q; q}{\therefore p}
$$

This can be shown to be false by considering the case where $p$ is false and
$q$ is true.

### Fallacy of Denying the Hypothesis
The fallacy of denying the hypothesis is a fallacy that occurs when a
conclusion is assumed to be false because a premise is false:

$$
\frac{p \rightarrow q; \neg p}{\therefore \neg q}
$$

This can be shown to be false by considering the case where $p$ is false and
$q$ is true.
