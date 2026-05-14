# Composable Character Controller

A modular 2.5D character controller framework for Godot 4, written in C#. It's designed for complex character movement, where:
- Multiple movement types can be active simultaneously.
- Movement is condition-driven.
- Movement actions can influence each other.
- Players and NPCs share underlying behavior.

Instead of a single monolithic controller, characters are composed from a core *pawn* node, several reusable *component* nodes and a character-specific *driver* node.

The framework uses Godot's 3D physics system, while constraining movement to a 2D plane.

## Installation
1. Download the repository.
2. Extract the contents of the `Scripts` folder into your Godot project's resource folder.
3. Press the `Build Project` button.

A C# build of Godot is required.

## Architecture

Characters consist of three parts:
- Pawn: The core node that manages components and runs the movement pipeline.
- Components: Child nodes that implement behavior.
- Drivers: Character-specific nodes that control the pawn and its components, using player input or AI decisions.

Scene tree structure:
```
Driver
└ Pawn
  ├ Component 1
  ├ Component 2
  ├ ...
  └ Component N
```

Several categories of components exist:
- Actions:
  - Movements: Implement movement behavior such as walking, jumping, flying or dashing. Each movement maintains its own state, which the pawn comnbines into one movement vector each frame.
  - Modifiers: Alter the state of movement actions. These can affect individual movements or model interactions between multiple movements.
- Properties: Provide configurable values for actions, such as speed, acceleration or timing settings.
- Raycasters: Perform collision detection.
- Triggers: Detect events or state changes and fire effects in response.
- Effects: Execute behavior in response to triggers.
- Groups: Can be used to enable/disable collections of components.
- Conditions: Boolean checks that determine whether other components are active.

## Documentation
For a more detailed explanation of the framework, its concepts, and its classes - including getting started guides - visit the [documentation pages](https://github.com/RustyRoboticsBV/Rusty.Pawns/wiki).
