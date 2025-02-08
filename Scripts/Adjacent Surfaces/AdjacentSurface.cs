using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// Contains information about a surface adjacent to a character.
    /// </summary>
    public struct AdjacentSurface
    {
        /* Public properties. */
        /// <summary>
        /// A surface that represents empty air.
        /// </summary>
        public static AdjacentSurface Nothing => new AdjacentSurface
        {
            Normal = Vector2.Zero,
            Angle = 0f,
            Type = SurfaceType.Air
        };

        // Main properties.
        /// <summary>
        /// A vector that is perpendicular to the surface. It points away from the surface.
        /// </summary>
        public Vector2 Normal { get; private set; }
        /// <summary>
        /// The angle of the surface.
        /// </summary>
        public float Angle { get; private set; }
        /// <summary>
        /// The type of the surface.
        /// </summary>
        public SurfaceType Type { get; private set; }

        // Tangents.
        /// <summary>
        /// A vector that points parallel to the surface, in the right direction (relative to the surface normal).
        /// </summary>
        public Vector2 TangentRight => new Vector2(Normal.Y, -Normal.X);
        /// <summary>
        /// A vector that points parallel to the surface, in the left direction (relative to the surface normal).
        /// </summary>
        public Vector2 TangentLeft => new Vector2(-Normal.Y, Normal.X);

        /// <summary>
        /// A vector that points parallel to the surface, in the left direction. Returns (0, 0) if the surface is air or is vertical.
        /// </summary>
        public Vector2 ParallelLeft
        {
            get
            {
                switch (Type)
                {
                    case SurfaceType.Air:
                    case SurfaceType.StraightWall:
                        return Vector2.Zero;
                    default:
                        if (FacesUp)
                            return TangentLeft;
                        else
                            return TangentRight;
                }
            }
        }
        /// <summary>
        /// A vector that points parallel to the surface, in the right direction. Returns (0, 0) if the surface is air or is vertical.
        /// </summary>
        public Vector2 ParallelRight
        {
            get
            {
                switch (Type)
                {
                    case SurfaceType.Air:
                    case SurfaceType.StraightWall:
                        return Vector2.Zero;
                    default:
                        if (FacesUp)
                            return TangentRight;
                        else
                            return TangentLeft;
                }
            }
        }
        /// <summary>
        /// A vector that points parallel to the surface, in the direction of the ground. Returns (0, 0) if the surface is air or
        /// is horizontal.
        /// </summary>
        public Vector2 ParallelDown
        {
            get
            {
                switch (Type)
                {
                    case SurfaceType.SlopedGround:
                    case SurfaceType.SteepGround:
                    case SurfaceType.SlopedWallDownwards:
                    case SurfaceType.StraightWall:
                    case SurfaceType.SlopedWallUpwards:
                    case SurfaceType.SteepCeiling:
                    case SurfaceType.SlopedCeiling:
                        if (FacesRight)
                            return TangentRight;
                        else
                            return TangentLeft;
                    default:
                        return Vector2.Zero;
                }
            }
        }
        /// <summary>
        /// A vector that points parallel to the surface, in the direction of the sky. Returns (0, 0) if the surface is air or is
        /// horizontal.
        /// </summary>
        public Vector2 ParallelUp
        {
            get
            {
                switch (Type)
                {
                    case SurfaceType.SlopedGround:
                    case SurfaceType.SteepGround:
                    case SurfaceType.SlopedWallDownwards:
                    case SurfaceType.StraightWall:
                    case SurfaceType.SlopedWallUpwards:
                    case SurfaceType.SteepCeiling:
                    case SurfaceType.SlopedCeiling:
                        if (FacesRight)
                            return TangentLeft;
                        else
                            return TangentRight;
                    default:
                        return Vector2.Zero;
                }
            }
        }

        // Facing direction.
        /// <summary>
        /// Whether or not the surface normal points left.
        /// </summary>
        public bool FacesLeft => Normal.X < 0f;
        /// <summary>
        /// Whether or not the surface normal points right.
        /// </summary>
        public bool FacesRight => Normal.X > 0f;
        /// <summary>
        /// Whether or not the surface normal points down.
        /// </summary>
        public bool FacesDown => Normal.Y < 0f;
        /// <summary>
        /// Whether or not the surface normal points up.
        /// </summary>
        public bool FacesUp => Normal.Y > 0f;

        // Type shorthands.
        /// <summary>
        /// Whether or not the surface is empty air.
        /// </summary>
        public bool IsAir => Type == SurfaceType.Air;

        /// <summary>
        /// Whether or not the surface is a type of ground.
        /// </summary>
        public bool IsGround => IsLevelGround || IsSlopedGround || IsSteepGround;
        /// <summary>
        /// Whether or not the surface is perfectly horizontal ground.
        /// </summary>
        public bool IsLevelGround => Type == SurfaceType.LevelGround;
        /// <summary>
        /// Whether or not the surface is sloped ground.
        /// </summary>
        public bool IsSlopedGround => Type == SurfaceType.SlopedGround;
        /// <summary>
        /// Whether or not the surface is steep ground (ground with an anle higher than sloped ground).
        /// </summary>
        public bool IsSteepGround => Type == SurfaceType.SteepGround;

        /// <summary>
        /// Whether or not the surface is a type of wall.
        /// </summary>
        public bool IsWall => IsDownwardsSlopedWall || IsStraightWall || IsUpwardsSlopedWall;
        /// <summary>
        /// Whether or not the surface is a downwards sloped wall (a wall with an angle lower than a straight wall).
        /// </summary>
        public bool IsDownwardsSlopedWall => Type == SurfaceType.SlopedWallDownwards;
        /// <summary>
        /// Whether or not the surface is a perfectly vertical wall.
        /// </summary>
        public bool IsStraightWall => Type == SurfaceType.StraightWall;
        /// <summary>
        /// Whether or not the surface is a upwards sloped wall (a wall with an angle higher than a straight wall).
        /// </summary>
        public bool IsUpwardsSlopedWall => Type == SurfaceType.SlopedWallUpwards;

        /// <summary>
        /// Whether or not the surface is a type of ceiling.
        /// </summary>
        public bool IsCeiling => IsSteepCeiling || IsSlopedCeiling || IsLevelCeiling;
        /// <summary>
        /// Whether or not the surface is a steep ceiling (a ceiling with an angle lower than a sloped ceiling).
        /// </summary>
        public bool IsSteepCeiling => Type == SurfaceType.SteepCeiling;
        /// <summary>
        /// Whether or not the surface is a sloped ceiling.
        /// </summary>
        public bool IsSlopedCeiling => Type == SurfaceType.SlopedCeiling;
        /// <summary>
        /// Whether or not the surface is a perfectly horizontal ceiling.
        /// </summary>
        public bool IsLevelCeiling => Type == SurfaceType.LevelCeiling;

        /// <summary>
        /// Whether or not the surface is the inside of a solid.
        /// </summary>
        public bool IsInside => Type == SurfaceType.Inside;

        /// <summary>
        /// Whether or not the surface is perfectly horizontal.
        /// </summary>
        public bool IsHorizontal => IsLevelGround || IsLevelCeiling;
        /// <summary>
        /// Whether or not the surface is perfectly vertical.
        /// </summary>
        public bool IsVertical => IsStraightWall;
        /// <summary>
        /// Whether or not the surface is neither perfectly horizontal nor perfectly vertical.
        /// </summary>
        public bool IsSlanted => IsSlopedGround || IsSteepGround
            || IsDownwardsSlopedWall || IsUpwardsSlopedWall
            || IsSteepCeiling || IsSlopedCeiling;

        /* Constructors. */
        public AdjacentSurface(bool isAir, Vector2 normal, Vector2 up, float maxSlopeAngle, float maxGroundAngle,
            float maxCeilingSlopeAngle, float maxCeilingAngle) : this()
        {
            if (isAir)
            {
                // Store normal.
                Normal = normal;

                // Calculate angle of the surface, relative to vector representing the local up direction.
                Angle = NormalToAngle(normal, up);

                // Determine surface type.
                if (normal == Vector2.Zero)
                    Type = SurfaceType.Inside;
                else
                    Type = AngleToType(Angle, maxSlopeAngle, maxGroundAngle, maxCeilingSlopeAngle, maxCeilingAngle);
            }
            else
            {
                Normal = Vector2.Zero;
                Angle = 0f;
                Type = SurfaceType.Air;
            }
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{Type} ({Angle}\u00B0, normal = {Normal})";
        }

        /// <summary>
        /// Convert a normal vector to an angle (in degrees).
        /// </summary>
        public static float NormalToAngle(Vector2 normal, Vector2 up)
        {
            if (normal == Vector2.Zero)
                return 0f;
            else
                return Round(Mathf.Abs(Mathf.RadToDeg(normal.AngleTo(up))));
        }

        /// <summary>
        /// Convert a surface angle to a surface type. The angle should be between 0 and 180 degrees.
        /// </summary>
        public static SurfaceType AngleToType(float angle, float maxSlopeAngle, float maxGroundAngle,
            float maxCeilingSlopeAngle, float maxCeilingAngle)
        {
            // Ground.
            if (angle <= maxGroundAngle)
            {
                if (angle == 0f)
                    return SurfaceType.LevelGround;
                else if (angle <= maxSlopeAngle)
                    return SurfaceType.SlopedGround;
                else
                    return SurfaceType.SteepGround;
            }

            // Wall types.
            else if (angle < 180f - maxCeilingAngle)
            {
                if (angle == 90f)
                    return SurfaceType.StraightWall;
                else if (angle < 90f)
                    return SurfaceType.SlopedWallDownwards;
                else
                    return SurfaceType.SlopedWallUpwards;
            }

            // Ceiling types.
            else
            {
                if (angle == 180f)
                    return SurfaceType.LevelCeiling;
                else if (angle >= 180f - maxCeilingSlopeAngle)
                    return SurfaceType.SlopedCeiling;
                else
                    return SurfaceType.SteepCeiling;
            }
        }

        /* Private methods. */
        /// <summary>
        /// Round a number after n + 1 fractional decimals, then discard the n + 1th fractional decimal. Used to avoid
        /// floating-point imprecision jank.
        /// </summary>
        private static float Round(float number, int decimals = 3)
        {
            // Conversion to int factor.
            float factor = Mathf.Pow(10, decimals + 1);

            // Convert integer part and n + 1 fractional decimals to int.
            int digits = Mathf.RoundToInt(number * factor);

            // Discard n + 1th decimal.
            digits /= 10;

            // Convert back to float.
            return digits / (factor / 10f);
        }
    }
}