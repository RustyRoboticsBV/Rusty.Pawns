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
        return GetGravityAcceleration(CurrentProperties.GravityMultiplier);
    }

    protected override Speed CalculateSpeed(double deltaTime, Pawn pawn)
    {
        if (CurrentAcceleration == 0f)
            return 0f;
        Speed speed = base.CalculateSpeed(deltaTime, pawn);
        Speed topSpeed = GetFallSpeed(CurrentProperties.TopSpeed);
        if (speed < topSpeed)
            speed = topSpeed;
        return speed;
    }
}