# Composable Character Controller

A modular, action-based 2D character controller framework for Godot 4, written in C#. It is designed around composable movement logic, deterministic update stages, and surface-aware movement handling.

Instead of a single monolithic controller, movement is built from independent *action*, *action properties*, *raycaster* and *condition* nodes that are managed and driven by a *pawn* root node, allowing complex character behavior to emerge from small, reusable components.

## Core Concepts

### Pawn

The `Pawn` class is the root node of the character controller. It:
- Collects and manages all `PawnComponent` children.
- Runs the movement loop, consisting of an property update, acceleration, speed, movement and face direction update.
- Performs surface detection and classification.
- Applies movement output from active actions.

### Components

All other behavior comes from `PawnComponent` nodes that are attached to a `Pawn`. There are several types:
- `Action`: A pawn component with an update loop for properties, acceleration, speed, movement and face direction.
  - `Movement`: An action that stores a properties, acceleration, speed, movement and face direction update state.
    - `MovementX`: A horizontal movement action.
    - `MovementY`: A vertical movement action.
    - `Movement2D`: A 2D movement action, consisting of X and Y components to acceleration, speed, movement and face direction.
    - `MovementDirectional`: An angular movement action, consisting of a direction and one-dimensional acceleration, speed, movement and face direction.
  - `Modifier`: An action that does not store its own acceleration, speed, movement and face direction, but instead modifies the state of other actions.
- `ActionProperties`: A node that exists to configure the behavior of the parent action.
- `Raycaster`: A pawn collision detector. Various shapes exist: `Point`, `Circle`, `Line`, `Box` and `Capsule`.
- `Condition`: A pawn component that can be used to check if some condition holds true. They can be used to automatically enable/disable other components. They don't do anything on their own.

### Built-In Components

The module comes with several built-in components that cover common use-cases.