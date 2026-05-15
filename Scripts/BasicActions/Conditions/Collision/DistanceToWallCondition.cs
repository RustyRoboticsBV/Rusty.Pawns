using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if the pawn is some distance from a wall.
/// </summary>
[GlobalClass, Icon("./DistanceToWallCondition.svg")]
public sealed partial class DistanceToWallCondition : Condition
{
    /* Public types. */
    public enum DistanceOperator { CloserThan, FurtherThan };

    /* Public properties. */
    [Export] public float Distance { get; set; } = 1f;
    [Export] public DistanceOperator Operator { get; set; } = DistanceOperator.CloserThan;

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        if (Operator == DistanceOperator.CloserThan)
            return pawn.Front.Distance < Distance;
        if (Operator == DistanceOperator.FurtherThan)
            return pawn.Front.Distance > Distance;
        return false;
    }
}