using System;

namespace Rusty.Pawns
{
    /// <summary>
    /// Contains information about the surface that's nearest to a pawn in some direction.
    /// </summary>
    [Serializable]
    public struct NearestSurface
    {
        /* Public properties. */
        /// <summary>
        /// A surface that represents empty air.
        /// </summary>
        public static NearestSurface Nothing => new NearestSurface
        {
            Surface = AdjacentSurface.Nothing,
            Distance = 0f,
            IsAdjacent = false
        };

        public AdjacentSurface Surface { get; set; }
        public float Distance { get; set; }
        public bool IsAdjacent { get; set; }

        /* Constructors. */
        public NearestSurface(AdjacentSurface surface, float distance, float adjacencyDistance) : this()
        {
            Surface = surface;
            Distance = distance;
            IsAdjacent = Distance <= adjacencyDistance;
        }

        /* Public methods. */
        public override string ToString()
        {
            if (IsAdjacent)
                return $"{Surface} at {Distance} (adjacent)";
            else
                return $"{Surface} at {Distance} (distant)";
        }
    }
}