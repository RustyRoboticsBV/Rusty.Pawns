using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A class that organizes raycasts in a box shape, meant for a 2D game that uses 3D physics.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Raycasting/RaycastBox.svg")]
    public sealed partial class RaycastBox : Raycaster
    {
        /* Public properties. */
        [Export] public override Vector2 Size { get; set; } = Vector2.One;
        [Export] public override float SkinWidth { get; set; } = 0.01f;
        [Export] public Vector2I RayNumber { get; set; } = Vector2I.One * 3;
        [Export(PropertyHint.Layers3DPhysics)] public uint LayerMask { get; set; } = 1;

        public Vector3 TopLeft => new(-Size.X / 2f, Size.Y / 2f, 0f);
        public Vector3 TopRight => new(Size.X / 2f, Size.Y / 2f, 0f);
        public Vector3 BottomLeft => -TopRight;
        public Vector3 BottomRight => -TopLeft;

        /* Private properties. */
        private RaycastArray RaycastLeft { get; set; }
        private RaycastArray RaycastRight { get; set; }
        private RaycastArray RaycastDown { get; set; }
        private RaycastArray RaycastUp { get; set; }
        private RayCast3D TopLeftRay { get; set; }
        private RayCast3D TopRightRay { get; set; }
        private RayCast3D BottomLeftRay { get; set; }
        private RayCast3D BottomRightRay { get; set; }

        /* Public methods. */
        public override ShapecastResult CheckLeft(float distance)
        {
            // Handle negative values by checking the opposite side.
            if (distance < 0f)
                return CheckRight(-distance);

            // Update raycast array.
            RaycastLeft.RayNumber = RayNumber.X;
            RaycastLeft.StartOffset = TopLeft + new Vector3(SkinWidth, -SkinWidth, 0f);
            RaycastLeft.EndOffset = BottomLeft + new Vector3(SkinWidth, SkinWidth, 0f);
            RaycastLeft.RayDirection = Vector3.Left;
            RaycastLeft.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastLeft.Check(distance + SkinWidth), SkinWidth);
        }

        public override ShapecastResult CheckRight(float distance)
        {
            // Handle negative values by checking the opposite side.
            if (distance < 0f)
                return CheckLeft(-distance);

            // Update raycast array.
            RaycastRight.RayNumber = RayNumber.X;
            RaycastRight.StartOffset = TopRight + new Vector3(-SkinWidth, -SkinWidth, 0f);
            RaycastRight.EndOffset = BottomRight + new Vector3(-SkinWidth, SkinWidth, 0f);
            RaycastRight.RayDirection = Vector3.Right;
            RaycastRight.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastRight.Check(distance + SkinWidth), SkinWidth);
        }

        public override ShapecastResult CheckDown(float distance)
        {
            // Handle negative values by checking the opposite side.
            if (distance < 0f)
                return CheckUp(-distance);

            // Update raycast array.
            RaycastDown.RayNumber = RayNumber.Y;
            RaycastDown.StartOffset = BottomLeft + new Vector3(SkinWidth, SkinWidth, 0f);
            RaycastDown.EndOffset = BottomRight + new Vector3(-SkinWidth, SkinWidth, 0f);
            RaycastDown.RayDirection = Vector3.Down;
            RaycastDown.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastDown.Check(distance + SkinWidth), SkinWidth);
        }

        public override ShapecastResult CheckUp(float distance)
        {
            // Handle negative values by checking the opposite side.
            if (distance < 0f)
                return CheckDown(-distance);

            // Update raycast array.
            RaycastUp.RayNumber = RayNumber.Y;
            RaycastUp.StartOffset = TopLeft + new Vector3(SkinWidth, -SkinWidth, 0f);
            RaycastUp.EndOffset = TopRight + new Vector3(-SkinWidth, -SkinWidth, 0f);
            RaycastUp.RayDirection = Vector3.Up;
            RaycastUp.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastUp.Check(distance + SkinWidth), SkinWidth);
        }

        public override RaycastResult CheckLeftEdge(bool startAtTop)
        {
            if (startAtTop)
                return CheckRay(TopLeftRay, TopLeft + Vector3.Left * SkinWidth, Vector3.Down * Size.Y, LayerMask);
            else
                return CheckRay(BottomLeftRay, BottomLeft + Vector3.Left * SkinWidth, Vector3.Up * Size.Y, LayerMask);
        }

        public override RaycastResult CheckRightEdge(bool startAtTop)
        {
            if (startAtTop)
                return CheckRay(TopRightRay, TopRight + Vector3.Right * SkinWidth, Vector3.Down * Size.Y, LayerMask);
            else
                return CheckRay(BottomRightRay, BottomRight + Vector3.Right * SkinWidth, Vector3.Up * Size.Y, LayerMask);
        }

        public override RaycastResult CheckBottomEdge(bool startAtRight)
        {
            if (startAtRight)
                return CheckRay(BottomRightRay, BottomRight, Vector3.Left * Size.X, LayerMask);
            else
                return CheckRay(BottomLeftRay, BottomLeft, Vector3.Right * Size.X, LayerMask);
        }

        public override RaycastResult CheckTopEdge(bool startAtRight)
        {
            if (startAtRight)
                return CheckRay(TopRightRay, TopRight, Vector3.Left * Size.X, LayerMask);
            else
                return CheckRay(TopLeftRay, TopLeft, Vector3.Right * Size.X, LayerMask);
        }

        public override ShapecastResult CheckInteriorFromLeft()
        {
            // Update raycast array.
            RaycastLeft.RayNumber = RayNumber.X;
            RaycastLeft.StartOffset = TopLeft + new Vector3(SkinWidth, -SkinWidth, 0f);
            RaycastLeft.EndOffset = BottomLeft + new Vector3(SkinWidth, SkinWidth, 0f);
            RaycastLeft.RayDirection = Vector3.Right;
            RaycastLeft.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastLeft.Check(Size.X - SkinWidth), -SkinWidth);
        }

        public override ShapecastResult CheckInteriorFromRight()
        {
            // Update raycast array.
            RaycastRight.RayNumber = RayNumber.X;
            RaycastRight.StartOffset = TopRight + new Vector3(-SkinWidth, -SkinWidth, 0f);
            RaycastRight.EndOffset = BottomRight + new Vector3(-SkinWidth, SkinWidth, 0f);
            RaycastRight.RayDirection = Vector3.Left;
            RaycastRight.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastRight.Check(Size.X - SkinWidth), -SkinWidth);
        }

        public override ShapecastResult CheckInteriorFromBottom()
        {
            // Update raycast array.
            RaycastDown.RayNumber = RayNumber.Y;
            RaycastDown.StartOffset = BottomLeft + new Vector3(SkinWidth, SkinWidth, 0f);
            RaycastDown.EndOffset = BottomRight + new Vector3(-SkinWidth, SkinWidth, 0f);
            RaycastDown.RayDirection = Vector3.Up;
            RaycastDown.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastDown.Check(Size.Y - SkinWidth), -SkinWidth);
        }

        public override ShapecastResult CheckInteriorFromTop()
        {
            // Update raycast array.
            RaycastUp.RayNumber = RayNumber.Y;
            RaycastUp.StartOffset = TopLeft + new Vector3(SkinWidth, -SkinWidth, 0f);
            RaycastUp.EndOffset = TopRight + new Vector3(-SkinWidth, -SkinWidth, 0f);
            RaycastUp.RayDirection = Vector3.Down;
            RaycastUp.LayerMask = LayerMask;

            // Perform check.
            return new(RaycastUp.Check(Size.Y - SkinWidth), SkinWidth);
        }

        /* Protected methods. */
        protected override void OnInit(Pawn pawn)
        {
            RaycastLeft = new();
            AddChild(RaycastLeft);

            RaycastRight = new();
            AddChild(RaycastRight);

            RaycastDown = new();
            AddChild(RaycastDown);

            RaycastUp = new();
            AddChild(RaycastUp);

            TopLeftRay = new();
            TopLeftRay.HitFromInside = true;
            AddChild(TopLeftRay);

            TopRightRay = new();
            TopRightRay.HitFromInside = true;
            AddChild(TopRightRay);

            BottomLeftRay = new();
            BottomLeftRay.HitFromInside = true;
            AddChild(BottomLeftRay);

            BottomRightRay = new();
            BottomRightRay.HitFromInside = true;
            AddChild(BottomRightRay);
        }
    }
}