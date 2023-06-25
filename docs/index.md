# Research Projects and Demos
Hello there! Welcome to `wizard.dev`, my portfolio of projects I've worked on.
Many of these projects were created with the goal of experimenting with and
learning more about machine learning and robotics, two fields that I'm deeply
interested in learning more about. You can find the source code for each of the
projects on this site's [GitHub repository](https://www.github.com/zkWildfire/wizard.dev)
and interactive demos of these projects on my blog at [whattf.how](https://www.whattf.how).
You can also learn more about who I am on the [About Me](./about-me.md) page.

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

## Game Development
### Unreal Engine 4 Tower Defense Game
Around 2017, I created a small tower defense game in Unreal Engine to learn
more about the engine. This game was written mostly using Blueprints rather
than C++ as it was more of a learning project than a game intended for release.
I developed this project to the point where it was playable and had all the
features I had in mind for the game, but I did not release this game anywhere.

### Singularity Game Engine
Over the years, I've experimented heavily with my own game engine designed to
make games more easily multithreaded. Originally envisioned as a full game
engine, I later repurposed the engine to act as a "co-engine" for an existing
game engine such as Unreal Engine so that I could focus on the multithreading
aspect of the design and not worry about implementing systems such as rendering
or animations myself.

I was able to build a small prototype in Unreal Engine which calculated actor
updates on background threads managed by Singularity before running the updates
on Unreal Engine's game thread, which proved that the design I created for
Singularity was functional. However, I never ended up building a game using
this design due to lack of art assets.

### Unreal Engine Testing Framework
While developing Singularity for Unreal Engine, I implemented my own version of
GoogleTest on top of the existing Unreal Engine automation testing framework.
I rely heavily on unit and integration tests to validate my code, and Unreal
Engine's automation testing framework was significantly clunkier at the time
than other testing frameworks such as GoogleTest. To fix this, I implemented
my own testing framework that mimicked the design of GoogleTest, but generated
tests that used Unreal Engine's automation testing framework under the hood.
My testing framework also implemented a clone of GoogleMock, which was
implemented entirely independently of GoogleMock but provided the same syntax
for declaring and using mocks.

This testing framework improved on Unreal Engine's automation testing framework
significantly by making it possible to define tests using syntax almost
identical to GoogleTest instead of the highly verbose test declarations that
the Unreal Automation Testing Framework required. Additionally, I used the Boost
C++ libraries' support for coroutines to allow unit tests to execute
single method which could execute over multiple engine frames, which was a
massive improvement over how Unreal Engine's Automation Testing Framework
handled latent actions at the time. This testing framework was written prior
to C++20, so using the coroutine support now built into C++ itself was not an
option.

!!! info
    While my testing framework was designed to match GoogleTest and GoogleMock
    as closely as possible, my testing framework was not created by taking the
    existing GoogleTest source code and making it compile for Unreal Engine.
    I wrote all of the code for the testing and mocking frameworks myself.
