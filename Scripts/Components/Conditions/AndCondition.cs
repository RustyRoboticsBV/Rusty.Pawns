using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A condition that evaluates to true when a set of other conditions are all true.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Conditions/AndCondition.svg")]
    public sealed partial class AndCondition : Condition
    {
        /* Public properties. */
        [Export] Condition[] Operands { get; set; } = new Condition[0];

        /* Protected methods. */
        protected override bool DoEvaluate(Pawn pawn)
        {
            // Check if all of the operands are true.
            if (Operands == null)
                return true;

            foreach (Condition operand in Operands)
            {
                if (!operand.Evaluate(pawn))
                    return false;
            }
            return true;
        }
    }
}