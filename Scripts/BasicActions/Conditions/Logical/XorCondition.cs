using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if exactly one of several conditions is true.
/// </summary>
[GlobalClass, Icon("./XorCondition.svg")]
public sealed partial class XorCondition : Condition
{
    /* Public properties. */
    [Export] public Array<Condition> Condition { get; set; } = new();

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        bool foundTrue = false;

        foreach (Condition condition in Condition)
        {
            if (!condition.Evaluate(pawn))
                continue;

            if (foundTrue)
                return false;

            foundTrue = true;
        }

        return foundTrue;
    }
}