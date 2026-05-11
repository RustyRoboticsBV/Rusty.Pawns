# Composable Character Controller

A modular 2.5D character controller framework for Godot 4, written in C#. It is designed for complex character movement, where many types of movement may be active at the same time, can be conditionable, and can influence each other.

Instead of a single monolithic controller, character movement is built using a *pawn* root node and several *component* child nodes, allowing complex character behavior to emerge from small, reusable components.

The module is made with the 3D physics system in mind, while handling purely 2D movement.

## Pawn

The `Pawn` class is the root node of the character controller. It:
- Collects and manages all `PawnComponent` children.
- Runs the movement loop, consisting of an property update, acceleration, speed, movement and face direction update.
- Performs surface detection and classification.
- Applies movement output from active actions.

## Components

All other behavior comes from `PawnComponent` nodes that are attached to a `Pawn`. There are several types:
- `Action`: A pawn component that maintains its own properties, acceleration, speed, movement and face direction. Each has a dedicated update method.
  - `Movement`: An action that stores a properties, acceleration, speed, movement and face direction update state.
    - `MovementX`: A horizontal movement action.
    - `MovementY`: A vertical movement action.
    - `Movement2D`: A 2D movement action, consisting of X and Y components to acceleration, speed, movement and face direction.
    - `MovementDirectional`: An angular movement action, consisting of a direction and one-dimensional acceleration, speed, movement and face direction.
  - `Modifier`: An action that does not store its own acceleration, speed, movement and face direction, but instead modifies the state of other actions. They can be thought of as modelling how movement actions interact with each other.
- `ActionProperties`: A node that exists to configure the behavior of the parent action.
- `Raycaster`: A pawn collision detector. Several can be active at the same time to create complex collision shapes.
- `Condition`: A pawn component that can be used to check if some condition holds true. They can be used to automatically enable/disable other components such as actions, properties and raycasters. They don't do anything on their own.

Components are updated by their order in the scene tree.

## Built-In Components

The module comes with several built-in components that cover common use-cases.

### Raycasters
- `PointRaycaster`: casts from a single point.
- `CircleRaycaster`: casts from the edge of a circle.
- `BoxRaycaster`: casts from the edges of a box.
- `CapsuleRaycaster`: casts from the edges of a capsule.

### Movement Actions
- `WalkAction`: A horizontal movement. Contains the following properties: `StartSpeed`, `TopSpeed` and `AccelerationTime`, `DecelerationTime`,  and `TurnTime`. Requires the `Walk` method to be called every loop to avoid deceleration.
- `JumpFallAction`: A vertical jumping and falling movement. Contains the following properties: `JumpHeight`, `JumpGravity`, `FallGravity`, `CancelGravity`, `MaxFallSpeed`. Jumps can be initiated using the `Jump` method.
- `JumpAction`: A variant movement that only handles jumping.
- `FallAction`: A variant movement that only handles falling.
- `DashAction`: A directional dash movement. Contains the following properties: `StartSpeed`, `TopSpeed`, `DelayTime`, `AccelerationTime`, `TopSpeedTime`, `DecelerationTime`.
- `ClimbAction`: A vertical version of the walk movement. Contains the same properties.
- `GrabAction`: A horizontal wall grab movement. Contains the following properties: `StartSpeed`, `TopSpeed`, `AccelerationTime`.
- `LedgeAction`: A 2D movement that models pulling a character onto a ledge. Contains the following properties: `JumpHeight`, `JumpGravity`, `FallGravity`, `StartXSpeed`, `TopXSpeed`, `AccelerationTime` and `DecelerationTime`.

### Modifier Actions
- `JumpFallModifier`: A modifier that stops all `FallAction` instances if there is at least one `JumpAction` in the middle of a jump.
- `DashWalkModifier`: A modifier that stops all `WalkAction` instances if there is at least one `DashAction` in the middle of a dash.
- `DashJumpModifier`: A modifier that stops all `JumpAction`, `FallAction` and `JumpFallAction` instances when a `DashAction` starts.

### Conditions
- `AndCondition`: Composes several conditions using a logical AND.
- `OrCondition`: Composes several conditions using a logical OR.
- `XorCondition`: Composes several conditions using a logical XOR.
- `NotCondition`: Negates another condition using a logical NOT.
- `IsGroundedCondition`: Checks if the surface below the pawn is adjacent and is either level, sloped or steep ground.
- `IsFacingWallCondition`: Checks if the surface in front of the pawn is adjacent is is either a level, downwards sloped or upwards sloped wall.
