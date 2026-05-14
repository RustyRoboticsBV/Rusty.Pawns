using Godot;

namespace Rusty.Pawns.Examples;

[GlobalClass]
public partial class SimplePlayerController : PawnDriver
{
    /* Public properties. */
    [Export] int Jumps { get; set; } = 2;
    [Export] float CoyoteTime { get; set; } = 0.0833f;

    /* Private methods. */
    private float MoveX { get; set; }
    private bool Jump { get; set; }
    private int JumpsLeft { get; set; }
    private float CoyoteTimeLeft { get; set; }
    private bool Grabbing { get; set; }
    private float MoveY { get; set; }

    /* Godot overrides. */
    public override void _EnterTree()
    {
        JumpsLeft = Jumps;
        CoyoteTimeLeft = CoyoteTime;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (Input.IsKeyPressed(Key.Z))
            {
                if (!Jump && JumpsLeft > 0)
                {
                    Pawn.GetComponent<JumpAction>().Jump();
                    JumpsLeft--;
                }
                Jump = true;
            }
            else
            {
                if (Jump)
                    Pawn.GetComponent<JumpAction>().CancelJump();
                Jump = false;
            }

            if (Input.IsKeyPressed(Key.Ctrl))
            {
                if (!Grabbing)
                {
                    Pawn.GetComponent<ToggleCondition>("IsGrabbing").State = true;
                    Grabbing = true;
                }
            }
            else
            {
                if (Grabbing)
                {
                    Pawn.GetComponent<ToggleCondition>("IsGrabbing").State = false;
                    Grabbing = false;
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Pawn.CheckCondition<IsGroundedCondition>() && !Pawn.GetComponent<JumpAction>().IsJumping)
        {
            JumpsLeft = Jumps;
            CoyoteTimeLeft = CoyoteTime;
        }
        else if (JumpsLeft == Jumps)
        {
            if (CoyoteTimeLeft <= 0)
            {
                JumpsLeft--;
                CoyoteTimeLeft = CoyoteTime;
            }
            else
                CoyoteTimeLeft -= (float)delta;
        }

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