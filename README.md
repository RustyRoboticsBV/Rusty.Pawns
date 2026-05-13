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
