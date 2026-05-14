using Godot;

namespace Rusty.Pawns.Examples;

[GlobalClass]
public partial class SimplePlayerController : Node3D
{
    /* Public properties. */
	[Export] Pawn Pawn { get; set; }

    /* Private methods. */
    private float MoveX { get; set; }
    private bool Jump { get; set; }

    /* Godot overrides. */
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (Input.IsKeyPressed(Key.Z))
            {
                if (!Jump)
                    Pawn.GetComponent<JumpAction>().Jump();
                Jump = true;
            }
            else
            {
                if (Jump)
                    Pawn.GetComponent<JumpAction>().CancelJump();
                Jump = false;
            }

        }
    }

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