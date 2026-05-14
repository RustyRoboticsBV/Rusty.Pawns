using Godot;

namespace Rusty.Pawns;

/// <summary>
/// An action that alters the behavior of other actions.
/// </summary>
[GlobalClass, Icon("./ModifierAction.svg")]
public abstract partial class ModifierAction : Action
{
}