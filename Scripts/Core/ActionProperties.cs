using Godot;

namespace Rusty.Pawns
{
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/ActionProperties.svg")]
    public abstract partial class ActionProperties : PawnComponent
    {
        /// <summary>
        /// Called when this set of properties is activated.
        /// </summary>
        public virtual void OnActivate(double deltaTime, Pawn pawn) { }

        /// <summary>
        /// Called when this set of properties is deactivated.
        /// </summary>
        public virtual void OnDeactivate(double deltaTime, Pawn pawn) { }
    }
}