using Godot;
using Modules.L0.Quantities;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// Action that keeps track of both horizontal and vertical speed and movement.
    /// </summary>
    public abstract partial class Action2D<T> : ActionWithProperties<T> where T : ActionProperties
    {
        /* Public properties. */
        public Speed CurrentSpeedX { get; set; }
        public Speed CurrentSpeedY { get; set; }
        public Distance CurrentMovementX { get; set; }
        public Distance CurrentMovementY { get; set; }
        public FaceDirectionX CurrentFaceDirectionX { get; set; } = FaceDirectionX.NoChange;
        public FaceDirectionY CurrentFaceDirectionY { get; set; } = FaceDirectionY.NoChange;

        /* Public methods. */
        public override void ForceStop()
        {
            base.ForceStop();
            CurrentSpeedX = 0d;
            CurrentSpeedY = 0d;
            CurrentMovementX = 0d;
            CurrentMovementY = 0d;
            CurrentFaceDirectionX = FaceDirectionX.NoChange;
            CurrentFaceDirectionY = FaceDirectionY.NoChange;
        }

        public override void UpdateProperties(double deltaTime, Pawn pawn)
        {
            CurrentProperties = GetActiveProperties(pawn);
        }

        public override void UpdateSpeed(double deltaTime, Pawn pawn)
        {
            if (CurrentProperties == null)
                ForceStop();
        }

        public override void UpdateMovement(double deltaTime, Pawn pawn)
        {
            if (CurrentProperties == null)
                ForceStop();

            CurrentMovementX = Distance.Calculate(CurrentSpeedX, deltaTime);
            CurrentMovementY = Distance.Calculate(CurrentSpeedY, deltaTime);
        }

        public override void UpdateFaceDirection(double deltaTime, Pawn pawn)
        {
            if (CurrentMovementX < 0f)
                CurrentFaceDirectionX = FaceDirectionX.Left;
            else if (CurrentMovementX > 0f)
                CurrentFaceDirectionX = FaceDirectionX.Right;
            else
                CurrentFaceDirectionX = FaceDirectionX.NoChange;

            if (CurrentMovementY < 0f)
                CurrentFaceDirectionY = FaceDirectionY.Down;
            else if (CurrentMovementY > 0f)
                CurrentFaceDirectionY = FaceDirectionY.Up;
            else
                CurrentFaceDirectionY = FaceDirectionY.NoChange;
        }

        public sealed override Vector2 GetSpeed()
        {
            return new Vector2(CurrentSpeedX, CurrentSpeedY);
        }

        public sealed override Vector2 GetMovement()
        {
            return new Vector2(CurrentMovementX, CurrentMovementY);
        }

        public sealed override FaceDirection GetFaceDirection()
        {
            return new FaceDirection(CurrentFaceDirectionX, CurrentFaceDirectionY);
        }
    }
}