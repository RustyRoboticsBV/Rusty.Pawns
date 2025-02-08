using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A condition that evaluates to true when, in a set of conditions, exactly one condition is true.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Conditions/XorCondition.svg")]
    public sealed partial class XorCondition : Condition
    {
        /* Public properties. */
        [Export] Condition[] Operands { get; set; } = new Condition[0];

        /* Protected methods. */
        protected override bool DoEvaluate(Pawn pawn)
        {
            // Check if exactly one of the operands is true.
            if (Operands == null)
                return true;

            bool matchFound = false;
            foreach (Condition operand in Operands)
            {
                if (operand.Evaluate(pawn))
                {
                    if (matchFound)
                        return false;
                    else
                        matchFound = true;
                }
            }
            return matchFound;
        }
    }
}