using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A two-dimensional movement action.
/// </summary>
public abstract partial class MovementAction2D<T> : MovementAction
    where T : ActionProperties
{
    /* Public properties. */
    public T CurrentProperties { get; set; }
    public Acceleration CurrentAccelerationX { get; set; }
    public Acceleration CurrentAccelerationY { get; set; }
    public Speed CurrentSpeedX { get; set; }
    public Speed CurrentSpeedY { get; set; }
    public Distance CurrentMovementX { get; set; }
    public Distance CurrentMovementY { get; set; }
    public FaceDirectionX CurrentFaceDirectionX { get; set; } = FaceDirectionX.NoChange;
    public FaceDirectionY CurrentFaceDirectionY { get; set; } = FaceDirectionY.NoChange;

    /* Public methods. */
    public override void ForceStop()
    {
        CurrentAccelerationX = 0f;
        CurrentAccelerationY = 0f;
        CurrentSpeedX = 0f;
        CurrentSpeedY = 0f;
        CurrentMovementX = 0f;
        CurrentMovementY = 0f;
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
        {
            CurrentAccelerationX = 0f;
            CurrentAccelerationY = 0f;
        }
        else
        {
            Vector2 acceleration = CalculateAcceleration(deltaTime, pawn);
            CurrentAccelerationX = acceleration.X;
            CurrentAccelerationY = acceleration.Y;
        }
    }

    public sealed override void UpdateSpeed(double deltaTime, Pawn pawn)
    {
        if (CurrentProperties == null)
        {
            CurrentSpeedX = 0f;
            CurrentSpeedY = 0f;
        }
        else
        {
            Vector2 speed = CalculateSpeed(deltaTime, pawn);
            CurrentSpeedX = speed.X;
            CurrentSpeedY = speed.Y;
        }
    }

    public sealed override void UpdateDistance(double deltaTime, Pawn pawn)
    {
        if (CurrentProperties == null)
        {
            CurrentMovementX = 0f;
            CurrentMovementY = 0f;
        }
        else
        {
            Vector2 movement = CalculateMovement(deltaTime, pawn);
            CurrentMovementX = movement.X;
            CurrentMovementY = movement.Y;
        }
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
        return new Vector2(CurrentAccelerationX, CurrentAccelerationY);
    }

    public sealed override Vector2 GetSpeed()
    {
        return new Vector2(CurrentSpeedX, CurrentSpeedY);
    }

    public sealed override Vector2 GetDistance()
    {
        return new Vector2(CurrentMovementX, CurrentMovementY);
    }

    public sealed override FaceDirection GetFaceDirection()
    {
        return new FaceDirection(CurrentFaceDirectionX, CurrentFaceDirectionY);
    }

    public sealed override bool IsMoving()
    {
        return CurrentMovementX != 0f || CurrentMovementY != 0f;
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
    protected virtual Vector2 CalculateAcceleration(double deltaTime, Pawn pawn)
    {
        return Vector2.Zero;
    }

    /// <summary>
    /// Determine the speed for this update loop.
    /// </summary>
    protected virtual Vector2 CalculateSpeed(double deltaTime, Pawn pawn)
    {
        Speed x = Speed.EndSpeedFromUAT(CurrentSpeedX, CurrentAccelerationX, deltaTime);
        Speed y = Speed.EndSpeedFromUAT(CurrentSpeedY, CurrentAccelerationY, deltaTime);
        return new(x, y);
    }

    /// <summary>
    /// Determine the movement for this update loop.
    /// </summary>
    protected virtual Vector2 CalculateMovement(double deltaTime, Pawn pawn)
    {
        Distance x = Distance.FromVT(CurrentSpeedX, deltaTime);
        Distance y = Distance.FromVT(CurrentSpeedY, deltaTime);
        return new(x, y);
    }

    /// <summary>
    /// Determine the face direction for this update loop.
    /// </summary>
    protected virtual FaceDirection CalculateFaceDirection(double deltaTime, Pawn pawn)
    {
        FaceDirectionX x = FaceDirectionX.NoChange;
        if (CurrentMovementX < 0f)
            x = FaceDirectionX.Left;
        else if (CurrentMovementX > 0f)
            x = FaceDirectionX.Right;

        FaceDirectionY y = FaceDirectionY.NoChange;
        if (CurrentMovementY < 0f)
            y = FaceDirectionY.Down;
        else if (CurrentMovementY > 0f)
            y = FaceDirectionY.Up;

        return new(x, y);
    }
}