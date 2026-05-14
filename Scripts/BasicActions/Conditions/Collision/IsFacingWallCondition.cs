using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if the pawn is facing a wall.
/// </summary>
[GlobalClass, Icon("./IsFacingWallCondition.svg")]
public sealed partial class IsFacingWallCondition : Condition
{
    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return pawn.FrontAdjacent.IsWall;
    }
}