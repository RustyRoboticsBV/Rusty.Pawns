using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A trigger effect that cancels a jump movement action.
/// </summary>
[GlobalClass]
public sealed partial class CancelJumpEffect : Effect
{
    /* Public properties. */
    [Export] public JumpAction Action { get; set; }

    /* Public methods. */
    public override void Invoke(double deltaTime)
    {
        Action.CancelJump();
    }
}