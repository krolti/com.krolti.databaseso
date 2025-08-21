using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


#if UNIRX

using UniRx;

#endif


#if UNITASK

using Cysharp.Threading.Tasks;

#endif


#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Krolti.DatabaseSO
{
    /// <summary>
    /// Base class for the Database.
    /// </summary>
    /// <typeparam name="T">Type of database items, must implement IDatabaseItem interface.</typeparam>
    public abstract class Database<T> : ScriptableObject, IDatabase<T>

        where T : class, IDatabaseItem
    {
        [SerializeField, TextArea(3, 10)] protected string comment;
        [Tooltip("The underlying list that stores all database items")]
        [SerializeField] protected List<T> Data = new List<T>();

        private readonly object _initLock = new();
        private readonly object _searchLock = new();


        [field: NonSerialized] public bool IsInit { get; private set; } = false;
        public int Count => Data.Count;


        public IReadOnlyList<T> SafeData
        {
            get
            {
                if (!IsInit) Initialize();

                return new ReadOnlyCollection<T>(Data);
            }
        }


        public IReadOnlyList<T> GetAll()
        {
            return SafeData;
        }

        public DatabaseQuery<T> Query()
        {
            if (!IsInit) Initialize();

            return new DatabaseQuery<T>(this);
        }



#if UNIRX

        private Subject<Unit> _onInitialized = new Subject<Unit>();
        private Subject<Unit> _onDirty = new Subject<Unit>();


        /// <summary>
        /// Observable that triggers when the database finishes initialization.
        /// </summary>
        public IObservable<Unit> OnInitialized => _onInitialized;


        /// <summary>
        /// Observable that triggers when the database is marked as dirty.
        /// </summary>
        public IObservable<Unit> OnDirty => _onDirty;
#endif




        protected virtual void OnEnable()
        {
            AttributeUtility.CheckDatabaseTypes<T>();
        }

        protected virtual void OnBeforeInitialize() { }

        protected virtual void OnAfterInitialize() { }



        protected internal virtual void Initialize()
        {
            if (IsInit) return;

            OnBeforeInitialize();
            //
            // Be aware that this code is not fully thread safe.
            // If you're calling this from other threads consider
            // to exclude unity objects in IFixable and IsValid logic.
            //

            lock (_initLock)
            {
                if (IsInit) return;


                bool shouldBeReassigned = false;

                HashSet<int> uniqueIDs = new();

                for (int i = 0; i < Count; i++)
                {
                    var data = Data[i];

                    if (!uniqueIDs.Add(data.ID))
                    {
                        throw DebugDB.Exception<T>("Non unique element found in database.", data);
                    }

                    ValidateDatabaseItem(data);

                }

                if (shouldBeReassigned)
                {
                    AssignUniqueIDs();
                }

                if (!IsSortedByID())
                {
                    Data.Sort();
                }
            }
            IsInit = true;

#if UNIRX
            _onInitialized.OnNext(Unit.Default);
#endif
            OnAfterInitialize();
        }



        private static bool ValidateDatabaseItem(T data)
        {
            if (data.IsValid)
            {
                return true;
            }


            DebugDB.Warn("Invalid element found in database.", data);

            if (data is IFixable fixable)
            {
                if (fixable.TryFix())
                {
                    DebugDB.Log("Fixed element in database.", data);
                    return true;
                }
                else
                {
                    DebugDB.Error("Database element corrupted in database!", data);
                }
            }
            else
            {
                throw DebugDB.Exception<T>("Database element is not IFixable in database. Use it to fix corrupted database items.",
                    data);
            }

            return false;
        }



        protected virtual void OnDisable()
        {
#if UNIRX

            _onInitialized?.OnCompleted();
            _onInitialized?.Dispose();
            _onInitialized = null;

            _onDirty?.OnCompleted();
            _onDirty?.Dispose();
            _onDirty = null;

#endif
        }




        /// <summary>
        /// Force Database to reinitialize.
        /// </summary>
        public virtual void SetDatabaseDirty()
        {
            IsInit = false;

#if UNIRX
            _onDirty.OnNext(Unit.Default);
#endif
        }



        /// <summary>
        /// Get random item in the database.
        /// </summary>
        /// <returns></returns>
        public T GetRandomItem()
        {
            if (Data.Count == 0) return default;
            return Data[UnityEngine.Random.Range(0, Data.Count)];
        }



        /// <summary>
        /// Quick and memory efficient searching method.
        /// 
        /// <para>Time complexity: O(logN), may be worse if database is not initialized.</para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warningIfNotFound">Create unity log warning if element was not found.
        /// <para>WARNING! Use only if param cannot be null for the gameplay and operations count is less than 1000
        /// per nullable item.</para></param>
        /// <returns>Database T type.</returns>
        /// <exception cref="IndexOutOfRangeException">throws if index is less than 0.</exception>
        public T Search(int id, bool warningIfNotFound = true)
        {
            if (id < 0)
            {
                throw new IndexOutOfRangeException(nameof(id));
            }


            if (!IsInit) Initialize();

            lock (_searchLock)
            {
                T item;
                item = BinarySearch(id);
                if (item != null)
                {
                    return item;
                }
                if (warningIfNotFound)
                {
                    DebugDB.Warn<T>($"Element was not found, target: {id}");
                }
            }

            return null;
        }



        /// <summary>
        /// Safe way to search the item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns>If found</returns>
        public bool TrySearch(int id, out T value)
        {
            value = null;

            if (id < 0)
                return false;

            if (!IsInit) Initialize();

            lock (_searchLock)
            {

                T item = BinarySearch(id);

                if (item != null)
                {
                    value = item;
                    return true;
                }

            }

            return false;
        }


        /// <summary>
        /// Efficient contains method that uses binary search to check for containing item.
        /// 
        /// <para>Time complexity: O(logN), may be worse if database is not initialized.</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsID(int id)
        {
            if (!IsInit) Initialize();
            return BinarySearch(id) != null;
        }



        private T BinarySearch(int id)
        {
            int left = 0;
            int right = Data.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var midID = Data[mid].ID;

                if (midID == id)
                {
                    return Data[mid];
                }
                else if (midID < id)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }


            return null;
        }



        private bool IsSortedByID()
        {
            for (int i = 0; i < Data.Count - 1; i++)
            {
                if (Data[i].ID > Data[i + 1].ID)
                {
                    return false;
                }
            }
            return true;
        }



        protected virtual bool AssignUniqueIDs()
        {
            bool hadChanges = false;
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i].ID == i)
                {
                    continue;
                }


                hadChanges = true;

                if (Data[i] is IRewritableItem rewritable)
                {
                    rewritable.OverwriteID(i);
                }
                else
                {
                    throw DebugDB.Exception<T>("Item is not rewritable. Consider making Item rewritable or database have custom unique check logic");
                }

            }

            return hadChanges;
        }

        protected virtual void OnBeforeJsonExport() { }

        protected virtual void OnAfterJsonExport() { }


        public async Task<string> ConvertToJsonAsync(bool prettyPrint = false, CancellationToken cancellationToken = default, bool warnEmpty = true)
        {
            if (!IsInit) Initialize();

            OnBeforeJsonExport();

            List<T> dataCopy;

            lock (Data)
            {
                dataCopy = new List<T>(Data);
            }

            if (dataCopy.Count == 0 && warnEmpty)
            {
                DebugDB.Warn<T>("Data count is 0 may not properly convert to json. If it's okay set warnEmpty as false");
            }


            string json = await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var wrapper = new DatabaseWrapper<T>(dataCopy);
                return JsonUtility.ToJson(wrapper, prettyPrint);
            }, cancellationToken);

            OnAfterJsonExport();

            return json;
        }



        public string ConvertToJson(bool prettyPrint = false, bool warnEmpty = true)
        {
            if (!IsInit) Initialize();

            OnBeforeJsonExport();

            if(Data.Count == 0 && warnEmpty)
            {
                DebugDB.Warn<T>("Data count is 0 may not properly convert to json. If it's okay set warnEmpty as false");
            }

            string json = JsonConverter.ConvertToJson(Data, prettyPrint);

            OnAfterJsonExport();

            return json;
        }



        public void ImportFromJSON(string json)
        {
            JsonConverter.ImportFromJSON(json, ref Data);
            SetDatabaseDirty();
        }



