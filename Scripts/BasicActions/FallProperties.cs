using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A set of properties for a fall action.
/// </summary>
[GlobalClass]
public sealed partial class FallProperties : ActionProperties
{
    /* Public properties. */
    [Export] public float GravityMultiplier { get; set; } = 5f;
    [Export] public float TopSpeed { get; set; } = 30f;
}
