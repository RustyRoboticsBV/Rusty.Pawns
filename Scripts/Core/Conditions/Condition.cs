using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition, which can be used to dynamically enable/disable other pawn components.
/// </summary>
[GlobalClass, Icon("./Condition.svg")]
public abstract partial class Condition : PawnComponent
{
    /* Public methods. */
    /// <summary>
    /// Check if this condition holds true.
    /// </summary>
    public abstract bool Evaluate(Pawn pawn);
}