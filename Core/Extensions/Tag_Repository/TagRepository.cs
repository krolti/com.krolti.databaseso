using System;
using System.Collections.Concurrent;
using UnityEngine;



#if UNIRX
using UniRx;
#endif

namespace Krolti.DatabaseSO
{
    /// <summary>
    /// Base class for the tag-indexed database. (supports int id and string tag)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TagRepository<T> : Database<T>, ITagRepository<T>

        where T : class, ITagData, IDatabaseItem
    {
        private const uint MaxTagLength = 1000;
        /// <summary>
        /// 0 - length. 1 - max length.
        /// </summary>
        private const string LengthExceptionMessage = "Requested item tag exceeded max length: (current length - '{0}'). " +
            "Change the tagMaxLength if you are aware that the tag's hash code will be much harder to get " +
            "and type will overuse memory. Current max length: {1}";

        public const string EmptyTagMessage = "Requested null or empty tag in the tag repository";

        [Header("Leave on 0 to get default max length (1000)")]
        [Space(8)]
        [Tooltip("Default length: 1000")]
        [SerializeField] private uint tagMaxLength = 0;

        private ConcurrentDictionary<string, T> _tagToItem;

        private uint MaxLengthComparer => tagMaxLength != 0 ? tagMaxLength : MaxTagLength;

        public bool IsRepositoryInit { get; private set; } = false;

#if UNIRX

        private Subject<Unit> _onRepositoryInit = new Subject<Unit>();
        private Subject<Unit> _onRepositoryDirty = new Subject<Unit>();


        /// <summary>
        /// Observable that triggers when the database finishes initialization.
        /// </summary>
        public IObservable<Unit> OnRepositoryInit => _onRepositoryInit;


        /// <summary>
        /// Observable that triggers when the database is marked as dirty.
        /// </summary>
        public IObservable<Unit> OnRepositoryDirty => _onRepositoryDirty;
#endif


        protected override void OnEnable()
        {
            base.OnEnable();
            AttributeUtility.CheckDatabaseTypes<T>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

#if UNIRX
            _onRepositoryInit.Dispose();
            _onRepositoryDirty.Dispose();

            _onRepositoryInit = null;
            _onRepositoryDirty = null;
#endif
        }



        protected override void OnAfterInitialize()
        {
            InitializeRepository();
        }



        public T GetByTag(string itemTag, bool safeMode = true)
        {
            if (string.IsNullOrEmpty(itemTag))
            {
                if (safeMode)
                {
                    DebugDB.Error<T>(EmptyTagMessage);
                    return null;
                }
                else
                {
                    throw DebugDB.Exception<T>(EmptyTagMessage);
                }

            }


            if (itemTag.Length > MaxLengthComparer)
            {
                if (safeMode)
                {
                    DebugDB.WarnFormat<T>(LengthExceptionMessage,
                        itemTag.Length.ToString(), MaxLengthComparer.ToString());
                    return null;
                }
                else
                {
                    throw DebugDB.ExceptionFormat<T>(LengthExceptionMessage, itemTag.Length, MaxLengthComparer);
                }

            }


            if (!IsRepositoryInit || _tagToItem == null) Initialize();


            if (_tagToItem.TryGetValue(itemTag, out T value))
            {
                return value;
            }

            return null;
        }



        public bool TryGetValue(string itemTag, out T value, bool safeMode = true)
        {
            value = GetByTag(itemTag, safeMode);

            if (value != null)
            {
                return true;
            }

            value = null;
            return false;
        }




        public bool ContainsTag(string itemTag)
        {
            if (string.IsNullOrEmpty(itemTag))
            {
                return false;
            }

            if (!IsInit || !IsRepositoryInit || _tagToItem == null) Initialize();

            return _tagToItem.ContainsKey(itemTag);
        }



        /// <summary>
        /// Same as contains tag but throws if the tag is empty
        /// </summary>
        /// <param name="itemTag"></param>
        /// <returns></returns>
        /// <exception cref="EmptyTagException"></exception>
        public bool ContainsTagThrowIfEmpty(string itemTag)
        {
            if (string.IsNullOrEmpty(itemTag))
            {
                throw DebugDB.Exception<T>(EmptyTagMessage);
            }

            return ContainsTag(itemTag);
        }



        private void InitializeRepository()
        {
            if (_tagToItem == null)
            {
                _tagToItem = new ConcurrentDictionary<string, T>
                    (Environment.ProcessorCount, Count);
            }


            _tagToItem.Clear();


            foreach (var item in Data)
            {

                if (_tagToItem.ContainsKey(item.ItemTag))
                {
                    throw DebugDB.Exception<T>($"Found duplicates in the tag repository. Tag: {item.ItemTag}");
                }


                if (string.IsNullOrEmpty(item.ItemTag))
                {
                    throw DebugDB.Exception<T>(EmptyTagMessage, item);
                }


                if (item.ItemTag.Length > MaxLengthComparer)
                {
                    throw DebugDB.ExceptionFormat<T>(LengthExceptionMessage,
                        item.ItemTag.Length, MaxLengthComparer);
                }


                _tagToItem[item.ItemTag] = item;
            }

            IsRepositoryInit = true;

#if UNIRX
            _onRepositoryInit.OnNext(Unit.Default);
#endif
        }

        public override void SetDatabaseDirty()
        {
            base.SetDatabaseDirty();
            IsRepositoryInit = false;

#if UNIRX
            _onRepositoryDirty.OnNext(Unit.Default);
#endif
        }


#if UNITY_EDITOR


        public T GetByTagInEditor(string itemTag)
        {
            if (string.IsNullOrEmpty(itemTag))
                DebugDB.Error<T>(EmptyTagMessage);


            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i].ItemTag == itemTag)
                {
                    return Data[i];
                }
            }

            return null;
        }



        public bool TryGetValueEditor(string itemTag, out T value)
        {
            value = GetByTagInEditor(itemTag);

            if (value != null)
            {
                return true;
            }

            value = null;
            return false;
        }

#endif

    }

}

