using Godot;

namespace Rusty.Pawns;

[GlobalClass]
public partial class GrabTrigger : CancelTrigger
{
    /* Public properties. */
    [Export] public float GrabDistance { get; set; } = 0.5f;

    /* Private properties. */
    private bool IsGrabbing { get; set; }
    private bool MustGrab { get; set; }
    private bool MustRelease { get; set; }

    /* Public methods. */
    /// <summary>
    /// Try to queue a jump.
    /// </summary>
    public void TryGrab()
    {
        if (Pawn.Front.Distance <= GrabDistance)
            MustGrab = true;
    }

    /// <summary>
    /// Try to queue a jump cancel.
    /// </summary>
    public void TryRelease()
    {
        MustRelease = true;
    }

    public override void TryInvoke(double deltaTime)
    {
        if (MustGrab)
        {
            InvokeEffect.Invoke(deltaTime);
            IsGrabbing = true;
        }

        if (IsGrabbing && Pawn.Front.Distance > GrabDistance)
            MustRelease = true;

        if (MustRelease)
        {
            CancelEffect.Invoke(deltaTime);
            MustRelease = false;
            IsGrabbing = false;
        }
    }
}