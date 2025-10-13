namespace Rusty.Pawns;

/// <summary>
/// An interface for actions that maintain a set of properties.
/// </summary>
public interface IActionWithProperties
{
    /* Public methods. */
    /// <summary>
    /// Select a set of action properties to use for the next update loop, depending on the current context.
    /// </summary>
    public void UpdateProperties(double deltaTime, Pawn pawn);

    /// <summary>
    /// Get the current action properties.
    /// </summary>
    public ActionProperties GetProperties();
}