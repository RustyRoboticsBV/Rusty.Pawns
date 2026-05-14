using Godot;

using Rusty.Pawns;

[GlobalClass]
public partial class JumpTrigger : CancelTrigger
{
    /* Public properties. */
    [Export] public int Jumps { get; set; } = 2;
    [Export] public float CoyoteTime { get; set; } = 0.083f;
    [Export] public float BufferTime { get; set; } = 0.083f;
    [Export] public IsGroundedCondition IsGroundedCondition { get; set; }

    /* Private properties. */
    private int JumpsLeft { get; set; }
    private float CoyoteTimeLeft { get; set; }
    private float BufferTimeLeft { get; set; }
    private bool MustCancel { get; set; }

    /* Public methods. */
    /// <summary>
    /// Try to queue a jump.
    /// </summary>
    public void TryJump()
    {
        BufferTimeLeft = BufferTime;
    }

    /// <summary>
    /// Try to queue a jump cancel.
    /// </summary>
    public void TryCancel()
    {
        BufferTimeLeft = 0f;
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
        if (BufferTimeLeft > 0f)
        {
            if (JumpsLeft > 0)
            {
                InvokeEffect.Invoke(deltaTime);
                JumpsLeft--;
                BufferTimeLeft = 0f;
            }
            BufferTimeLeft -= (float)deltaTime;
        }
        if (MustCancel)
        {
            CancelEffect.Invoke(deltaTime);
            MustCancel = false;
        }
    }
}