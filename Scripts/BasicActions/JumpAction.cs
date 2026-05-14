using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A vertical jump action.
/// </summary>
[GlobalClass, Icon("./JumpAction.svg")]
public sealed partial class JumpAction : MovementActionY<JumpProperties>
{
    /* Public properties. */
    public bool IsJumping => MustStart || CurrentSpeed > 0f;

    /* Private properties. */
    private bool MustStart { get; set; }
    private bool LowJump { get; set; }

    /* Public methods. */
    public void Jump()
    {
        if (CurrentProperties != null)
        {
            MustStart = true;
            LowJump = false;
        }
    }

    public void CancelJump()
    {
        if (IsJumping)
            LowJump = true;
    }

    public override void ForceStop()
    {
        base.ForceStop();
        MustStart = false;
        LowJump = false;
    }

    /* Protected methods. */
    protected override JumpProperties CalculateProperties(double deltaTime, Pawn pawn)
    {
        if (!IsJumping)
            return base.CalculateProperties(deltaTime, pawn);
        else
            return CurrentProperties;
    }

    protected override Acceleration CalculateAcceleration(double deltaTime, Pawn pawn)
    {
        if (LowJump)
            return GetGravityAcceleration(CurrentProperties.CancelledGravityMultiplier);
        else
            return GetGravityAcceleration(CurrentProperties.GravityMultiplier);
    }

    protected override Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        // Do nothing if there is a ceiling above us.
        if (Pawn.AboveAdjacent.IsCeiling)
        {
            ForceStop();
            return 0f;
        }

        // Start a jump.
        if (MustStart)
        {
            MustStart = false;
            Acceleration gravity = GetGravityAcceleration(CurrentProperties.GravityMultiplier);
            return Speed.StartSpeedFromSVA(CurrentProperties.Height, 0f, gravity);
        }

        // Do nothing if there is no acceleration.
        if (CurrentAcceleration == 0f)
            return 0f;

        // Update speed.
        Speed speed = base.CalculateSpeed(deltaTime, pawn);
        if (speed < 0f)
            speed = 0f;
        return speed;
    }
}