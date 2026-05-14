using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A set of properties for a jump action.
/// </summary>
[GlobalClass]
public sealed partial class JumpProperties : ActionProperties
{
    /* Public properties. */
    [Export] public float Height { get; set; } = 5f;
    [Export] public float GravityMultiplier { get; set; } = 5f;
    [Export] public float CancelledGravityMultiplier { get; set; } = 25f;
}
