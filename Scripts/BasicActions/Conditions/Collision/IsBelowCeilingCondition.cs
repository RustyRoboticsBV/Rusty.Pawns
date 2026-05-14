using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if the pawn is below a ceiling.
/// </summary>
[GlobalClass, Icon("./IsBelowCeilingCondition.svg")]
public sealed partial class IsBelowCeilingCondition : Condition
{
    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return pawn.AboveAdjacent.IsCeiling;
    }
}