using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A grab effect.
/// </summary>
[GlobalClass]
public sealed partial class GrabEffect : Effect
{
    /* Public properties. */
    [Export] public GrabAction Action { get; set; }

    /* Public methods. */
    public override void Invoke(double deltaTime)
    {
        Action.Grab();
    }
}