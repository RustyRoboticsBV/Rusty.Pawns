using System;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// Represents the type of an adjacent surface.
    /// </summary>
    [Flags]
    public enum SurfaceType
    {
        // Nothing.
        Air = 1,

        // Ground.
        LevelGround = 1 << 1,
        SlopedGround = 1 << 2,
        SteepGround = 1 << 3,

        // Wall.
        SlopedWallDownwards = 1 << 4,
        StraightWall = 1 << 5,
        SlopedWallUpwards = 1 << 6,

        // Ceiling.
        SteepCeiling = 1 << 7,
        SlopedCeiling = 1 << 8,
        LevelCeiling = 1 << 9,

        // Inside of solid.
        Inside = 1 << 10
    }
}