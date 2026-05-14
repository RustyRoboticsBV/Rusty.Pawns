using Godot;

namespace Rusty.Pawns.Examples;

[GlobalClass]
public partial class SimplePlayerController : Node3D
{
    /* Public properties. */
	[Export] Pawn Pawn { get; set; }
    [Export] int Jumps { get; set; } = 2;
    [Export] float CoyoteTime { get; set; } = 0.0833f;

    /* Private methods. */
    private float MoveX { get; set; }
    private bool Jump { get; set; }
    private int JumpsLeft { get; set; }
    private float CoyoteTimeLeft { get; set; }

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
        }
    }

    public override void _Process(double delta)
    {
        if (Pawn.CheckCondition("IsGrounded") && !Pawn.GetComponent<JumpAction>().IsJumping)
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
    }

    public override void _PhysicsProcess(double delta)
    {
        WalkAction walkAction = Pawn.GetComponent<WalkAction>();
        walkAction.Walk(MoveX);
    }
}