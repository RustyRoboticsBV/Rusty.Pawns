using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

[GlobalClass, Icon("./ActionProperties.svg")]
public abstract partial class ActionProperties : PawnComponent, IConditions
{
    /* Public properties. */
    public Array<Condition> Conditions { get; private set; } = new();

    /* Public methods. */
    /// <summary>
    /// Called when this set of properties starts being an action's active set.
    /// </summary>
    public virtual void OnSelected(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Called when this set of properties stops being an action's active set.
    /// </summary>
    public virtual void OnDeselected(double deltaTime, Pawn pawn) { }

    public bool CheckActiveAndEnabled(Pawn pawn)
    {
        return ((IConditions)this).CheckActiveAndEnabled(pawn);
    }
}