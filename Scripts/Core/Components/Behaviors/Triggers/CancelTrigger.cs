using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A trigger with a cancel event.
/// </summary>
[GlobalClass, Icon("./CancelTrigger.svg")]
public abstract partial class CancelTrigger : Trigger
{
    /* Public properties. */
    [Export] public Effect CancelEffect { get; set; }
}