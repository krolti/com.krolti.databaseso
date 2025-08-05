
namespace Krolti.DatabaseSO
{
    /// <summary>
    /// Interface representing an item that can be stored in a database.
    /// </summary>
    public interface IDatabaseItem
    {
        /// <summary>
        /// Gets the unique identifier for this database element.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets a value indicating whether this database element is valid and properly configured.
        /// </summary>
        bool IsValid { get; }
    }
}