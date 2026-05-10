using Godot;
using Godot.Collections;

namespace Rusty.Pawns;

/// <summary>
/// Base class for pawn actions.
/// </summary>
[GlobalClass, Icon("./Action.svg")]
public abstract partial class Action : PawnComponent, IConditions
{
    /* Public properties. */
    public Array<Condition> Conditions { get; private set; } = new();

    /* Public methods. */
    /// <summary>
    /// Runs before the properties update loop.
    /// </summary>
    public virtual void BeforeUpdateProperties(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Runs after the properties update loop and before the acceleration update loop.
    /// </summary>
    public virtual void AfterUpdateProperties(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Runs after the acceleration update loop before the speed update loop.
    /// </summary>
    public virtual void AfterUpdateAcceleration(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Runs after the speed update loop before the movement update loop.
    /// </summary>
    public virtual void AfterUpdateSpeed(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Runs after the movement update loop before the face direction update loop.
    /// </summary>
    public virtual void AfterUpdateMovement(double deltaTime, Pawn pawn) { }

    /// <summary>
    /// Runs after the face direction update loop.
    /// </summary>
    public virtual void AfterUpdateFaceDirection(double deltaTime, Pawn pawn) { }

    public bool CheckActiveAndEnabled(Pawn pawn)
    {
        return ((IConditions)this).CheckActiveAndEnabled(pawn);
    }
}