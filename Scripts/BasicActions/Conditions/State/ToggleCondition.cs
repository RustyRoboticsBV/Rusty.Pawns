using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition whose state depends on a boolean.
/// </summary>
[GlobalClass, Icon("./ToggleCondition.svg")]
public sealed partial class ToggleCondition : Condition
{
    /* Public properties. */
    [Export] public bool State { get; set; }

    /* Protected methods. */
    protected override bool EvaluateMe(Pawn pawn)
    {
        return State;
    }
}