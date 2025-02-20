using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// A condition that evaluates to true when another condition does.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Conditions/ConditionReference.svg")]
    public sealed partial class ConditionReference : Condition
    {
        /* Public properties. */
        [Export] Condition Reference { get; set; }
        [Export] bool Not { get; set; }

        /* Protected methods. */
        protected override bool DoEvaluate(Pawn pawn)
        {
            if (Reference == null)
                return true;
            else
                return Reference.Evaluate(pawn) != Not;
        }
    }
}