using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A jump effect.
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