using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A grab/walk interaction.
/// </summary>
[GlobalClass]
public sealed partial class WalkGrabModifier : ModifierAction
{
    /* Public properties. */
    [Export] public WalkAction Walk { get; set; }
    [Export] public GrabAction Grab { get; set; }

    /* Public methods. */
    public override void AfterUpdateSpeed(double deltaTime, Pawn pawn)
    {
        if (Walk.CurrentSpeed < 0f && Grab.CurrentSpeed > 0f
            || Walk.CurrentSpeed > 0f && Grab.CurrentSpeed < 0f)
        {
            Walk.ForceStop();
        }

        else if (Walk.CurrentSpeed.Abs() > Grab.CurrentSpeed.Abs())
            Grab.ForceStop();
        else
            Walk.ForceStop();
    }
}
