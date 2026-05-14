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

    /* Public methods. */
    public override bool Evaluate(Pawn pawn)
    {
        return !Condition.Evaluate(pawn);
    }
}