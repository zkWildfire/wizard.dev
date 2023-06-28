# Matrix Cache Simulator
## Overview
The Matrix Cache Simulator project is a simulation of how an in-place matrix
transpose algorithm uses the CPU. The simulator supports a variety of different
cache configurations and is designed to be used in a reinforcement learning
loop to train agents of various types.

Agents are scored by the number of cache hits they achieve while performing
the matrix transpose. Each cache miss incurs a small penalty, and agents are
heavily penalized if they overwrite a transposed value or write a value to the
wrong location. At each step in the simulation, the agent may choose to read a
value into a stack memory location or write a value from a stack memory location
to the matrix. Agents may use up to 8 registers as temporary storage and no
reward or penalty is given for register usage. Register usage does not impact
cache usage.

## High Level Architectural Overview
The simulation code is designed around a model-view-controller style
architecture. The **simulator** classes fulfill the model role and are
responsible for keeping track of the state of the simulator. As simulator state
changes, the simulator emits events, which are captured by an event hub class
that feeds events to the reward system and to the replay buffer. These classes
are referred to as the **observer** classes and are responsible for providing
feedback to the simulation algorithm and for recording the simulation so that
it can be replayed later. Algorithm playback is handled by the simulation code
on [whattf.how](https://www.whattf.how), which is written in TypeScript.

Lastly, the simulation code contains a set of interfaces that are implemented
by each agent that the simulation code supports. These agents range from
classical algorithms to reinforcement learning agents. Agents are handled by
the **training loop** classes, which are responsible for calling into the
agent code repeatedly until each simulation instance is complete or the agent
fails the simulation.

## Simulator Architecture
The simulator is implemented as a set of components that implement various
interfaces. This allows the main simulator class to be used for all cache
configurations.

```plantuml
title Simulator Interfaces

interface ISimulator {
    Interface that defines the API used by non-simulation components to \
interact with the simulator.
}

interface IPlacementPolicy {
    Component that determines where cache lines may be placed.
}

interface IMemory {
    Component that simulates the system's memory.
}

interface ICache {
    Component that manages the entire set of cache lines available to the simulation.
}

interface ICacheLine {
    Interface for classes that manage individual cache lines.
}

interface IValidator {
    Component that verifies whether a matrix was successfully transposed.
}

interface IEvictionPolicy {
    Component that determines which cache line should be evicted if all cache \
    lines that a new cache line maps to are in use.
}

ISimulator "1" *-- "1" IPlacementPolicy
ISimulator "1" *--- "1" IMemory
ISimulator "1" *--- "1" ICache
ICache "1" *-- "n" ICacheLine
ISimulator "1" *--- "1" IValidator
ISimulator "1" *-- IEvictionPolicy
```

Observer classes will never directly interact with the simulation object.
Instead, methods on the observer classes will be attached to events implemented
by the simulator interface. This ensures that the observer classes remain
entirely decoupled from the simulation code.

In order to facilitate both simulation playback as well as agent training,
the simulator will emit three types of events. The first type of event is
emitted when a memory location is accessed by the agent. The second and third
types of events are emitted when a cache hit or miss occurs, respectively. Cache
hits and misses are treated as separate events to make it easier for observer
classes to track the state of the cache.

## Observer Architecture
The observer classes consist of a main event hub that receives all events from
the simulator and a set of listeners that attach to event hub events. In the
code itself, the event hub will actually be the simulator object itself, but
as far as the observer classes are concerned, the event hub is its own interface
that is independent of the simulator code.

```plantuml
title Observer Interfaces

interface IEventHub {
    Defines the types of events that may be listened to by the observer classes.
}

interface IRewardPolicy {
    Component that determines the reward or penalty for a given event.
}

interface IReplayBuffer {
    Component that records the simulation so that it can be replayed later.
}

IEventHub "1" .. "1" IRewardPolicy
IEventHub "1" .. "1" IReplayBuffer
```

The reward policy classes are hidden behind an interface to ensure that
different policies can be easily swapped out for testing purposes. This makes
it extremely easy to test different reward policies against each other to
determine which policy is the most effective.

!!! note
    The replay buffer is also hidden behind an interface for ease of testing,
    but only one replay buffer implementation is expected to be needed.

## Training Loop
The code that handles the training loop is also responsible for setting up each
component used by the entire simulation. The code flow for the training loop
code is expected to look something like this:

```plantuml
title Training Loop

:Construct simulator components;
:Link simulator components;

repeat :Get action from agent;
    :Apply action;
    :Get reward from reward policy;
    :Notify agent of reward;
repeat while (Matrix is transposed?) is (false) not (true)

:Report results;
```

## Agents
This simulator will be used to test a variety of agents, ranging from purely
classical agents to reinforcement learning agents. Each agent implemented by
the simulator is listed below.

### Naive Transpose Algorithm
This is an adaption of the native matrix transpose algorithm for the simulator.
The algorithm does not attempt to benefit from the cache in any way and simply
scans each row of the matrix and transposes it.

### Cache Friendly Algorithm (Fixed)
This is the first of two classical cache friendly algorithms and is the same
as the cache friendly algorithm implemented on whattf.how's
[Matrix Cache Simulator Demo](https://whattf.how/posts/matrix-cache-simulator-demo/).
This algorithm is more cache friendly than the naive algorithm, but uses a fixed
set of parameters that do not change based on the cache configuration currently
in use.

### Cache Friendly Algorithm (Adaptive)
This version of the classical cache friendly algorithm makes use of the extra
feedback provided by this version of the simulator. The algorithm will use the
feedback from the simulator to adjust its internal parameters to better fit the
cache configuration currently in use. This algorithm is not available on the
whattf.how demo as it requires the extra feedback provided by this simulator.

### Componentized Classical Transpose Algorithm
This algorithm forms the baseline for some of the more advanced reinforcement
learning agents. It is a variant of the adaptive cache friendly algorithm that
places extra emphasis on using specific components to determine the best
action to take at each step in the simulation. This algorithm differs from the
adaptive cache friendly algorithm in that the adaptive cache friendly algorithm
will only adapt if it finds that its default parameters are causing extra cache
misses, whereas this algorithm will aggressively attempt to detect and adapt
to the cache configuration currently in use.

### Naive Neural Network
This agent is the first and most basic neural network agent. It will be used to
determine if a simple neural network composed of linear layers is capable of
learning to transpose a matrix. This agent will be used as a baseline for
comparing the performance of more advanced neural network agents.

### Cyclic Neural Network
An agent will also be created that uses recurrent or long short term memory
layers to learn to transpose a matrix. The purpose of this agent is to see if
doing nothing more than switching from the linear layers used by the naive
neural network to recurrent layers will improve the performance of the agent
at all. The rationale behind using recurrent layers is that the optimal action
to take at any given step in the simulation is dependent on the current cache
configuration, which is impossible to determine without knowing the results of
previous actions.

### Hybrid Componentized Network
This agent will act as a stepping stone to fully componentized and network
based agents. The design of this agent is based on the design of the classical
componentized agent, but substitutes some of the classically implemented
components with neural network layers. The purpose of this agent is to see if
the classical component agent's performance can be improved by using neural
networks in place of some of the classical code.

### Fully Componentized Network
This agent takes the design of the hybrid componentized network and extends it
to use neural networks for all components. The purpose of this agent is to
see if an agent can be composed out of multiple independent neural networks
that are then combined by using another network to decide which component to
use for each step. The goal of this design is to allow each individual network
component to be trained separately from the other components, which should
allow for faster training times.
