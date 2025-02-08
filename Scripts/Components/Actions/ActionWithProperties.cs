namespace Modules.L2.Pawns
{
    /// <summary>
    /// Intermediate class that all action types derive from. Includes basic properties functionality.
    /// </summary>
    public abstract partial class ActionWithProperties<T> : Action where T : ActionProperties
    {
        /* Public properties. */
        public T CurrentProperties { get; set; }

        /* Public methods. */
        public override void UpdateProperties(double deltaTime, Pawn pawn)
        {
            CurrentProperties = GetActiveProperties(pawn);
        }

        public sealed override ActionProperties GetProperties()
        {
            return CurrentProperties;
        }

        /* Protected methods. */
        /// <summary>
        /// Get the first property set that has all of its conditions met.
        /// </summary>
        protected T GetActiveProperties(Pawn pawn)
        {
            foreach (PawnComponent child in Children)
            {
                if (child is T properties && properties.CheckActive(pawn))
                    return properties;
            }
            return null;
        }
    }
}