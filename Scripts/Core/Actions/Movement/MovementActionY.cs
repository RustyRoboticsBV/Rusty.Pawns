using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A vertical movement action.
/// </summary>
public abstract partial class MovementActionY<T> : MovementAction
    where T : ActionProperties
{
    /* Public constants. */
    public static readonly Vector2 Up = new Vector2(Vector2.Up.X, Vector3.Up.Y);

    /* Public properties. */
    public T CurrentProperties { get; set; }
    public Acceleration CurrentAcceleration { get; set; }
    public Speed CurrentSpeed { get; set; }
    public Distance CurrentMovement { get; set; }
    public FaceDirectionY CurrentFaceDirection { get; set; }

    /* Public methods. */
    public override void ForceStop()
    {
        CurrentAcceleration = 0f;
        CurrentSpeed = 0f;
        CurrentMovement = 0f;
        CurrentFaceDirection = FaceDirectionY.NoChange;
    }

    public sealed override void UpdateProperties(double deltaTime, Pawn pawn)
    {
        CurrentProperties = CalculateProperties(deltaTime, pawn);
    }

    public sealed override void UpdateAcceleration(double deltaTime, Pawn pawn)
    {
        if (CurrentProperties == null)
            CurrentAcceleration = 0f;
        else
            CurrentAcceleration = CalculateAcceleration(deltaTime, pawn);
    }

    public sealed override void UpdateSpeed(double deltaTime, Pawn pawn)
    {
        if (CurrentProperties == null)
            CurrentSpeed = 0f;
        else
            CurrentSpeed = CalculateSpeed(deltaTime, pawn);
    }

    public sealed override void UpdateDistance(double deltaTime, Pawn pawn)
    {
        if (CurrentProperties == null)
            CurrentMovement = 0f;
        else
            CurrentMovement = CalculateMovement(deltaTime, pawn);
    }

    public sealed override void UpdateFaceDirection(double deltaTime, Pawn pawn)
    {
        if (CurrentProperties == null)
            CurrentFaceDirection = FaceDirectionY.NoChange;
        else
            CurrentFaceDirection = CalculateFaceDirection(deltaTime, pawn);
    }

    public sealed override ActionProperties GetProperties()
    {
        return CurrentProperties;
    }

    public sealed override Vector2 GetAcceleration()
    {
        return Up * CurrentAcceleration;
    }

    public sealed override Vector2 GetSpeed()
    {
        return Up * CurrentSpeed;
    }

    public sealed override Vector2 GetDistance()
    {
        return Up * CurrentMovement;
    }

    public sealed override FaceDirection GetFaceDirection()
    {
        return new FaceDirection(FaceDirectionX.NoChange, CurrentFaceDirection);
    }

    public sealed override bool IsMoving()
    {
        return CurrentMovement != 0f;
    }

    /* Protected methods. */
    /// <summary>
    /// Get a gravity acceleration by multiplying a value with the default physics gravity from the project settings.
    /// </summary>
    protected static Acceleration GetGravityAcceleration(float multiplier)
    {
        float defaultGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
        return -Mathf.Abs(defaultGravity * multiplier);
    }

    /// <summary>
    /// Convert a value to a negative speed.
    /// </summary>
    protected static Speed GetFallSpeed(float speed)
    {
        return -Mathf.Abs(speed);
    }

    /// <summary>
    /// Determine the properties for this update loop.
    /// </summary>
    protected virtual T CalculateProperties(double deltaTime, Pawn pawn)
    {
        return GetActiveProperties<T>(pawn);
    }

    /// <summary>
    /// Determine the acceleration for this update loop.
    /// </summary>
    protected virtual Acceleration CalculateAcceleration(double deltaTime, Pawn pawn)
    {
        return 0f;
    }

    /// <summary>
    /// Determine the speed for this update loop.
    /// </summary>
    protected virtual Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        return Speed.EndSpeedFromUAT(CurrentSpeed, CurrentAcceleration, deltaTime);
    }

    /// <summary>
    /// Determine the movement for this update loop.
    /// </summary>
    protected virtual Distance CalculateMovement(double deltaTime, Pawn pawn)
    {
        return Distance.FromVT(CurrentSpeed, deltaTime);
    }

    /// <summary>
    /// Determine the face direction for this update loop.
    /// </summary>
    protected virtual FaceDirectionY CalculateFaceDirection(double deltaTime, Pawn pawn)
    {
        if (CurrentMovement < 0f)
            return FaceDirectionY.Down;
        else if (CurrentMovement > 0f)
            return FaceDirectionY.Up;
        else
            return FaceDirectionY.NoChange;
    }
}