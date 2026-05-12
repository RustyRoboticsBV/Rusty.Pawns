using System;
using Godot;

namespace Rusty.Pawns;

/// <summary>
/// Base class for pawn component nodes.
/// </summary>
[GlobalClass, Icon("./PawnComponent.svg")]
public abstract partial class PawnComponent : Node3D
{
    /* Fields. */
    private bool initialized;

    /* Public properties. */
    /// <summary>
    /// The name of this pawn component.
    /// </summary>
    [Export] public string Alias { get; protected set; } = "";

    /// <summary>
    /// The pawn that this pawn component belongs to.
    /// </summary>
    public Pawn Pawn { get; private set; }

    /* Godot overrides. */
    public sealed override void _EnterTree() { /* Force use of Init instead. */ }

    public sealed override void _Ready() { /* Force use of Init instead. */ }

    /* Public methods. */
    /// <summary>
    /// Initialize this pawn component.
    /// </summary>
    public void Init(Pawn pawn)
    {
        if (initialized)
            throw new InvalidOperationException($"Already initialized pawn component '{GetPath()}'.");

        Pawn = pawn;
        OnInit(pawn);

        initialized = true;
    }

    /// <summary>
    /// Check whether or not this component is enabled and whether its conditions hold true.
    /// </summary>
    public virtual bool IsActive(Pawn pawn) => true;

    /// <summary>
    /// Get the name of this pawn component by which it can be retrieved from the pawn.
    /// This is the PawnComponentName if it was set, otherwise it is the path relative to the pawn.
    /// </summary>
    public string GetSearchName()
    {
        if (string.IsNullOrEmpty(Alias))
            return GetPawnComponentPath();
        return Alias;
    }

    /* Protected methods. */
    /// <summary>
    /// Called when this pawn component is initialized.
    /// </summary>
    protected virtual void OnInit(Pawn pawn) { }

    /* Private methods. */
    /// <summary>
    /// Get the path to the pawn component from the root pawn (or the scene root if there is no pawn).
    /// </summary>
    private string GetPawnComponentPath()
    {
        string str = Name;
        Node node = GetParent();
        while (node != null && !(node is Pawn))
        {
            str = node.Name + '/' + str;
            node = node.GetParent();
        }
        return str;
    }
}