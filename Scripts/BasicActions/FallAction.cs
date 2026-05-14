using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns;

/// <summary>
/// A vertical falling action.
/// </summary>
[GlobalClass, Icon("./FallAction.svg")]
public sealed partial class FallAction : MovementActionY<FallProperties>
{
    /* Protected methods. */
    protected override Acceleration CalculateAcceleration(double deltaTime, Pawn pawn)
    {
        if (Pawn.BelowAdjacent.IsGround)
            return 0f;
        return -(GetDefaultGravity() * CurrentProperties.GravityMultiplier).Abs();
    }

    protected override Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        if (CurrentAcceleration == 0f)
            return 0f;
        Speed speed = base.CalculateSpeed(deltaTime, pawn);
        Speed topSpeed = -new Speed(CurrentProperties.TopSpeed).Abs();
        if (speed < topSpeed)
            speed = topSpeed;
        return speed;
    }

    protected override Distance CalculateMovement(double deltaTime, Pawn pawn)
    {
        return base.CalculateMovement(deltaTime, pawn);
    }
}