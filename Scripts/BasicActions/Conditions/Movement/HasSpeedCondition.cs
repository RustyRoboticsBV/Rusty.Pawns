using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition that checks if a movement action has a non-zero speed.
/// </summary>
[GlobalClass, Icon("./HasSpeedCondition.svg")]
public sealed partial class HasSpeedCondition : Condition
{
    /* Public properties. */
    [Export] public MovementAction Movement { get; set; }

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return Movement.IsSpeeding();
    }
}