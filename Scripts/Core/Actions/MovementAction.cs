using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A base class for actions that maintain acceleration, speed, movement and face direction.
/// </summary>
[GlobalClass, Icon("./MovementAction.svg")]
public abstract partial class MovementAction : Action, IActionWithProperties
{
    /* Public properties. */
    public virtual bool DescendsSlopes => false;

    /* Public methods. */
    /// <summary>
    /// Instantly removes all acceleration, speed and movement.
    /// </summary>
    public abstract void ForceStop();

    /// <summary>
    /// Select a set of action properties to use for the next update loop, depending on the current context.
    /// </summary>
    public abstract void UpdateProperties(double deltaTime, Pawn pawn);

    /// <summary>
    /// Update the current acceleration.
    /// </summary>
    public abstract void UpdateAcceleration(double deltaTime, Pawn pawn);

    /// <summary>
    /// Update the current speed.
    /// </summary>
    public abstract void UpdateSpeed(double deltaTime, Pawn pawn);

    /// <summary>
    /// Calculate the move distance for this update loop.
    /// </summary>
    public abstract void UpdateMovement(double deltaTime, Pawn pawn);

    /// <summary>
    /// Determine the face direction for this loop.
    /// </summary>
    public abstract void UpdateFaceDirection(double deltaTime, Pawn pawn);

    /// <summary>
    /// Get the current action properties.
    /// </summary>
    public abstract ActionProperties GetProperties();

    /// <summary>
    /// Get the current acceleration as a Vector2.
    /// </summary>
    public abstract Vector2 GetAcceleration();

    /// <summary>
    /// Get the current speed as a Vector2.
    /// </summary>
    public abstract Vector2 GetSpeed();

    /// <summary>
    /// Get the current movement as a Vector2.
    /// </summary>
    public abstract Vector2 GetMovement();

    /// <summary>
    /// Get the current face direction in both directions.
    /// </summary>
    public abstract FaceDirection GetFaceDirection();

    /// <summary>
    /// Get whether or not the action has a non-zero movement.
    /// </summary>
    public virtual bool IsMoving()
    {
        return GetMovement() != Vector2.Zero;
    }

    /* Protected methods. */
    /// <summary>
    /// Get the first property set that has all of its conditions met.
    /// </summary>
    protected T GetActiveProperties<T>(Pawn pawn)
        where T : ActionProperties
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is T properties && properties.CheckActiveAndEnabled(pawn))
                return properties;
        }
        return null;
    }
}