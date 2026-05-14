using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A base class for all pawn effects.
/// </summary>
[GlobalClass, Icon("./Effect.svg")]
public abstract partial class Effect : PawnBehavior
{
    /* Public methods. */
    /// <summary>
    /// Invoke this effect.
    /// </summary>
    public abstract void Invoke(double deltaTime);
}