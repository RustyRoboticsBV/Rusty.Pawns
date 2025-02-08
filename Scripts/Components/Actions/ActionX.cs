using Godot;
using Modules.L0.Quantities;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// An action that keeps track of horizontal speed and movement.
    /// </summary>
    public abstract partial class ActionX<T> : ActionWithProperties<T> where T : ActionProperties
    {
        /* Public properties. */
        public Speed CurrentSpeed { get; set; }
        public Distance CurrentMovement { get; set; }
        public FaceDirectionX CurrentFaceDirection { get; set; }

        /* Public methods. */
        public override void ForceStop()
        {
            base.ForceStop();
            CurrentSpeed = 0f;
            CurrentMovement = 0f;
            CurrentFaceDirection = FaceDirectionX.NoChange;
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

            CurrentMovement = Distance.Calculate(CurrentSpeed, deltaTime);
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

        public sealed override Vector2 GetSpeed()
        {
            return Vector2.Right * CurrentSpeed;
        }

        public sealed override Vector2 GetMovement()
        {
            return Vector2.Right * CurrentMovement;
        }

        public sealed override FaceDirection GetFaceDirection()
        {
            return new FaceDirection(CurrentFaceDirection, FaceDirectionY.NoChange);
        }

        public override bool IsMoving()
        {
            return CurrentMovement != 0d;
        }
    }
}