#if UNITASK


        public async UniTask<string> ConvertToJsonUniTaskAsync(bool prettyPrint = false, CancellationToken cancellationToken = default)
        {
            if (!IsInit) Initialize();

            OnBeforeJsonExport();

            List<T> dataCopy;

            lock (Data)
            {
                dataCopy = new List<T>(Data);
            }

            if (dataCopy.Count == 0)
            {
                DebugDB.Warn<T>("Data count is 0 may not properly convert to json.");
            }

            string json = await UniTask.RunOnThreadPool(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var wrapper = new DatabaseWrapper<T>(dataCopy);
                return JsonUtility.ToJson(wrapper, prettyPrint);
            }, cancellationToken: cancellationToken);

            OnAfterJsonExport();

            return json;
        }


#endif





#if UNITY_EDITOR


        /// <summary>
        /// Use this to search database elements in the editor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T SearchInEditor(int id)
        {
            int count = Data.Count;
            for (int i = 0; i < count; i++)
            {
                if (Data[i].ID == id)
                    return Data[i];

            }

            return null;
        }



        private void OnValidate()
        {
            int removed = Data.RemoveAll(item => item == null);
            bool sorted = false;

            bool hadChanges = AssignUniqueIDs();

            if (!IsSortedByID())
            {
                Data.Sort((a, b) => a.ID.CompareTo(b.ID));
                sorted = true;
            }


            if (removed > 0 || sorted || hadChanges)
            {
                EditorUtility.SetDirty(this);
                SetDatabaseDirty();
            }

        }



#endif


    }

}
