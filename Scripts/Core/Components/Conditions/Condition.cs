using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A condition, which can be used to dynamically enable/disable other pawn components.
/// </summary>
[GlobalClass, Icon("./Condition.svg")]
public abstract partial class Condition : PawnComponent
{
    /* Public properties. */
    [Export] public bool InvertResult { get; set; }

    /* Public methods. */
    /// <summary>
    /// Check if this condition holds true.
    /// </summary>
    public bool Evaluate(Pawn pawn)
    {
        if (InvertResult)
            return !EvaluateMe(pawn);
        else
            return EvaluateMe(pawn);
    }

    /* Protected methods. */
    /// <summary>
    /// Run the evaluation logic.
    /// </summary>
    protected abstract bool EvaluateMe(Pawn pawn);
}