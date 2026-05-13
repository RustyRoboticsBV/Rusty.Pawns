# Composable Character Controller

A modular 2.5D character controller framework for Godot 4, written in C#. It's intended for complex character movement, where:
- Multiple movement types may be active at the same time.
- Movement is condition-driven.
- Movement actions can influence each other.
- Player(s) and NPCs share underlying behavior.

Instead of a single monolithic controller, characters are built from a core *pawn* node, several reusable *component* nodes and a character-specific *driver* node.

The framework uses Godot's 3D physics system. Movement is constrained to a 2D plane.

## Architecture

Characters are built from three parts:
- Pawn: The core node that manages components and runs the movement pipeline.
- Components: Child nodes that implement behavior.
- Drivers: Character-specific controllers that translate player input or AI decisions into movement/action requests for the pawn's components.

The scene tree structure is:
```
Driver
└ Pawn
  ├ Component 1
  ├ Component 2
  ├ ...
  └ Component N
```

Several types of components exist:
- Actions:
  - Movements: Implement movement behavior such as walking, jumping, flying or dashing. Each action manages its own movement state.
  - Modifiers: Modify the state of movement actions, modelling interactions between different movement actions.
- Action Properties: Provide configurable values for actions, such as speed, acceleration or timing settings.
- Raycasters: Perform collision and surface detection.
- Triggers: Detect events or state changes and fire effects in response.
- Effects: Execute behavior in response to triggers.
- Conditions: Boolean checks that determine whether other components are active.

## Documentation
For a more in-depth description of concepts and classes, as well as how to get started, visit the [documentation pages](https://github.com/RustyRoboticsBV/Rusty.Pawns/wiki).
