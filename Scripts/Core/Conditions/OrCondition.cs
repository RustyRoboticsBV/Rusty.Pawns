using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// A condition that evaluates to true when at least one condition within a set of conditions is true.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Conditions/OrCondition.svg")]
    public sealed partial class OrCondition : Condition
    {
        /* Public properties. */
        [Export] Condition[] Operands { get; set; } = new Condition[0];

        /* Protected methods. */
        protected override bool DoEvaluate(Pawn pawn)
        {
            // Check if at least one of the operands is true.
            if (Operands == null)
                return true;

            foreach (Condition operand in Operands)
            {
                if (operand.Evaluate(pawn))
                    return true;
            }
            return false;
        }
    }
}