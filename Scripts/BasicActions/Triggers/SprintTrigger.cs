using Godot;

namespace Rusty.Pawns;

[GlobalClass]
public partial class SprintTrigger : CancelTrigger
{
    /* Private properties. */
    private bool Sprinting { get; set; }
    private bool MustSprint { get; set; }
    private bool MustStop { get; set; }

    /* Public methods. */
    /// <summary>
    /// Try to queue a jump.
    /// </summary>
    public void TryStart()
    {
        Sprinting = true;
        MustSprint = true;
    }

    /// <summary>
    /// Try to queue a jump cancel.
    /// </summary>
    public void TryStop()
    {
        Sprinting = true;
        MustStop = true;
    }

    public override void TryInvoke(double deltaTime)
    {
        if (MustSprint)
        {
            InvokeEffect.Invoke(deltaTime);
            MustSprint = false;
        }
        if (MustStop)
        {
            CancelEffect.Invoke(deltaTime);
            MustStop = false;
        }
    }
}