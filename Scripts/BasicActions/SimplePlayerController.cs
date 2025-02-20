using Godot;

namespace Rusty.Pawns.Examples
{
    [GlobalClass]
	public partial class SimplePlayerController : Node3D
	{
		[Export] Pawn Pawn { get; set; }

        /* Godot overrides. */
        public override void _Process(double delta)
        {
            float moveX = 0f;
            if (Input.IsKeyPressed(Key.Left))
                moveX -= 1f;
            if (Input.IsKeyPressed(Key.Right))
                moveX += 1f;

            WalkAction walkAction = Pawn.Actions.Get<WalkAction>();
            walkAction.Walk(moveX);
        }
    }
}