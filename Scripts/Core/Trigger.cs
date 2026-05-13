using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A base class for all pawn triggers.
/// </summary>
[GlobalClass, Icon("./Trigger.svg")]
public abstract partial class Trigger : Behavior
{
    /* Public methods. */
    /// <summary>
    /// Called at the start of the update loop.
    /// </summary>
    public virtual void TryTrigger(double deltaTime, Pawn pawn) { }

    /* Godot overrides. */
}