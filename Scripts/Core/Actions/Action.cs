using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// Base class for pawn actions.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Actions/Action.svg")]
    public abstract partial class Action : PawnComponent
    {
        /* Public properties. */
        public virtual bool DescendsSlopes => false;

        /* Public methods. */
        /// <summary>
        /// Forcefully removes all speed and movement.
        /// </summary>
        public virtual void ForceStop() { }

        /// <summary>
        /// The recommended method to do property updating in. By default, it simply gets the first set of properties whose
        /// conditions match the pawn's current state. During the pawn's update loop, it calls this method for every action (in
        /// the order of their index on the pawn class), after it has updated its surroundings.
        /// </summary>
        public virtual void UpdateProperties(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// The recommended method to do speed updating in. It doesn't do anything by default. During the pawn's update loop, it
        /// calls this method for every action (in the order of their index on the pawn class), after the property update step.
        /// </summary>
        public virtual void UpdateSpeed(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// This is the recommended method to do movement updating in. By default, it simply takes the speed and multiplies it
        /// with the time delta. During the pawn's update loop, it calls this method for every action (in the order of their
        /// index on the pawn class), after the speed update step.
        /// </summary>
        public virtual void UpdateMovement(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// This is the recommended method to do face direction updating in. By default, it doesn't do anything. During the
        /// pawn's update loop, it calls this method for every action (in the order of their index on the pawn class), after the
        /// movement update loop.
        /// </summary>
        public virtual void UpdateFaceDirection(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Get the updated action properties.
        /// </summary>
        public abstract ActionProperties GetProperties();

        /// <summary>
        /// Get the updated speed as a Vector2.
        /// </summary>
        public abstract Vector2 GetSpeed();

        /// <summary>
        /// Get the updated movement as a Vector2.
        /// </summary>
        public abstract Vector2 GetMovement();

        /// <summary>
        /// Get the updated face direction in both directions.
        /// </summary>
        public abstract FaceDirection GetFaceDirection();

        /// <summary>
        /// Get whether or not the action is currently attempting to move.
        /// </summary>
        public virtual bool IsMoving()
        {
            return GetMovement() != Vector2.Zero;
        }
    }
}