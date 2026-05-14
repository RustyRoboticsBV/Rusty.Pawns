using Godot;

[GlobalClass]
public partial class AutoRotate : Node3D
{
    [Export] float Distance { get; set; } = 10f;
    [Export] float MoveSpeed { get; set; } = 10f;
    [Export] float RotateSpeed { get; set; } = 30f;

    Vector3 StartPos { get; set; }

    public override void _EnterTree()
    {
        StartPos = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        GlobalPosition = StartPos + Vector3.Right * Mathf.Sin(Time.GetTicksMsec() / 1000f * MoveSpeed) * Distance;
        RotateZ(Mathf.DegToRad((float)delta * RotateSpeed));
    }
}
