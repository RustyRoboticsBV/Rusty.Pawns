using Godot;

namespace Rusty.Pawns.Examples;

[GlobalClass]
public partial class SimplePlayerController : Node3D
{
    /* Public properties. */
	[Export] Pawn Pawn { get; set; }

    /* Private methods. */
    private float MoveX { get; set; }
    
    /* Godot overrides. */
    public override void _Process(double delta)
    {
        MoveX = 0f;
        if (Input.IsKeyPressed(Key.Left))
            MoveX -= 1f;
        if (Input.IsKeyPressed(Key.Right))
            MoveX += 1f;
    }

    public override void _PhysicsProcess(double delta)
    {
        WalkAction walkAction = Pawn.GetComponent<WalkAction>();
        walkAction.Walk(MoveX);
    }
}