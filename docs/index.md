# Research Projects and Demos
Hello there and welcome to `wizard.dev`, my portfolio of projects I've worked on!
My name is Zach Wilson, and these are projects that I've built to experiment
with and learn more about machine learning and robotics, two fields that I'm
deeply interested in learning more about. You can find the source code for each
of the projects on this site's [GitHub repository](https://www.github.com/zkWildfire/wizard.dev)
and interactive demos of these projects on my blog at [whattf.how](https://www.whattf.how).

## Machine Learning
### Matrix Cache Simulator
The Matrix Cache Simulator is a small project that I originally wrote back in
2018 while taking classes at UW Seattle. The lab I was working on required me
to implement a matrix transpose algorithm that would read from one matrix and
write its transpose to another matrix. The original simulation was created to
help me visualize the how the cache was being used as my transpose algorithm
ran and allowed me to optimize the algorithm to reduce cache misses.

In 2023, I rewrote the simulation in TypeScript and made it available as an
[interactive demo](https://whattf.how/posts/matrix-cache-simulator-demo/) on my
blog. Unlike the original simulator, this simulator performs an in-place matrix
transpose, which allows the visualization to more easily fit on the blog layout
that I use.

After completing the demo for my blog, I decided to use the simulator as a
testbed for experimenting with reinforcement learning. At the time of working
on this project, the only experience I had with machine learning were the
classes I took at UW. This project afforded me the opportunity to acquire hands
on experience with a project of my own choosing rather than following a tutorial
that would constantly hold my hand.

## Tools
### [Kymira](https://www.kymira.dev)
Kymira is a build tool that I began developing after finding myself automating
various build tasks via Python scripts or shell scripts. This build tool differs
from other build tools in that it's designed to build projects written in
multiple languages and is designed to invoke existing build systems rather than
build code directly. It's especially useful for my C++ projects, where Kymira
allows me to bypass writing CMake build scripts entirely thanks to its ability
to autogenerate CMake build scripts.

### [PyShell](https://www.pyshell.dev)
PyShell is a small Python library that has since been eclipsed by Kymira. The
goal of this project was to make it easier to write shell scripts traditionally
written in Bash or Batch using Python. As work on Kymira progressed, Kymira
gradually grew to provide functionality similar to PyShell and inherited many
design elements from this project. As a result, most of my projects do not use
PyShell anymore since my projects almost always require Kymira for handling
builds, and Kymira is capable of handling tasks that would have otherwise been
written using PyShell.
