using System;
using System.Collections.Generic;

namespace Krolti.DatabaseSO
{
    /// <summary>
    /// A serializable container class that wraps a list of database items.
    /// Used for storing and transporting collections of database elements.
    /// </summary>
    /// <typeparam name="U">The type of items stored in the database wrapper.</typeparam>
    [Serializable]
    public sealed class DatabaseWrapper<U> where U : class, IDatabaseItem
    {
        public List<U> Items;
        public DatabaseWrapper(List<U> data) => Items = data;
    }
}