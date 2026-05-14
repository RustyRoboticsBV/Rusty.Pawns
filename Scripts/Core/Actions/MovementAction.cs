using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A base class for actions that maintain acceleration, speed, movement and face direction.
/// </summary>
[GlobalClass, Icon("./MovementAction.svg")]
public abstract partial class MovementAction : Action, IActionWithProperties
{
    /* Public properties. */
    /// <summary>
    /// Whether or not this movement moves down sloped ground.
    /// </summary>
    public virtual bool DescendsSlopes => false;

    /// <summary>
    /// Whether or not this movement moves down sloped ceilings.
    /// </summary>
    public virtual bool DescendsSlopedCeilings => false;

    /* Public methods. */
    /// <summary>
    /// Instantly removes all acceleration, speed and movement.
    /// </summary>
    public abstract void ForceStop();

    // Update methods.

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
    public abstract void UpdateDistance(double deltaTime, Pawn pawn);

    /// <summary>
    /// Determine the face direction for this loop.
    /// </summary>
    public abstract void UpdateFaceDirection(double deltaTime, Pawn pawn);

    /// <summary>
    /// Finish up the update loop.
    /// </summary>
    public virtual void PostProcess(double deltaTime, Pawn pawn) { }

    // Getter methods.

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

    // Is not zero methods.

    /// <summary>
    /// Get whether or not the action has an active set of properties.
    /// </summary>
    public virtual bool HasProperties() => GetProperties() != null;

    /// <summary>
    /// Get whether or not the action has a non-zero acceleration.
    /// </summary>
    public virtual bool IsAccelerating() => GetAcceleration() != Vector2.Zero;

    /// <summary>
    /// Get whether or not the action has a non-zero speed.
    /// </summary>
    public virtual bool IsSpeeding() => GetSpeed() != Vector2.Zero;

    /// <summary>
    /// Get whether or not the action has a non-zero movement.
    /// </summary>
    public virtual bool IsMoving() => GetMovement() != Vector2.Zero;

    /// <summary>
    /// Get whether or not the action is facing in a direction.
    /// </summary>
    public virtual bool IsFacing() => GetFaceDirection() != FaceDirection.None;

    /* Protected methods. */
    /// <summary>
    /// Get the first property set that has all of its conditions met.
    /// </summary>
    protected T GetActiveProperties<T>(Pawn pawn)
        where T : ActionProperties
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is T properties && properties.IsActive(pawn))
                return properties;
        }
        return null;
    }
}