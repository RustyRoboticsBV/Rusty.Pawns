using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// Base class for pawn component nodes that contain a conditions list and an enabled toggle.
/// </summary>
[GlobalClass, Icon("./ConditionalComponent.svg")]
public abstract partial class ConditionalComponent : PawnComponent
{
    /* Public properties. */
    /// <summary>
    /// Whether or not this pawn component has been enabled.
    /// </summary>
    [Export] public bool Enabled { get; set; } = true;
    /// <summary>
    /// The conditions that must hold true for this pawn component to be active.
    /// </summary>
    [Export] public Array<Condition> Conditions { get; set; } = new();

    /* Public methods. */
    public override bool IsActive(Pawn pawn)
    {
        if (!Enabled)
            return false;

        foreach (Condition condition in Conditions)
        {
            if (!condition.Evaluate(pawn))
                return false;
        }

        return true;
    }
}