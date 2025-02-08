using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// Base class for pawn action modifiers.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Modifier.svg")]
    public abstract partial class Modifier : PawnComponent
    {
        /* Public methods. */
        /// <summary>
        /// Called after the pawn has finished updating the properties of all actions. The modifiers are handled in the order
        /// of their array index.
        /// </summary>
        public virtual void PostUpdateProperties(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called after the pawn has finished updating the speed of all actions. The modifiers are handled in the order of
        /// their array index.
        /// </summary>
        public virtual void PostUpdateSpeed(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called after the pawn has finished updating the movement of all actions. The modifiers are handled in the order of
        /// their array index.
        /// </summary>
        public virtual void PostUpdateMovement(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called after the pawn has finished updating the face direction of all actions. The modifiers are handled in the order
        /// of their array index.
        /// </summary>
        public virtual void PostUpdateFaceDirection(double deltaTime, Pawn pawn) { }
    }
}