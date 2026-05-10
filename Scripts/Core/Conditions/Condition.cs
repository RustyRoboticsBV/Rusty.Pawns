using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition, which can be used to dynamically enable/disable other pawn components, including other conditions.
/// </summary>
[GlobalClass, Icon("./Condition.svg")]
public partial class Condition : PawnComponent
{
    /* Public properties. */
    /// <summary>
    /// The boolean operator that this condition will apply to its child conditions.
    /// </summary>
    public Operator Operator { get; set; } = Operator.And;
    /// <summary>
    /// Negate the result of the expression.
    /// </summary>
    public bool Not { get; set; }

    /* Public methods. */
    /// <summary>
    /// Checks if this condition is true. If the condition is not active, it is deemed irrelevant and this will always return
    /// true. Child condition nodes are also checked.
    /// </summary>
    public bool Evaluate(Pawn pawn)
    {
        bool result = EvaluateSelf(pawn);

        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is Condition condition)
            {
                switch (Operator)
                {
                    case Operator.And:
                        result &= condition.Evaluate(pawn);
                        break;
                    case Operator.Or:
                        result |= condition.Evaluate(pawn);
                        break;
                    case Operator.XOr:
                        result ^= condition.Evaluate(pawn);
                        break;
                }
            }
        }

        if (Not)
            result = !result;

        return result;
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
    /// Evaluate this condition (does not include any potential child conditions).
    /// </summary>
    protected virtual bool EvaluateSelf(Pawn pawn)
    {
        return true;
    }
}