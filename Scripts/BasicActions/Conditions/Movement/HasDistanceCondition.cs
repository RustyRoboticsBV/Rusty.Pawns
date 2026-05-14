using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if a movement action has a non-zero move distance.
/// </summary>
[GlobalClass, Icon("./HasDistanceCondition.svg")]
public sealed partial class HasDistanceCondition : Condition
{
    /* Public properties. */
    [Export] public MovementAction Movement { get; set; }

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return Movement.IsMoving();
    }
}