using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns
{
    /// <summary>
    /// Action that keeps track of both a speed and a 2D direction vector.
    /// </summary>
    public abstract partial class ActionDirectional<T> : ActionWithProperties<T> where T : ActionProperties
    {
        /* Public properties. */
        public Vector2 CurrentDirection { get; set; }
        public Speed CurrentSpeed { get; set; }
        public Distance CurrentMovementX { get; set; }
        public Distance CurrentMovementY { get; set; }
        public FaceDirectionX CurrentFaceDirectionX { get; set; } = FaceDirectionX.NoChange;
        public FaceDirectionY CurrentFaceDirectionY { get; set; } = FaceDirectionY.NoChange;

        /* Public methods. */
        public override void ForceStop()
        {
            base.ForceStop();
            CurrentDirection = Vector2.Zero;
            CurrentSpeed = 0d;
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
                CurrentSpeed = 0d;
        }

        public override void UpdateMovement(double deltaTime, Pawn pawn)
        {
            if (CurrentProperties == null)
            {
                CurrentMovementX = 0d;
                CurrentMovementY = 0d;
            }

            CurrentMovementX = Distance.FromVT(CurrentSpeed * CurrentDirection.X, deltaTime);
            CurrentMovementY = Distance.FromVT(CurrentSpeed * CurrentDirection.Y, deltaTime);
        }

        public override void UpdateFaceDirection(double deltaTime, Pawn pawn)
        {
            if (CurrentMovementX < 0d)
                CurrentFaceDirectionX = FaceDirectionX.Left;
            else if (CurrentMovementX > 0d)
                CurrentFaceDirectionX = FaceDirectionX.Right;
            else
                CurrentFaceDirectionX = FaceDirectionX.NoChange;

            if (CurrentMovementY < 0d)
                CurrentFaceDirectionY = FaceDirectionY.Down;
            else if (CurrentMovementY > 0d)
                CurrentFaceDirectionY = FaceDirectionY.Up;
            else
                CurrentFaceDirectionY = FaceDirectionY.NoChange;
        }

        public sealed override Vector2 GetSpeed()
        {
            return CurrentSpeed * CurrentDirection;
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