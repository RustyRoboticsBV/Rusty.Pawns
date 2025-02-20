namespace Rusty.Pawns
{
    /// <summary>
    /// Represents a pawn's two-dimensional face direction.
    /// </summary>
    public struct FaceDirection
    {
        /* Public properties. */
        public static FaceDirection None => new FaceDirection(FaceDirectionX.NoChange, FaceDirectionY.NoChange);

        public FaceDirectionX X { get; set; }
        public FaceDirectionY Y { get; set; }

        /* Constructors. */
        public FaceDirection(FaceDirectionX x, FaceDirectionY y) : this()
        {
            X = x;
            Y = y;
        }
    }
}