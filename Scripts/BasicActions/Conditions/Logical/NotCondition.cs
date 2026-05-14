using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that negates another condition.
/// </summary>
[GlobalClass, Icon("./NotCondition.svg")]
public sealed partial class NotCondition : Condition
{
    /* Public properties. */
    [Export] public Condition Condition { get; set; }

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return !Condition.Evaluate(pawn);
    }
}