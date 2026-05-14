using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A manually-activated trigger.
/// </summary>
[GlobalClass]
public sealed partial class ManualTrigger : Trigger
{
    /* Private properties. */
    private bool MustInvoke { get; set; }

    /* Public methods. */
    /// <summary>
    /// Invoke this trigger's effects.
    /// </summary>
    public void Activate()
    {
        MustInvoke = true;
    }

    /* Protected methods. */
    /// <summary>
    /// Check if this trigger's effects must be invoked.
    /// </summary>
    protected override bool Check(double deltaTime)
    {
        if (MustInvoke)
        {
            MustInvoke = false;
            return true;
        }
        return false;
    }
}