using Godot;
using Godot.Collections;

namespace Rusty.Pawns
{
    /// <summary>
    /// A node that organizes raycasts along a line relative to its parent node.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Raycasting/RaycastArray.svg")]
    public sealed partial class RaycastArray : Node3D
    {
        /* Public properties. */
        [Export] public int RayNumber { get; set; } = 3;
        [Export] public Vector3 StartOffset { get; set; } = Vector3.Left * 0.5f;
        [Export] public Vector3 EndOffset { get; set; } = Vector3.Right * 0.5f;
        [Export] public Vector3 RayDirection { get; set; } = Vector3.Up;
        [Export(PropertyHint.Layers3DPhysics)] public uint LayerMask { get; set; } = 1;

        /* Private properties. */
        private Array<RayCast3D> Rays { get; set; } = new Array<RayCast3D>();

        /* Constructors & destructors. */
        ~RaycastArray()
        {
            for (int i = 0; i < Rays.Count; i++)
            {
                Rays[i].QueueFree();
            }
        }

        /* Public methods. */
        /// <summary>
        /// Get a raycast result objects with the result of what the rays hit.
        /// </summary>
        public RaycastResult Check(float distance)
        {
            // Make sure we have enough rays.
            while (Rays.Count < RayNumber)
            {
                RayCast3D ray = new RayCast3D();
                ray.Name = "Ray Cast " + Rays.Count;
                ray.CollisionMask = LayerMask;
                Rays.Add(ray);
                AddChild(ray);
            }

            // Cast with each ray.
            RaycastResult closest = RaycastResult.None;
            for (int i = 0; i < RayNumber; i++)
            {
                RaycastResult result = RayCast(i, distance);
                if (result.HasHit && result.HitDistance < closest.HitDistance || !closest.HasHit)
                    closest = result;
            }
            return closest;
        }

        /* Private methods. */
        /// <summary>
        /// Perform a single raycast.
        /// </summary>
        private RaycastResult RayCast(int index, float distance)
        {
            RayCast3D ray = Rays[index];

            // Update raycast object.
            ray.Position = StartOffset + (EndOffset - StartOffset) * (index / (RayNumber - 1f));
            ray.TargetPosition = RayDirection * distance;
            ray.CollisionMask = LayerMask;
            ray.ForceRaycastUpdate();

            // Create result.
            Vector3 globalTarget = ray.GlobalPosition + ray.TargetPosition;

            RaycastResult result = new RaycastResult(ray.GlobalPosition, globalTarget, null, globalTarget, Vector3.Zero);

            if (ray.IsColliding() && ray.GetCollider() is CollisionObject3D collider)
            {
                result.Collider = collider;
                result.HitPosition = ray.GetCollisionPoint();
                result.HitNormal = ray.GetCollisionNormal();
            }

            // Return result.
            return result;
        }
    }
}