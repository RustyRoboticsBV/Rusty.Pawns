using Godot;

namespace Rusty.Pawns.Examples;

[GlobalClass]
public partial class SimplePlayerController : PawnDriver
{
    /* Fields. */
    [Export] Key LeftKey = Key.Left;
    [Export] Key RightKey = Key.Right;
    [Export] Key DownKey = Key.Down;
    [Export] Key UpKey = Key.Up;
    [Export] Key JumpKey = Key.Z;
    [Export] Key GrabKey = Key.Ctrl;
    [Export] Key DashKey = Key.Alt;

    /* Private methods. */
    private float MoveX { get; set; }
    private float MoveY { get; set; }
    private bool Jump { get; set; }
    private bool Grab { get; set; }
    private bool Dash { get; set; }

    /* Godot overrides. */
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (Input.IsKeyPressed(JumpKey))
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

            if (Input.IsKeyPressed(GrabKey))
            {
                Grab = true;
            }
            else
            {
                Grab = false;
            }

            if (Input.IsKeyPressed(DashKey))
            {
                if (!Dash)
                {
                    Pawn.GetComponent<SprintTrigger>().TryStart();
                    Dash = true;
                }
            }
            else
            {
                if (Dash)
                {
                    Pawn.GetComponent<SprintTrigger>().TryStop();
                    Dash = false;
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        MoveX = 0f;
        if (Input.IsKeyPressed(LeftKey))
            MoveX -= 1f;
        if (Input.IsKeyPressed(RightKey))
            MoveX += 1f;

        MoveY = 0f;
        if (Input.IsKeyPressed(DownKey))
            MoveY -= 1f;
        if (Input.IsKeyPressed(UpKey))
            MoveY += 1f;

        if (Grab)
            Pawn.GetComponent<GrabTrigger>().TryGrab();
        else
            Pawn.GetComponent<GrabTrigger>().TryRelease();
    }

    public override void _PhysicsProcess(double delta)
    {
        WalkAction walkAction = Pawn.GetComponent<WalkAction>();
        walkAction.Walk(MoveX);

        ClimbAction climbAction = Pawn.GetComponent<ClimbAction>();
        climbAction.Climb(MoveY);
    }
}