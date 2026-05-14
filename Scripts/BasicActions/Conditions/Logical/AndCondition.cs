using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if multiple conditions are true at the same time.
/// </summary>
[GlobalClass, Icon("./AndCondition.svg")]
public sealed partial class AndCondition : Condition
{
    /* Public properties. */
    [Export] public Array<Condition> Condition { get; set; } = new();

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        foreach (Condition condition in Condition)
        {
            if (!condition.Evaluate(pawn))
                return false;
        }
        return true;
    }
}