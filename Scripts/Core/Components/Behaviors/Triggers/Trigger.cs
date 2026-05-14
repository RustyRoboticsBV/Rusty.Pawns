using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A base class for all pawn triggers.
/// </summary>
[GlobalClass, Icon("./Trigger.svg")]
public abstract partial class Trigger : PawnBehavior
{
    /* Public methods. */
    /// <summary>
    /// Called at the start of the update loop.
    /// </summary>
    public abstract void TryInvoke(double deltaTime);
}