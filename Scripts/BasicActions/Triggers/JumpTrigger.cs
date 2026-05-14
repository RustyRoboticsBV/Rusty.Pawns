using Godot;

using Rusty.Pawns;

[GlobalClass]
public partial class JumpTrigger : Trigger
{
    /* Public properties. */
    [Export] public int Jumps { get; set; } = 2;
    [Export] public float CoyoteTime { get; set; } = 0.083f;
    [Export] public HasDistanceCondition IsJumpingCondition { get; set; }
    [Export] public IsGroundedCondition IsGroundedCondition { get; set; }
    [Export] public Effect JumpEffect { get; set; }
    [Export] public Effect CancelJumpEffect { get; set; }

    /* Private properties. */
    private int JumpsLeft { get; set; }
    private float CoyoteTimeLeft { get; set; }
    private bool MustJump { get; set; }
    private bool MustCancel { get; set; }

    /* Public methods. */
    /// <summary>
    /// Try to queue a jump.
    /// </summary>
    public void TryJump()
    {
        MustJump = true;
    }

    /// <summary>
    /// Try to queue a jump cancel.
    /// </summary>
    public void TryCancel()
    {
        MustCancel = true;
    }

    public override void TryInvoke(double deltaTime)
    {
        bool isGrounded = IsGroundedCondition.Evaluate(Pawn);

        // Jumps and coyote time.
        if (isGrounded)
        {
            JumpsLeft = Jumps;
            CoyoteTimeLeft = CoyoteTime;
        }
        else if (JumpsLeft == Jumps)
        {
            if (CoyoteTimeLeft <= 0f)
                JumpsLeft--;
            else
                CoyoteTimeLeft -= (float)deltaTime;
        }

        // Jump.
        if (MustJump)
        {
            if (JumpsLeft > 0)
            {
                JumpEffect.Invoke(deltaTime);
                JumpsLeft--;
            }
            MustJump = false;
        }
        if (MustCancel)
        {
            CancelJumpEffect.Invoke(deltaTime);
            MustCancel = false;
        }
    }
}