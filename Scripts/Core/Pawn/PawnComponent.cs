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
    /// Whether or not this pawn component has been enabled.
    /// </summary>
    [Export] public bool Enabled { get; set; } = true;
    /// <summary>
    /// Whether or not this pawn component is accessible through the pawn's GetChild method.
    /// </summary>
    [Export] public virtual bool Discoverable { get; set; } = true;

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
    public bool IsActive(Pawn pawn) => Enabled;

    /* Protected methods. */
    /// <summary>
    /// Called when this pawn component is initialized.
    /// </summary>
    protected virtual void OnInit(Pawn pawn) { }
}