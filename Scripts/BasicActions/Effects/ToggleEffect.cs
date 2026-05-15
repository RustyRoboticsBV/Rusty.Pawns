using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A toggle effect.
/// </summary>
[GlobalClass]
public sealed partial class ToggleEffect : Effect
{
    /* Public properties. */
    [Export] public ToggleCondition Toggle { get; set; }
    [Export] public bool Value { get; set; }

    /* Public methods. */
    public override void Invoke(double deltaTime)
    {
        Toggle.State = Value;
    }
}