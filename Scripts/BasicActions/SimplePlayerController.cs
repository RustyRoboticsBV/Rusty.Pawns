using Godot;

namespace Rusty.Pawns.Examples;

[GlobalClass]
public partial class SimplePlayerController : PawnDriver
{
    /* Private methods. */
    private float MoveX { get; set; }
    private float MoveY { get; set; }
    private bool Jump { get; set; }
    private bool Grab { get; set; }

    /* Godot overrides. */
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (Input.IsKeyPressed(Key.Z))
            {
                if (!Jump)
                {
                    Pawn.GetComponent<JumpTrigger>().TryJump();
                    Jump = true;
                }
            }
            else
            {
                if (Jump)
                {
                    Pawn.GetComponent<JumpTrigger>().TryCancel();
                    Jump = false;
                }
            }

            if (Input.IsKeyPressed(Key.Ctrl))
            {
                if (!Grab)
                {
                    Pawn.GetComponent<ToggleCondition>("IsGrabbing").State = true;
                    Grab = true;
                }
            }
            else
            {
                if (Grab)
                {
                    Pawn.GetComponent<ToggleCondition>("IsGrabbing").State = false;
                    Grab = false;
                }
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

        MoveY = 0f;
        if (Input.IsKeyPressed(Key.Down))
            MoveY -= 1f;
        if (Input.IsKeyPressed(Key.Up))
            MoveY += 1f;
    }

    public override void _PhysicsProcess(double delta)
    {
        WalkAction walkAction = Pawn.GetComponent<WalkAction>();
        walkAction.Walk(MoveX);

        ClimbAction climbAction = Pawn.GetComponent<ClimbAction>();
        climbAction.Climb(MoveY);
    }
}