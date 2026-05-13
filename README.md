# Composable Character Controller

A modular 2.5D character controller framework for Godot 4, written in C#. It's intended for complex character movement, where:
- Multiple movement types may be active at the same time.
- Movement is condition-driven.
- Movement actions can influence each other.
- Player(s) and NPCs share underlying behavior.

Instead of a single monolithic controller, characters are built from a core *pawn* node, several reusable *component* nodes and a character-specific *driver* node.

The framework uses Godot's 3D physics system. Movement is constrained to a 2D plane.

## Pawn

The `Pawn` class is the central node of the controller. It is responsible for:
- Collecting and managing all attached `PawnComponent` children.
- Running the movement pipeline.
- Detecting and classifying surfaces.

Attached components can be retrieved by type and/or by name with the `GetComponent` method.

### Surface Types
The `Pawn` collects surface information in all four directions. This includes distance, surface normal, angle and semantic classification.

Ten types of surfaces are recognized:
- `Air`
- `Ground`:
  - `Level ground`
  - `Sloped ground`: can be climbed and descended.
  - `Steep ground`: can be descended but not climbed.
- `Wall`:
  - `Straight wall`
  - `Downwards sloped wall`: redirects downwards movement.
  - `Upwards sloped wall`: redirects upwards movement.
- `Ceiling`:
  - `Level ceiling`
  - `Sloped ceiling`: can be climbed and descended.
  - `Steep ceiling`: can be descended but not climbed.

## Components

The pawn doesn't do much on its own. All functionality is implemented by attaching `PawnComponent` nodes. There are six categories: raycasters, actions, properties, triggers, effects and conditions. The actions are split into two sub-groups: movement and modifiers.

All components are processed in several phases, by scene tree order in each phase. The phases are: `EvaluateConditions`, `InvokeTriggers`, `UpdateMovementProperties`, `ModifierPostUpdateProperties`, `UpdateMovementAcceleration`, `ModifierPostUpdateAcceleration`, `UpdateMovementSpeed`, `ModifierPostUpdateSpeed`, `UpdateMovementDistance`, `ModifierPostUpdateDistance`, `UpdateMovementFaceDirection`, `ModifierPostUpdateFaceDirection` and `CommitMovement`.

### Movement Actions
A `Movement` action is a `PawnComponent` that manages its own state, including properties, acceleration, speed, displacement and face direction. Each state has a dedicated update method.

Four subtypes exist:
- `MovementX`: A horizontal-only movement action.
- `MovementY`: A vertical-only movement action.
- `Movement2D`: A 2D movement action, consisting of separate X and Y acceleration, speed, etc.
- `MovementDirectional`: An angular movement action, consisting of a direction vector and one-dimensional acceleration, speed, etc.

Examples of built-in movement actions include `WalkAction`, `JumpFallAction`, `FlyAction` and `DashAction`.

### Modifier Actions
A `Modifier` action does not maintain its own movement state. Instead, it modifies the state of other actions. This allows interactions between movement actions to be modeled cleanly.

An example of a built-in modifier actions is `WalkDashModifier`, which checks the combined horizontal displacement of all walk and dash actions and keeps only the largest of the two.

### Action Properties
An `ActionProperties` component is used to configure the parent action. Each contains a set of properties, which often includes things like time, speed, acceleration or speed limit values. Each update, the parent action selects the first active properties component, which will be used for the rest of that loop.

### Raycasters
A `Raycaster` component is a collision detector. Multiple raycasters can be combined to create complex collision shapes.

Five types exist: `PointCaster`, `LineCaster`, `CircleCaster`, `BoxCaster` and `CapsuleCaster`.

### Triggers and Effects
A `Trigger` component represents an event or state transition. They may have one or more `Effect` child nodes; whenever the trigger's criteria are met, all child effects are executed.

Effects interact with another component, such as enabling/disabling a component or starting an action.

An example of a built-in triggers are `KeyEvent` and `LandedEvent`. An example of a built-in effect is `JumpEffect`, which calls the `Jump` method of a `JumpAction` or `JumpFallAction`.

### Conditions
A `Condition` component is used to evaluate whether a specific condition is true. By itself, it does not perform any behavior.

Actions, properties, raycasters, triggers and effects all contain a `Conditions` field that determines when the component is active. A component is only active when all assigned conditions evaluate to true.

The `Pawn` automatically enables and disables components based on their conditions.

Examples of built-in conditions include `IsGroundedCondition`, `IsFacingWallCondition` and `IsMovingCondition`.

Conditions can be combined using various logical operator conditions:
- `AndCondition`: is true if all referenced conditions are true.
- `OrCondition`: is true if at least one referenced condition is true.
- `XorCondition`: is true if exactly one referenced condition is true.
- `NotCondition`: is true if the referenced condition is NOT true.

A disabled component has the following effect:
- Movement: no updates occur, and the displacement does not affect the pawn's position.
- Modifier: no updates occur, and no actions are modified.
- ActionProperties: are never used by their parent action.
- Raycaster: the raycaster does not do any collision checking.
- Trigger: the trigger never fires.
- Effect: the effect is not invoked if its parent trigger fires.

## Drivers
`Driver` classes are responsible for calling certain action methods at appropriate times (i.e. in response to player inputs or enemy AI logic). A `Driver` can only control one `Pawn`.

For instance, a `JumpAction` only iniates a jump when its `Jump` method is called, which can be done as follows:
```
public partial class MyDriver : Driver
{
	public override _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Space))
			Pawn.GetComponent<JumpAction>.Jump();
	}
}
```
