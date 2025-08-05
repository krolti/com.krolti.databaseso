using System.Collections.Generic;

namespace Krolti.DatabaseSO
{

    /// <summary>
    /// Database main interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDatabase<T>
    {
        int Count { get; }

        IReadOnlyList<T> GetAll();

        T Search(int id, bool warningIfNotFound = true);

        bool TrySearch(int id, out T value);

        bool ContainsID(int id);

    }
}
