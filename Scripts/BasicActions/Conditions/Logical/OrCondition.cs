using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if one of several conditions is true.
/// </summary>
[GlobalClass, Icon("./OrCondition.svg")]
public sealed partial class OrCondition : Condition
{
    /* Public properties. */
    [Export] public Array<Condition> Condition { get; set; } = new();

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        foreach (Condition condition in Condition)
        {
            if (condition.Evaluate(pawn))
                return true;
        }
        return false;
    }
}