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

## Linear Transformation
A linear transformation is a function that maps vectors from one vector space to
another vector space while preserving the vector addition and scalar
multiplication properties of the vector space. A transformation must guarantee
that all lines in the original vector space map to lines in the new vector
space and that the origin of the original vector space maps to the origin of the
new vector space to be considered a linear transformation.

Linear transformations may map vectors from a vector space to the same vector
space or to a subspace of the original vector space. For example, a linear
transformation may rotate all vectors in a vector space by 90 degrees. This
transformation maps vectors from the original vector space to the same vector
space. However, a linear transformation may also project all vectors in a vector
space onto a line. This transformation maps vectors from the original vector
space to a subspace of the original vector space.

A linear transformation may be expressed as a matrix, where the columns of the
matrix record where the basis vectors of the original vector space are mapped
to. For example, a linear transformation that rotates all vectors in 2D space by
90 degrees can be expressed as

$$
\begin{equation}
\begin{bmatrix}
    0 & -1 \\
    1 & 0
\end{bmatrix}
\end{equation}
$$

In the resulting vector space after the transformation, the basis vectors are
mapped to

$$
\begin{equation}
\begin{bmatrix}
    0 \\
    1
\end{bmatrix}
\end{equation}
$$

and

$$
\begin{equation}
\begin{bmatrix}
    -1 \\
    0
\end{bmatrix}
\end{equation}
$$

This property means that if you take any vector in the original vector space
and express it as a linear combination of the basis vectors of the original
vector space, then the resulting vector in the new vector space will be the
same linear combination of the basis vectors of the new vector space. In other
words, given some vector defined as

$$
\vec{v} = \alpha \hat{i} + \beta \hat{j}
$$

The resulting vector after the transformation will be

$$
\vec{v}' = \alpha \begin{bmatrix}
    0 \\
    1
\end{bmatrix} + \beta \begin{bmatrix}
    -1 \\
    0
\end{bmatrix}
$$

## Matrix Multiplication
Matrix multiplication allows a linear transformation to be applied to a vector,
or to describe a new linear transformation that is the composition of two
linear transformations. Matrix multiplication is not commutative, meaning that
the order of the matrices in the multiplication matters. For example, given two
matrices $A$ and $B$, $A B \neq B A$. However, matrix multiplication is
associative, meaning that $(A B) C = A (B C)$.

To understand why matrix multiplication is not commutative, consider each matrix
as a function rather than a variable. This reasoning is valid because each
matrix represents a linear transformation. Just as you would not expect
$f(g(x))$ to be equal to $g(f(x))$ for arbitrary functions $f$ and $g$, you
should not expect $A B$ to be equal to $B A$ for arbitrary matrices $A$ and $B$.

When multiplying a matrix and a vector, I like to mentally visualize the
operation as taking the vector, rotating it 90 degrees counter-clockwise, and
then sliding it down the matrix. So for example, given the following matrix
and vector:

$$
\begin{equation}
\begin{bmatrix}
    a & b \\
    c & d
\end{bmatrix}
\begin{bmatrix}
    e \\
    f
\end{bmatrix}
\end{equation}
$$

I first start by mentally "tipping over" the vector to rotate it 90 degrees
counter-clockwise, giving me the following vector:

$$
\begin{equation}
\begin{bmatrix}
    e &
    f
\end{bmatrix}
\end{equation}
$$

I then take the vector and slide it down the matrix. So when calculating the
first element of the resulting vector, I'm mentally thinking of the matrix like
this:

$$
\begin{equation}
\begin{bmatrix}
    a * e & b * f \\
    c & d
\end{bmatrix}
\end{equation}
$$

To get the final value for the first element of the resulting vector, I add the
two elements in the first row of the matrix together. So the first element of
the resulting vector is $a e + b f$. I then repeat this process for the second
element of the resulting vector, which can be visualized as sliding the vector
down the matrix like this:

$$
\begin{equation}
\begin{bmatrix}
    a & b \\
    c * e & d * f
\end{bmatrix}
\end{equation}
$$

So the second element of the resulting vector is $c e + d f$. Putting this
together, the resulting vector has the form typically given for matrix
multiplication:

$$
\begin{equation}
\begin{bmatrix}
    a e + b f \\
    c e + d f
\end{bmatrix}
\end{equation}
$$

When multiplying two matrices together, I follow the same mental process by
treating the second matrix as a sequence of vectors and processing each vector
individually. Calculating the result for the first vector from the second
matrix gives me the first column of the resulting matrix. I then repeat this
process for each vector in the second matrix to get the remaining columns of
the resulting matrix.

Mentally visualizing matrix multiplication as sliding vectors down the first
matrix also helps demonstrate why requirements exist on the dimensions of the
matrices being multiplied. The second matrix must always have the same number
of vertical elements (rows) as the first matrix has horizontal elements
(columns). If this requirement is not met, then it won't be possible to "slide"
the vectors down the first matrix since they'll be too long or too short.
