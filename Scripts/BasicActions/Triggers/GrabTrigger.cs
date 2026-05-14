using Godot;

using Rusty.Pawns;
using System.Reflection;

[GlobalClass]
public partial class GrabTrigger : CancelTrigger
{
    /* Public properties. */
    [Export] public float GrabDistance { get; set; } = 0.5f;

    /* Private properties. */
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
        }
        if (MustRelease)
        {
            CancelEffect.Invoke(deltaTime);
            MustRelease = false;
            MustGrab = false;
        }
    }
}