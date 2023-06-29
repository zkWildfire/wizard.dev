# Appendix 1: Proofs for Logical Equivalences
## Overview
This page contains proofs for the extra logical equivalences that are listed in
the Rosen textbook. The equivalences that these proofs are for are listed below
and on the [logical equivalences](logical-equivalences.md#additional-equivalences)
page.

## Equivalences Involving Conditional Statements
### $p \land q \equiv \neg (p \implies \neg q)$
$$
\begin{align}
p \land q &\equiv \neg \neg (p \land q) && \text{Double Negation} \\
&\equiv \neg (\neg p \lor \neg q) && \text{De Morgan's Laws} \\
&\equiv \neg (p \implies \neg q) && \text{Law of Implication}
\end{align}
$$

### $\neg (p \implies q) \equiv p \land \neg q$
$$
\begin{align}
\neg (p \implies q) &\equiv \neg (\neg p \lor q) && \text{Law of Implication} \\
&\equiv \neg \neg p \land \neg q && \text{De Morgan's Laws} \\
&\equiv p \land \neg q && \text{Double Negation}
\end{align}
$$

### $(p \implies q) \land (p \implies r) \equiv p \implies (q \land r)$
$$
\begin{align}
(p \implies q) \land (p \implies r) &\equiv (\neg p \lor q) \land (\neg p \lor r) && \text{Law of Implication} \\
&\equiv \neg p \lor (q \land r) && \text{Distributive} \\
&\equiv p \implies (q \land r) && \text{Law of Implication}
\end{align}
$$

### $(p \implies r) \land (q \implies r) \equiv (p \lor q) \implies r$
$$
\begin{align}
(p \implies r) \land (q \implies r) &\equiv (\neg p \lor r) \land (\neg q \lor r) && \text{Law of Implication} \\
&\equiv (r \lor \neg p) \land (r \lor \neg q) && \text{Commutative} \\
&\equiv r \lor (\neg p \land \neg q) && \text{Distributive} \\
&\equiv (\neg p \land \neg q) \lor r && \text{Commutative} \\
&\equiv \neg (p \lor q) \lor r && \text{De Morgan's Laws} \\
&\equiv (p \lor q) \implies r && \text{Law of Implication}
\end{align}
$$

### $(p \implies q) \lor (p \implies r) \equiv p \implies (q \lor r)$
$$
\begin{align}
(p \implies q) \lor (p \implies r) &\equiv (\neg p \lor q) \lor (\neg p \lor r) && \text{Law of Implication} \\
&\equiv \neg p \lor \neg p \lor q \lor r && \text{Commutative} \\
&\equiv \neg (p \land p) \lor q \lor r && \text{De Morgan's Laws} \\
&\equiv \neg p \lor q \lor r && \text{Idempotent} \\
&\equiv \neg p \lor (q \lor r) && \text{Associative} \\
&\equiv p \implies (q \lor r) && \text{Law of Implication}
\end{align}
$$

### $(p \implies r) \lor (q \implies r) \equiv (p \land q) \implies r$
$$
\begin{align}
(p \implies r) \lor (q \implies r) &\equiv (\neg p \lor r) \lor (\neg q \lor r) && \text{Law of Implication} \\
&\equiv \neg p \lor \neg q \lor r \lor r && \text{Commutative} \\
&\equiv \neg p \lor \neg q \lor r && \text{Idempotent} \\
&\equiv \neg (p \land q) \lor r && \text{De Morgan's Laws} \\
&\equiv (p \land q) \implies r && \text{Law of Implication}
\end{align}
$$

## Equivalences Involving Biconditional Statements
### $p \iff q \equiv \neg p \iff \neg q$
$$
\begin{align}
p \iff q &\equiv (p \implies q) \land (q \implies p) && \text{Biconditional} \\
&\equiv (\neg p \lor q) \land (\neg q \lor p) && \text{Law of Implication} \\
&\equiv (\neg q \lor p) \land (\neg p \lor q) && \text{Commutative} \\
&\equiv (p \lor \neg q) \land (q \lor \neg p) && \text{Commutative} \\
&\equiv (\neg p \implies \neg q) \land (\neg q \implies \neg p) && \text{Law of Implication} \\
&\equiv \neg p \iff \neg q && \text{Biconditional}
\end{align}
$$

### $p \iff q \equiv (p \land q) \lor (\neg p \land \neg q)$
$$
\begin{align}
p \iff q &\equiv (p \implies q) \land (q \implies p) && \text{Biconditional} \\
&\equiv (\neg p \lor q) \land (\neg q \lor p) && \text{Law of Implication} \\
&\equiv (\neg p \lor q) \land (p \lor \neg q) && \text{Commutative} \\
&\equiv (T \land (\neg p \lor q)) \land (T \land (p \lor \neg q)) && \text{Identity} \\
&\equiv ((p \land \neg p) \land (\neg p \lor q)) \land ((q \land \neg q) \land (p \lor \neg q)) && \text{Negation} \\
&\equiv ((\neg p \land p) \land (\neg p \lor q)) \land ((\neg q \land q) \land (\neg q \lor p)) && \text{Commutative} \\
&\equiv (\neg p \lor (p \land q)) \land (\neg q \lor (q \land p)) && \text{Distributive} \\
&\equiv (\neg p \lor (p \land q)) \land (\neg q \lor (p \land q)) && \text{Commutative} \\
&\equiv ((p \land q) \lor \neg p) \land ((p \land q) \lor \neg q) && \text{Commutative} \\
&\equiv (p \land q) \lor (\neg p \land \neg q) && \text{Distributive}
\end{align}
$$

### $\neg (p \iff q) \equiv p \iff \neg q$
$$
\begin{align}
\neg (p \iff q) &\equiv \neg ((p \land q) \lor (\neg p \land \neg q)) && \text{Previous Proof} \\
&\equiv \neg (p \land q) \land \neg (\neg p \land \neg q) && \text{De Morgan's Laws} \\
&\equiv (\neg p \lor \neg q) \land (\neg \neg p \lor \neg \neg q) && \text{De Morgan's Laws} \\
&\equiv (\neg p \lor \neg q) \land (p \lor q) && \text{Double Negation} \\
&\equiv (p \lor \neg q) \land (\neg p \lor q) && \text{Commutative} \\
&\equiv (p \implies \neg q) \land (\neg p \implies q) && \text{Law of Implication} \\
&\equiv p \iff \neg q && \text{Biconditional}
\end{align}
$$
