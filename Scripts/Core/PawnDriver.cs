using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A base class for pawn drivers.
/// </summary>
public abstract partial class PawnDriver : Node3D
{
    /* Public properties. */
    public Pawn Pawn { get; private set; }

    /* Godot overrides. */
    public sealed override void _Ready()
    {
        Pawn = FindPawn(this);
        Pawn.Driver = this;
        Init();
    }

    /* Protected methods. */
    /// <summary>
    /// Initialize. This gets called from _Ready.
    /// </summary>
    protected virtual void Init() { }

    /* Private methods. */
    private static Pawn FindPawn(Node node)
    {
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            if (node.GetChild(i) is Pawn pawn)
                return pawn;
        }
        return null;
    }
}