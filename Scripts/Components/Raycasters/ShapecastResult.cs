using Godot;
using System;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A raycast result with skin width applied. Contains the original raw raycast result without the skin width applied in case
    /// this is necessary.
    /// </summary>
    [Serializable]
    public struct ShapecastResult
    {
        /* Public properties. */
        public static ShapecastResult None => new ShapecastResult();

        public RaycastResult Raw { get; private set; }
        public float SkinWidth { get; private set; }

        /// <summary>
        /// The global origin of the raycast (with skin width applied).
        /// </summary>
        public Vector3 RayOrigin => Raw.RayOrigin + Raw.RayDirection * SkinWidth;
        /// <summary>
        /// The global end point of the raycast.
        /// </summary>
        public Vector3 RayTarget => Raw.RayTarget;
        /// <summary>
        /// The vector from the ray origin (with skin width applied) to the ray target.
        /// </summary>
        public Vector3 RayVector => Raw.RayVector;
        /// <summary>
        /// The direction from the ray origin to the ray target.
        /// </summary>
        public Vector3 RayDirection => Raw.RayDirection;
        /// <summary>
        /// The length of the raycast (with skin width applied).
        /// </summary>
        public float RayDistance => Mathf.Max(Raw.RayDistance - SkinWidth, 0f);

        /// <summary>
        /// Whether or not the ray hit something.
        /// </summary>
        public bool HasHit => Raw.HasHit;
        /// <summary>
        /// The collider that was hit.
        /// </summary>
        public CollisionObject3D Collider => Raw.Collider;
        /// <summary>
        /// The position where the ray hit a collider. If there was no hit, this is equal to the ray target.
        /// </summary>
        public Vector3 HitPosition => Raw.HitPosition;
        /// <summary>
        /// The surface normal of the collision point.
        /// </summary>
        public Vector3 HitNormal => Raw.HitNormal;
        /// <summary>
        /// The vector from the ray origin (with skin width applied) to the hit position.
        /// </summary>
        public Vector3 HitVector => HitPosition - RayOrigin;
        /// <summary>
        /// The distance between the ray origin (with skin width applied) and the hit position.
        /// </summary>
        public float HitDistance => Mathf.Max(Raw.HitDistance - SkinWidth, 0f);

        /* Constructors. */
        public ShapecastResult(RaycastResult raycastResult, float skinWidth) : this()
        {
            Raw = raycastResult;
            SkinWidth = skinWidth;
        }

        /* Operators. */
        /// <summary>
        /// Returns RaycastResult.HasHit.
        /// </summary>
        public static implicit operator bool(ShapecastResult result)
        {
            return result.HasHit;
        }

        /* Public methods. */
        public override string ToString()
        {
            return Raw.ToString() + " skinWidth = " + SkinWidth;
        }
    }
}