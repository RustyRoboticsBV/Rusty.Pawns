using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A horizontal movement action.
/// </summary>
[GlobalClass, Icon("./WalkAction.svg")]
public sealed partial class WalkAction : MovementActionX<WalkProperties>
{
    /* Public properties. */
    public override bool DescendsSlopes => true;

    public float WalkFactor { get; private set; }

    /* Public methods. */
    public void WalkRight() => Walk(1f);

    public void WalkLeft() => Walk(-1f);

    public void Walk(float walkFactor)
    {
        WalkFactor = walkFactor;
    }

    public override void ForceStop()
    {
        base.ForceStop();
        WalkFactor = 0f;
    }

    /* Protected methods. */
    protected override Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        // Get current and target speed.
        Speed targetSpeed = WalkFactor * CurrentProperties.TopSpeed;
        Speed newSpeed = CurrentSpeed;

        // Determine speed.

        // Case 1: Not walking.
        if (CurrentSpeed == 0f && targetSpeed == 0f)
        { }

        // Case 2: Turning.
        else if (CurrentSpeed > 0f && targetSpeed < 0f || CurrentSpeed < 0f && targetSpeed > 0f)
        {
            Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.TopSpeed, -CurrentProperties.TopSpeed, CurrentProperties.TurnTime);
            newSpeed = CurrentSpeed.Step(targetSpeed, (double)acceleration * deltaTime);
        }

        // Case 3: Initial speed.
        else if (CurrentSpeed == 0f && targetSpeed != 0f)
        {
            if (CurrentProperties.AccelerationTime == 0)
                newSpeed = WalkFactor * CurrentProperties.TopSpeed;
            else
                newSpeed = WalkFactor * CurrentProperties.StartSpeed;
        }

        // Case 4: Accelerating.
        else if (CurrentSpeed.Abs() < targetSpeed.Abs())
        {
            Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.StartSpeed, CurrentProperties.TopSpeed, CurrentProperties.AccelerationTime);
            newSpeed = CurrentSpeed.Step(targetSpeed, (double)acceleration * deltaTime);
        }

        // Case 5: Decelerating.
        else if (CurrentSpeed.Abs() > targetSpeed.Abs())
        {
            Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.TopSpeed, 0f, CurrentProperties.DecelerationTime);
            newSpeed = CurrentSpeed.Step(0f, (double)acceleration * deltaTime);
        }

        // Stop when moving into a wall.
        if (CurrentSpeed < 0f && (pawn.ToLeftAdjacent.IsWall || pawn.ToLeftAdjacent.IsSteepGround || pawn.ToLeftAdjacent.IsSteepCeiling)
            || CurrentSpeed > 0f && (pawn.ToRightAdjacent.IsWall || pawn.ToRightAdjacent.IsSteepGround || pawn.ToRightAdjacent.IsSteepCeiling))
        {
            newSpeed = 0f;
        }

        return targetSpeed;
    }
}