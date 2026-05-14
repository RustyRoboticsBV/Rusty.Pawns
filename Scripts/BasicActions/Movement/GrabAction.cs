using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A horizontal magnet grab action.
/// </summary>
[GlobalClass, Icon("./GrabAction.svg")]
public sealed partial class GrabAction : MovementActionX<GrabProperties>
{
    /* Public properties. */
    public bool IsGrabbing { get; set; }
    public bool IsMovingLeft { get; set; }

    /* Public methods. */
    public void Grab()
    {
        IsGrabbing = true;
        IsMovingLeft = Pawn.IsFacingLeft;
    }

    public void Release()
    {
        IsGrabbing = false;
    }

    public override void ForceStop()
    {
        base.ForceStop();
        IsGrabbing = false;
    }

    /* Protected methods. */
    protected override Acceleration CalculateAcceleration(double deltaTime, Pawn pawn)
    {
        if (!IsGrabbing)
            return 0f;

        return Acceleration.FromUVT(
            AimSpeed(CurrentProperties.StartSpeed, IsMovingLeft),
            AimSpeed(CurrentProperties.TopSpeed, IsMovingLeft),
            CurrentProperties.AccelerationTime
        );
    }

    protected override Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        if (!IsGrabbing)
            return 0f;

        // Update speed.
        if (CurrentSpeed == 0f && CurrentProperties.StartSpeed != 0f)
            return AimSpeed(CurrentProperties.StartSpeed, IsMovingLeft);

        else
        {
            Speed speed = base.CalculateSpeed(deltaTime, pawn);
            speed = LimitSpeed(speed, CurrentProperties.TopSpeed);
            return speed;
        }
    }
}