using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A jump/fall interaction.
/// </summary>
[GlobalClass]
public sealed partial class JumpFallModifier : ModifierAction
{
    /* Public properties. */
    [Export] public JumpAction Jump { get; set; }
    [Export] public FallAction Fall { get; set; }

    /* Public methods. */
    public override void AfterUpdateSpeed(double deltaTime, Pawn pawn)
    {
        if (Jump.CurrentSpeed > 0f)
            Fall.ForceStop();
    }
}
