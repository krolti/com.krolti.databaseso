using System;
using System.Collections.Generic;
using System.Linq;

namespace Krolti.DatabaseSO
{
    public sealed class DatabaseQuery<T> where T : class, IDatabaseItem
    {
        private readonly Database<T> _database;
        private IEnumerable<T> _currentSet;

        internal DatabaseQuery(Database<T> database)
        {
            _database = database;
            if (!_database.IsInit) _database.Initialize();
            _currentSet = _database.GetAll();
        }

        public DatabaseQuery<T> Where(Func<T, bool> predicate)
        {
            _currentSet = _currentSet.Where(predicate);
            return this;
        }

        public DatabaseQuery<T> OrderBy<TKey>(Func<T, TKey> keySelector)
        {
            _currentSet = _currentSet.OrderBy(keySelector);
            return this;
        }

        public DatabaseQuery<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            _currentSet = _currentSet.OrderByDescending(keySelector);
            return this;
        }

        public DatabaseQuery<T> Skip(int count)
        {
            _currentSet = _currentSet.Skip(count);
            return this;
        }

        public DatabaseQuery<T> Take(int count)
        {
            _currentSet = _currentSet.Take(count);
            return this;
        }

        public List<T> ToList() => _currentSet.ToList();
        public T First() => _currentSet.First();
        public T FirstOrDefault() => _currentSet.FirstOrDefault();
        public int Count() => _currentSet.Count();
        public bool Any() => _currentSet.Any();
        public bool Any(Func<T, bool> predicate) => _currentSet.Any(predicate);
    }
}
