using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// A set of properties for a walk action.
    /// </summary>
    [GlobalClass]
    public sealed partial class WalkProperties : ActionProperties
    {
        /* Public properties. */
        [Export] public float StartSpeed { get; set; } = 10f;
        [Export] public float TopSpeed { get; set; } = 20f;
        [Export] public float AccelerationTime { get; set; } = 0.05f;
        [Export] public float DecelerationTime { get; set; } = 0.2f;
        [Export] public float TurnTime { get; set; } = 0.1f;
    }
}
