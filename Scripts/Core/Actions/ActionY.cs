using Godot;
using Rusty.Quantities;

namespace Rusty.Pawns
{
    /// <summary>
    /// An action that keeps track of horizontal speed and movement.
    /// </summary>
    public abstract partial class ActionY<T> : ActionWithProperties<T> where T : ActionProperties
    {
        /* Public properties. */
        public Speed CurrentSpeed { get; set; }
        public Distance CurrentMovement { get; set; }
        public FaceDirectionY CurrentFaceDirection { get; set; }

        /* Public methods. */
        public override void ForceStop()
        {
            base.ForceStop();
            CurrentSpeed = 0f;
            CurrentMovement = 0f;
            CurrentFaceDirection = FaceDirectionY.NoChange;
        }

        public override void UpdateProperties(double deltaTime, Pawn pawn)
        {
            CurrentProperties = GetActiveProperties(pawn);
        }

        public override void UpdateSpeed(double deltaTime, Pawn pawn)
        {
            if (CurrentProperties == null)
                CurrentSpeed = 0f;
        }

        public override void UpdateMovement(double deltaTime, Pawn pawn)
        {
            if (CurrentProperties == null)
                CurrentMovement = 0f;

            CurrentMovement = Distance.FromVT(CurrentSpeed, deltaTime);
        }

        public override void UpdateFaceDirection(double deltaTime, Pawn pawn)
        {
            if (CurrentMovement < 0f)
                CurrentFaceDirection = FaceDirectionY.Down;
            else if (CurrentMovement > 0f)
                CurrentFaceDirection = FaceDirectionY.Up;
            else
                CurrentFaceDirection = FaceDirectionY.NoChange;
        }

        public sealed override Vector2 GetSpeed()
        {
            return new Vector2(Vector2.Up.X, Vector3.Up.Y) * CurrentSpeed;
        }

        public sealed override Vector2 GetMovement()
        {
            return new Vector2(Vector3.Up.X, Vector3.Up.Y) * CurrentMovement;
        }

        public sealed override FaceDirection GetFaceDirection()
        {
            return new FaceDirection(FaceDirectionX.NoChange, CurrentFaceDirection);
        }

        public override bool IsMoving()
        {
            return CurrentMovement != 0d;
        }
    }
}