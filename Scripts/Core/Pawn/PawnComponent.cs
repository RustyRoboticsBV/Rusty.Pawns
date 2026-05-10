using Godot;
using System.Collections.Generic;

namespace Rusty.Pawns;

/// <summary>
/// Base class for pawn component nodes.
/// </summary>
[GlobalClass, Icon("./PawnChild.svg")]
public abstract partial class PawnComponent : Node3D
{
    /* Public properties. */
    [Export] public virtual bool Discoverable { get; set; } = true;

    public Pawn Pawn { get; private set; }

    /* Godot overrides. */
    public sealed override void _EnterTree() { }

    public sealed override void _Ready() { }

    /* Public methods. */
    public void Init(Pawn pawn)
    {
        Pawn = pawn;
        OnInit(pawn);
    }

    /* Protected methods. */
    /// <summary>
    /// Called when this pawn component is initialized.
    /// </summary>
    protected virtual void OnInit(Pawn pawn) { }
}