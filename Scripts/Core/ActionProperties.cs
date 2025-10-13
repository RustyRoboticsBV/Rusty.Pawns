using Godot;

namespace Rusty.Pawns;

[GlobalClass, Icon("./ActionProperties.svg")]
public abstract partial class ActionProperties : PawnComponent
{
    /// <summary>
    /// Called when this set of properties starts being an action's active set.
    /// </summary>
    public virtual void OnSelected(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Called when this set of properties stops being an action's active set.
    /// </summary>
    public virtual void OnDeselected(double deltaTime, Pawn pawn) { }
}