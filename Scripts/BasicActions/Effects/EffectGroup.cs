using Godot;

namespace Rusty.Pawns;

/// <summary>
/// An effect that fires off other effects.
/// </summary>
[GlobalClass]
public sealed partial class EffectGroup : Effect
{
    /* Public methods. */
    public override void Invoke(double deltaTime)
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is Effect effect)
                effect.Invoke(deltaTime);
        }
    }
}