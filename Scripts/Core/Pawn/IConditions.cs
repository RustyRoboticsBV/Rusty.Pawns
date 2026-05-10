using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// An interface for pawn components that have a list of conditions.
/// </summary>
public interface IConditions
{
    /* Public properties. */
    /// <summary>
    /// If set to false, the pawn component will not affect anything.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// The list of conditions for this pawn component to be active.
    /// </summary>
    public Array<Condition> Conditions { get; }

    /* Public methods. */
    /// <summary>
    /// Check if this pawn child is active (all of its conditions are true) and enabled.
    /// </summary>
    public bool CheckActiveAndEnabled(Pawn pawn)
    {
        if (!Enabled)
            return false;

        for (int i = 0; i < Conditions.Count; i++)
        {
            if (!Conditions[i].Evaluate(pawn))
                return false;
        }

        return true;
    }
}