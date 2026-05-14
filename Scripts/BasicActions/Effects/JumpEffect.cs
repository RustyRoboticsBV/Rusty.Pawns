using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A trigger effect that starts a jump movement action.
/// </summary>
[GlobalClass]
public sealed partial class JumpEffect : Effect
{
    /* Public properties. */
    [Export] public JumpAction Action { get; set; }

    /* Public methods. */
    public override void Invoke(double deltaTime)
    {
        Action.Jump();
    }
}