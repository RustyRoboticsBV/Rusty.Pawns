using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A base class for nodes that organizes raycasts, meant for a 2D game that uses 3D physics.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Raycasting/Raycaster.svg")]
    public abstract partial class Raycaster : PawnComponent
    {
        public abstract Vector2 Size { get; set; }
        public abstract float SkinWidth { get; set; }

        /* Public methods. */
        /// <summary>
        /// Perform raycasts from the left side of box.
        /// </summary>
        public abstract ShapecastResult CheckLeft(float distance);
        /// <summary>
        /// Perform raycasts from the right side of box.
        /// </summary>
        public abstract ShapecastResult CheckRight(float distance);
        /// <summary>
        /// Perform raycasts from the down side of box.
        /// </summary>
        public abstract ShapecastResult CheckDown(float distance);
        /// <summary>
        /// Perform raycasts from the up side of box.
        /// </summary>
        public abstract ShapecastResult CheckUp(float distance);
        /// <summary>
        /// Perform raycasts from the left or right side of the box.
        /// </summary>
        public ShapecastResult CheckHorizontal(float distance)
        {
            if (distance >= 0f)
                return CheckRight(distance);
            else
                return CheckLeft(-distance);
        }
        /// <summary>
        /// Perform raycasts from the top or bottom side of the box.
        /// </summary>
        public ShapecastResult CheckVertical(float distance)
        {
            if (distance >= 0f)
                return CheckUp(distance);
            else
                return CheckDown(-distance);
        }

        /// <summary>
        /// Fire a raycast parallel to the right edge of the raycaster's bounding box.
        /// </summary>
        public abstract RaycastResult CheckRightEdge(bool startAtTop);
        /// <summary>
        /// Fire a raycast parallel to the left edge of the raycaster's bounding box.
        /// </summary>
        public abstract RaycastResult CheckLeftEdge(bool startAtTop);
        /// <summary>
        /// Fire a raycast parallel to the top edge of the raycaster's bounding box.
        /// </summary>
        public abstract RaycastResult CheckTopEdge(bool startAtLeft);
        /// <summary>
        /// Fire a raycast parallel to the bottom edge of the raycaster's bounding box.
        /// </summary>
        public abstract RaycastResult CheckBottomEdge(bool startAtLeft);

        /// <summary>
        /// Fire raycasts through the raycaster from the right side to the left side.
        /// </summary>
        public abstract ShapecastResult CheckInteriorFromRight();
        /// <summary>
        /// Fire raycasts through the raycaster from the left side to the right side.
        /// </summary>
        public abstract ShapecastResult CheckInteriorFromLeft();
        /// <summary>
        /// Fire raycasts through the raycaster from the top side to the bottom side.
        /// </summary>
        public abstract ShapecastResult CheckInteriorFromTop();
        /// <summary>
        /// Fire raycasts through the raycaster from the bottom side to the top side.
        /// </summary>
        public abstract ShapecastResult CheckInteriorFromBottom();

        /* Protected methods. */
        protected static RaycastResult CheckRay(RayCast3D ray, Vector3 localOrigin, Vector3 direction, float distance, uint layers)
        {
            return CheckRay(ray, localOrigin, direction * distance, layers);
        }

        protected static RaycastResult CheckRay(RayCast3D ray, Vector3 localOrigin, Vector3 localTarget, uint layers)
        {
            // Update ray.
            ray.Position = localOrigin;
            ray.TargetPosition = localTarget;
            ray.CollisionMask = layers;
            ray.ForceRaycastUpdate();

            // Check ray.
            Vector3 globalTarget = ray.GlobalPosition + localTarget;
            if (ray.IsColliding())
            {
                return new RaycastResult(ray.GlobalPosition, globalTarget,
                    ray.GetCollider() as CollisionObject3D, ray.GetCollisionPoint(), ray.GetCollisionNormal());
            }
            else
                return new RaycastResult(ray.GlobalPosition, globalTarget);
        }
    }
}