using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if the pawn is in the air.
/// </summary>
[GlobalClass, Icon("./IsAerialCondition.svg")]
public sealed partial class IsAerialCondition : Condition
{
    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return !pawn.BelowAdjacent.IsGround;
    }
}