namespace Modules.L2.Pawns
{
    /// <summary>
    /// A pawn child that can trigger an event.
    /// </summary>
    public partial class Trigger : PawnComponent
    {
        /* Public methods. */
        /// <summary>
        /// Called before the pawn starts updating the properties of all actions. The triggers are handled in the order
        /// of their node tree position.
        /// </summary>
        public virtual void PreUpdateProperties(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called before the pawn starts updating the speed of all actions. The triggers are handled in the order of
        /// their node tree position.
        /// </summary>
        public virtual void PreUpdateSpeed(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called before the pawn starts updating the movement of all actions. The triggers are handled in the order of
        /// their node tree position.
        /// </summary>
        public virtual void PreUpdateMovement(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called before the pawn starts updating the face direction of all actions. The triggers are handled in the order
        /// of their node tree position.
        /// </summary>
        public virtual void PreUpdateFaceDirection(double deltaTime, Pawn pawn) { }
    }
}