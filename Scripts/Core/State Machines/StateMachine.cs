using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// A state machine. The machine can be in exactly one state.
    /// </summary>
    [GlobalClass]
    public abstract partial class StateMachine : PawnComponent
    {
        /* Public methods. */
        /// <summary>
        /// Called before the pawn starts updating the properties of all actions. The state machines are handled in the order
        /// of their array index.
        /// </summary>
        public virtual void PreUpdateProperties(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called before the pawn starts updating the speed of all actions. The state machines are handled in the order of
        /// their array index.
        /// </summary>
        public virtual void PreUpdateSpeed(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called before the pawn starts updating the movement of all actions. The state machines are handled in the order of
        /// their array index.
        /// </summary>
        public virtual void PreUpdateMovement(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called before the pawn starts updating the face direction of all actions. The state machines are handled in the order
        /// of their array index.
        /// </summary>
        public virtual void PreUpdateFaceDirection(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Enter a new state. Make sure that the object's type matches the state machine's state type.
        /// </summary>
        public abstract void Enter(object newState);

        /// <summary>
        /// Check if the state machine is in a specific state. Make sure that the object's type matches the state machine's state
        /// type.
        /// </summary>
        public abstract bool IsIn(object state);
    }
}