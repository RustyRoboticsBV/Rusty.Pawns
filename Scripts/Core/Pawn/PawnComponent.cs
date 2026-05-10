using Godot;

namespace Rusty.Pawns;

/// <summary>
/// Base class for pawn component nodes.
/// </summary>
[GlobalClass, Icon("./PawnComponent.svg")]
public abstract partial class PawnComponent : Node3D
{
    /* Public properties. */
    /// <summary>
    /// The pawn that this pawn component belongs to.
    /// </summary>
    public Pawn Pawn { get; private set; }

    /* Godot overrides. */
    public sealed override void _EnterTree() { }

    public sealed override void _Ready() { }

    /* Public methods. */
    /// <summary>
    /// Initialize this pawn component.
    /// </summary>
    public void Init(Pawn pawn)
    {
        Pawn = pawn;
        OnInit(pawn);
    }

    /// <summary>
    /// Check whether or not this component is enabled and whether its conditions hold true.
    /// </summary>
    public virtual bool IsActive(Pawn pawn) => true;

    /* Protected methods. */
    /// <summary>
    /// Called when this pawn component is initialized.
    /// </summary>
    protected virtual void OnInit(Pawn pawn) { }
}