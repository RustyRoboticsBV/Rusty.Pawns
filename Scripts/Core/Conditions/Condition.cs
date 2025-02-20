using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// A condition, which can be used to dynamically enable/disable other pawn components, including other conditions.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Conditions/Condition.svg")]
    public abstract partial class Condition : PawnComponent
    {
        /* Public methods. */
        /// <summary>
        /// Checks if this condition is true. If the condition is not active, it is deemed irrelevant and this will always return
        /// true.
        /// </summary>
        public bool Evaluate(Pawn pawn)
        {
            // If we're not active, this condition is considered irrelevant and always returns true.
            if (!CheckActive(pawn))
                return true;

            // Else, evaluate.
            return DoEvaluate(pawn);
        }

        /// <summary>
        /// Checks if this condition is true. If the condition is not active, it is deemed irrelevant and this will always return
        /// true.
        /// </summary>
        public bool Evaluate()
        {
            return Evaluate(Pawn);
        }

        /* Protected methods. */
        /// <summary>
        /// The evaluation logic of this condition.
        /// </summary>
        protected virtual bool DoEvaluate(Pawn pawn)
        {
            return true;
        }
    }
}