
namespace Krolti.DatabaseSO
{
    public interface ITagRepository<T>
    {
        /// <summary>
        /// Get data in repository by item tag.
        /// </summary>
        /// <param name="itemTag">key for the repository.</param>
        /// <param name="safeMode"></param>
        /// <returns></returns>
        T GetByTag(string itemTag, bool safeMode = true);

        /// <summary>
        /// Get data in repository by item tag.
        /// </summary>
        /// <param name="itemTag">key for the repository.</param>
        /// <param name="value"></param>
        /// <param name="safeMode">desides whether throw exception or not.</param>
        /// <returns>If item is found</returns>
        bool TryGetValue(string itemTag, out T value, bool safeMode = true);

        /// <summary>
        /// Checks if tag exists in repository.
        /// </summary>
        /// <param name="itemTag">key for the repository.</param>
        /// <returns>If tag is found</returns>
        bool ContainsTag(string itemTag);
    }
}
