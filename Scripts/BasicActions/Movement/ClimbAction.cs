using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A vertical climbing movement action.
/// </summary>
[GlobalClass, Icon("./ClimbAction.svg")]
public sealed partial class ClimbAction : MovementActionY<ClimbProperties>
{
    /* Public properties. */
    public float ClimbFactor { get; private set; }

    /* Public methods. */
    public void ClimbUp() => Climb(1f);

    public void ClimbDown() => Climb(-1f);

    public void Climb(float climbFactor)
    {
        ClimbFactor = climbFactor;
    }

    public override void ForceStop()
    {
        base.ForceStop();
        ClimbFactor = 0f;
    }

    /* Protected methods. */
    protected override Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        // Get current and target speed.
        Speed targetSpeed = ClimbFactor * CurrentProperties.TopSpeed;
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
            if (CurrentProperties.AccelerationTime == 0f)
                newSpeed = ClimbFactor * CurrentProperties.TopSpeed;
            else if (CurrentProperties.StartSpeed != 0f)
                newSpeed = ClimbFactor * CurrentProperties.StartSpeed;
            else
            {
                Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.StartSpeed, CurrentProperties.TopSpeed, CurrentProperties.AccelerationTime);
                newSpeed = CurrentSpeed.Step(targetSpeed, (double)acceleration * deltaTime);
            }
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
        if (CurrentSpeed < 0f && pawn.BelowAdjacent.IsGround || CurrentSpeed > 0f && pawn.AboveAdjacent.IsCeiling)
            newSpeed = 0f;

        return newSpeed;
    }
}