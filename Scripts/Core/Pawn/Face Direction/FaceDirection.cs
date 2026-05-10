using System;

namespace Rusty.Pawns;

/// <summary>
/// Represents a pawn's two-dimensional face direction.
/// </summary>
public struct FaceDirection
{
    /* Public properties. */
    /// <summary>
    /// A face direction that represents no change.
    /// </summary>
    public static FaceDirection None => new FaceDirection(FaceDirectionX.NoChange, FaceDirectionY.NoChange);

    /// <summary>
    /// The x component of the face direction.
    /// </summary>
    public FaceDirectionX X { get; set; }
    /// <summary>
    /// The y component of the face direction.
    /// </summary>
    public FaceDirectionY Y { get; set; }

    /* Constructors. */
    public FaceDirection(FaceDirectionX x, FaceDirectionY y) : this()
    {
        X = x;
        Y = y;
    }

    /* Comparison operators. */
    public static bool operator ==(FaceDirection a, FaceDirection b) => a.Equals(b);
    public static bool operator !=(FaceDirection a, FaceDirection b) => !a.Equals(b);

    /* Public methods. */
    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
    public override bool Equals(object obj) => obj is FaceDirection other && Equals(other);
    public bool Equals(FaceDirection obj) => X == obj.X && Y == obj.Y;
}