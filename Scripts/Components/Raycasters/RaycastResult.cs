using Godot;
using System;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// Represents the result of one or more raycasts.
    /// </summary>
    [Serializable]
    public struct RaycastResult
    {
        /* Public properties. */
        public static RaycastResult None => new RaycastResult();

        /// <summary>
        /// The global origin of the raycast.
        /// </summary>
        public Vector3 RayOrigin { get; private set; }
        /// <summary>
        /// The global end point of the raycast.
        /// </summary>
        public Vector3 RayTarget { get; private set; }
        /// <summary>
        /// The vector from the ray origin to the ray target.
        /// </summary>
        public Vector3 RayVector => RayTarget - RayOrigin;
        /// <summary>
        /// The direction from the ray origin to the ray target.
        /// </summary>
        public Vector3 RayDirection => RayVector.Normalized();
        /// <summary>
        /// The length of the raycast.
        /// </summary>
        public float RayDistance => RayVector.Length();

        /// <summary>
        /// Whether or not the ray hit something.
        /// </summary>
        public bool HasHit => Collider != null;
        /// <summary>
        /// The collider that was hit.
        /// </summary>
        public CollisionObject3D Collider { get; set; }
        /// <summary>
        /// The position where the ray hit a collider. If there was no hit, this is equal to the ray target.
        /// </summary>
        public Vector3 HitPosition { get; set; }
        /// <summary>
        /// The surface normal of the collision point.
        /// </summary>
        public Vector3 HitNormal { get; set; }
        /// <summary>
        /// A vector from the ray origin to the hit position.
        /// </summary>
        public Vector3 HitVector => HitPosition - RayOrigin;
        /// <summary>
        /// The distance between the ray origin and the hit position.
        /// </summary>
        public float HitDistance => HitVector.Length();
        public float HitDistanceX => Mathf.Abs(HitVector.X);
        public float HitDistanceY => Mathf.Abs(HitVector.Y);

        /* Constructors. */
        public RaycastResult(Vector3 rayOrigin, Vector3 rayTarget)
            : this(rayOrigin, rayTarget, null, rayTarget, Vector3.Zero)
        { }

        public RaycastResult(Vector3 rayOrigin, Vector3 rayTarget, CollisionObject3D collider, Vector3 hitPosition, Vector3 hitNormal) : this()
        {
            RayOrigin = rayOrigin;
            RayTarget = rayTarget;
            Collider = collider;
            HitPosition = hitPosition;
            HitNormal = hitNormal;

            if (!HasHit)
                HitPosition = RayTarget;
        }

        /* Operators. */
        /// <summary>
        /// Returns RaycastResult.HasHit.
        /// </summary>
        public static implicit operator bool(RaycastResult result)
        {
            return result.HasHit;
        }

        /* Public methods. */
        public override string ToString()
        {
            string str = RayOrigin + " -> " + RayTarget + ": " + HitVector;

            if (HasHit)
                str += Collider.Name + " at " + HitPosition + ", distance = " + HitDistance + ", normal = " + HitNormal;
            else
                str += "null";

            return str;
        }
    }
}