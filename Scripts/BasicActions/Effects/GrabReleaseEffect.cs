using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A release grab effect.
/// </summary>
[GlobalClass]
public sealed partial class GrabReleaseEffect : Effect
{
    /* Public properties. */
    [Export] public GrabAction Action { get; set; }

    /* Public methods. */
    public override void Invoke(double deltaTime)
    {
        Action.Release();
    }
}