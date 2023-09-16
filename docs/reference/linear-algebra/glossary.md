# Glossary
## Vector
A vector is any object that may be added together and multiplied by a scalar.
In general, for linear algebra, vectors will always be represented as a
$1 \times N$ or $N \times 1$ matrix. For example, a 2D vector $\vec{v}$ can be
represented as
$$
\begin{equation}
\vec{v} = \begin{bmatrix}
    v_1 \\
    v_2
    \end{bmatrix}
\end{equation}
$$

> Note: Vectors are typically written vertically, but KaTeX doesn't seem to
> want to render them this way.

Adding two vectors together will result in a vector with the same dimension as
the original vectors where each element is the sum of the corresponding
elements in the original vectors. For example, given the vectors $\vec{v}$ and
$\vec{w}$ defined as
$$
\begin{equation}
\vec{v} = \begin{bmatrix}
    v_1 \\
    v_2
    \end{bmatrix}
\end{equation}
$$
$$
\begin{equation}
\vec{w} = \begin{bmatrix}
    w_1 \\
    w_2
    \end{bmatrix}
\end{equation}
$$

A vector $\vec{u}$ defined as the sum of $\vec{v}$ and $\vec{w}$ is
$$
\begin{equation}
\vec{u} = \begin{bmatrix}
    v_1 + w_1 \\
    v_2 + w_2
    \end{bmatrix}
\end{equation}
$$

Multiplying a vector by a scalar will result in a vector with the same
dimension as the original vector where each element is the product of the
corresponding element in the original vector and the scalar. For example, given
the vector $\vec{v}$ defined as
$$
\begin{equation}
\vec{v} = \begin{bmatrix}
    v_1 \\
    v_2
    \end{bmatrix}
\end{equation}
$$

A vector $\vec{u}$ defined as the product of $\vec{v}$ and a scalar $s$ is
$$
\begin{equation}
\vec{u} = \begin{bmatrix}
    s v_1 \\
    s v_2
    \end{bmatrix}
\end{equation}
$$

## Linear Independence
A set of vectors is said to be linearly independent if no vector in the set can
be expressed as a linear combination of the other vectors in the set. For
example, the vectors $\vec{v}$ and $\vec{w}$ are linearly independent if there
does not exist any scalars $\alpha$ and $\beta$ such that
$$
\begin{equation}
\vec{v} = \alpha \vec{w} + \beta \vec{v}
\end{equation}
$$

## Linear Combination
A vector can be expressed as a linear combination of other vectors if it can be
written as a sum of scalar multiples of those vectors.

Any vector can be decomposed into a linear combination of the basis vectors of
the vector space it belongs to. The decomposition of a vector into a linear
combination of the basis vectors will always have the form
$$
\begin{equation}
\vec{v} = \sum_{i=1}^{n} \alpha_i \vec{b}_i
\end{equation}
$$

In this equation, $\vec{v}$ is the vector being decomposed. $\alpha_i$ will
always be a scalar, and $\vec{b}_i$ will always be a basis vector of the vector
space. The number of basis vectors in the sum is equal to the dimension of the
vector space.

For 2D space using the standard basis vectors $\hat{i}$ and $\hat{j}$, any
vector $\vec{v}$ can be written as
$$
\begin{equation}
\vec{v} = \alpha \hat{i} + \beta \hat{j}
\end{equation}
$$

As before, $\alpha$ and $\beta$ are scalars while $\hat{i}$ and $\hat{j}$ are
vectors.

Linear combinations are not restricted to the standard basis vectors. However,
if the vectors used are not linearly independent, then the linear combination
can be simplified.

## Vector Space
A vector space is a set of vectors that is closed under vector addition and
scalar multiplication. This means that if two vectors are in the set, then the
sum of the two vectors is also in the set. Similarly, if a vector is in the
set, then the product of the vector and any scalar is also in the set.

## Span
The span of a set of vectors is the set of all vectors that can be created by
linearly combining the vectors in the set. For a set of N linearly independent
vectors in an N-dimensional vector space, the span of the set is the entire
N-dimensional vector space. If the number of linearly independent vectors is
less than N, the span forms a proper subspace within the larger N-dimensional
vector space. In cases where two or more vectors in the set are linearly
dependent, the span will be an M-dimensional subspace, where M is the number of
linearly independent vectors in the set.

## Basis
The basis of a vector space is a set of vectors that are linearly independent
and span the vector space. Each vector in the set is called a basis vector and
each vector in the vector space can be expressed as a linear combination of the
basis vectors. The number of vectors in the basis is equal to the dimension of
the vector space.

Basis vectors for a given vector space are not unique. For example, the standard
basis vectors $\hat{i}$ and $\hat{j}$ are not the only basis vectors for 2D
space. Any two linearly independent vectors can be used as basis vectors for 2D
space.
