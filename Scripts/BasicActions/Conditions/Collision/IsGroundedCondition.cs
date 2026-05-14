using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if the pawn is grounded.
/// </summary>
[GlobalClass, Icon("./IsGroundedCondition.svg")]
public sealed partial class IsGroundedCondition : Condition
{
    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return pawn.BelowAdjacent.IsGround;
    }
}