using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns
{
    /// <summary>
    /// A horizontal movement action.
    /// </summary>
    [GlobalClass]
    public sealed partial class WalkAction : ActionX<WalkProperties>
    {
        /* Public properties. */
        public override bool DescendsSlopes => true;

        public float WalkFactor { get; private set; }

        /* Public methods. */
        public void Walk(float walkFactor)
        {
            WalkFactor = walkFactor;
        }

        public override void ForceStop()
        {
            base.ForceStop();
            WalkFactor = 0f;
        }

        public override void UpdateSpeed(double deltaTime, Pawn pawn)
        {
            if (CurrentProperties == null)
            {
                CurrentSpeed = 0f;
                return;
            }

            // Get current and target speed.
            Speed targetSpeed = WalkFactor * CurrentProperties.TopSpeed;

            // Determine speed.

            // Case 1: Not walking.
            if (CurrentSpeed == 0f && targetSpeed == 0f)
            { }

            // Case 2: Turning.
            else if (CurrentSpeed > 0f && targetSpeed < 0f || CurrentSpeed < 0f && targetSpeed > 0f)
            {
                Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.TopSpeed, -CurrentProperties.TopSpeed, CurrentProperties.TurnTime);
                CurrentSpeed = CurrentSpeed.Step(targetSpeed, (double)acceleration * deltaTime);
            }

            // Case 3: Initial speed.
            else if (CurrentSpeed == 0f && targetSpeed != 0f)
                CurrentSpeed = WalkFactor * CurrentProperties.StartSpeed;

            // Case 4: Accelerating.
            else if (CurrentSpeed.Abs() < targetSpeed.Abs())
            {
                Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.StartSpeed, CurrentProperties.TopSpeed, CurrentProperties.AccelerationTime);
                CurrentSpeed = CurrentSpeed.Step(targetSpeed, (double)acceleration * deltaTime);
            }

            // Case 5: Decelerating.
            else if (CurrentSpeed.Abs() > targetSpeed.Abs())
            {
                Acceleration acceleration = Acceleration.FromUVT(CurrentProperties.TopSpeed, 0f, CurrentProperties.DecelerationTime);
                CurrentSpeed = CurrentSpeed.Step(0f, (double)acceleration * deltaTime);
            }

            // Stop when moving into a wall.
            if (CurrentSpeed < 0f && (pawn.ToLeftAdjacent.IsWall || pawn.ToLeftAdjacent.IsSteepGround || pawn.ToLeftAdjacent.IsSteepCeiling)
                || CurrentSpeed > 0f && (pawn.ToRightAdjacent.IsWall || pawn.ToRightAdjacent.IsSteepGround || pawn.ToRightAdjacent.IsSteepCeiling))
            {
                CurrentSpeed = 0f;
            }
        }

        public override void UpdateFaceDirection(double deltaTime, Pawn pawn)
        {
            if (CurrentMovement < 0f)
                CurrentFaceDirection = FaceDirectionX.Left;
            else if (CurrentMovement > 0f)
                CurrentFaceDirection = FaceDirectionX.Right;
            else
                CurrentFaceDirection = FaceDirectionX.NoChange;
        }

        public sealed override bool IsMoving()
        {
            return WalkFactor != 0f;
        }
    }
}
