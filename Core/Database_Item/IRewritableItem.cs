

namespace Krolti.DatabaseSO
{
    /// <summary>
    /// Interface for database items that support ID modification after creation.
    /// </summary>
    public interface IRewritableItem
    {
        /// <summary>
        /// Replaces the current ID of the item with a new specified identifier.
        /// </summary>
        /// <param name="id">The new unique identifier to assign to this item.</param>
        void OverwriteID(int id);
    }
}