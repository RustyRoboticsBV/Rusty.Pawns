using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A set of properties for a grab action.
/// </summary>
[GlobalClass]
public sealed partial class GrabProperties : ActionProperties
{
    /* Public properties. */
    [Export] public float StartSpeed { get; set; } = 0f;
    [Export] public float TopSpeed { get; set; } = 30f;
    [Export] public float AccelerationTime { get; set; } = 0.25f;
}
