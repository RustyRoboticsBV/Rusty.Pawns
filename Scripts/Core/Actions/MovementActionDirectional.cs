using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// Action that keeps track of both a 1D acceleration/speed/movement and a 2D direction vector.
/// </summary>
public abstract partial class MovementActionDirectional<T> : MovementAction
    where T : ActionProperties
{
    /* Public properties. */
    public T CurrentProperties { get; set; }
    public Acceleration CurrentAcceleration { get; set; }
    public Speed CurrentSpeed { get; set; }
    public Distance CurrentMovement { get; set; }
    public Vector2 CurrentDirection { get; set; }
    public FaceDirectionX CurrentFaceDirectionX { get; set; } = FaceDirectionX.NoChange;
    public FaceDirectionY CurrentFaceDirectionY { get; set; } = FaceDirectionY.NoChange;

    /* Public methods. */
    /// <summary>
    /// Instantly removes all acceleration, speed and movement. Does not change the direction.
    /// </summary>
    public override void ForceStop()
    {
        CurrentAcceleration = 0f;
        CurrentSpeed = 0f;
        CurrentMovement = 0f;
        CurrentFaceDirectionX = FaceDirectionX.NoChange;
        CurrentFaceDirectionY = FaceDirectionY.NoChange;
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
        {
            CurrentFaceDirectionX = 0f;
            CurrentFaceDirectionY = 0f;
        }
        else
        {
            FaceDirection faceDirection = CalculateFaceDirection(deltaTime, pawn);
            CurrentFaceDirectionX = faceDirection.X;
            CurrentFaceDirectionY = faceDirection.Y;
        }
    }

    public sealed override Vector2 GetAcceleration()
    {
        return CurrentAcceleration * CurrentDirection;
    }

    public sealed override Vector2 GetSpeed()
    {
        return CurrentSpeed * CurrentDirection;
    }

    public sealed override Vector2 GetMovement()
    {
        return CurrentMovement * CurrentDirection;
    }

    public sealed override FaceDirection GetFaceDirection()
    {
        return new FaceDirection(CurrentFaceDirectionX, CurrentFaceDirectionY);
    }

    public sealed override bool IsMoving()
    {
        return CurrentMovement != 0f;
    }

    /* Protected methods. */
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
    /// Determine the distance for this update loop.
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
    protected virtual FaceDirection CalculateFaceDirection(double deltaTime, Pawn pawn)
    {
        Vector2 movement = GetMovement();

        FaceDirectionX x = FaceDirectionX.NoChange;
        if (movement.X < 0f)
            x = FaceDirectionX.Left;
        else if (movement.X > 0f)
            x = FaceDirectionX.Right;

        FaceDirectionY y = FaceDirectionY.NoChange;
        if (movement.Y < 0f)
            y = FaceDirectionY.Down;
        else if (movement.Y > 0f)
            y = FaceDirectionY.Up;

        return new(x, y);
    }
}