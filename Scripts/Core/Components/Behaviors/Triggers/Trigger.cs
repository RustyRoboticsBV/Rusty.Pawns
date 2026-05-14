using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// A base class for all pawn triggers.
/// </summary>
[GlobalClass, Icon("./Trigger.svg")]
public abstract partial class Trigger : PawnBehavior
{
    /* Public methods. */
    /// <summary>
    /// Called at the start of the update loop.
    /// </summary>
    public void TryInvoke(double deltaTime)
    {
        if (Check(deltaTime))
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i) is Effect effect)
                    effect.Invoke(deltaTime);
            }
        }
    }

    /* Protected methods. */
    /// <summary>
    /// Check if this trigger's effects must be invoked.
    /// </summary>
    protected abstract bool Check(double deltaTime);
}