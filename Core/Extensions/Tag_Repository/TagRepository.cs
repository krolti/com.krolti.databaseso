using System;
using System.Collections.Generic;
using UnityEngine;

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

        [Header("Leave on 0 to get default max length (1000)")]
        [Space(8)]
        [Tooltip("Default length: 1000")]
        [SerializeField] private uint tagMaxLength = 0;

        private Dictionary<string, T> _tagToItem;
        
        private uint MaxLengthComparer => tagMaxLength != 0 ? tagMaxLength : MaxTagLength;

        public bool IsRepositoryInit { get; private set; } = false;


        protected override void OnEnable()
        {
            base.OnEnable();
            AttributeUtility.CheckDatabaseTypes<T>();
        }



        protected internal override void Initialize()
        {
            base.Initialize();
            InitializeRepository();
        }



        public T GetByTag(string itemTag, bool safeMode = true)
        {
            if (string.IsNullOrEmpty(itemTag))
            {
                if (safeMode)
                {
                    UnityEngine.Debug.LogWarningFormat(EmptyTagException.EmptyTagExceptionMessage,
                        nameof(TagRepository<T>), typeof(T));
                    return null;
                }
                else
                {
                    throw new EmptyTagException(nameof(TagRepository<T>), typeof(T));
                }

            }


            if (itemTag.Length > MaxLengthComparer)
            {
                if (safeMode)
                {
                    UnityEngine.Debug.LogWarningFormat(StringLengthException.InvalidLengthMessage,
                        nameof(TagRepository<T>), typeof(T), itemTag.Length, MaxTagLength
                    );
                    return null;
                }
                else
                {
                    throw new StringLengthException(nameof(TagRepository<T>), typeof(T), 
                        itemTag.Length, MaxTagLength);
                }

            }


            if (!IsRepositoryInit || _tagToItem == null) InitializeRepository();


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
            return _tagToItem.ContainsKey(itemTag);
        }



        private void InitializeRepository()
        {
            if (_tagToItem == null)
                _tagToItem = new();


            _tagToItem.Clear();


            foreach (var item in Data)
            {

                if (_tagToItem.ContainsKey(item.ItemTag))
                {
                    UnityEngine.Debug.LogErrorFormat("[{0}] Found duplicates in the tag repository. " +
                        "Tag: {1}. Repository type: {2}. Found id: {3}",
                        nameof(TagRepository<T>),
                        item.ItemTag,
                        typeof(T),
                        item.ID
                        );
                }


                if (string.IsNullOrEmpty(item.ItemTag))
                {
                    throw new EmptyTagException(nameof(TagRepository<T>), typeof(T));

                }


                if (item.ItemTag.Length > MaxLengthComparer)
                {
                    throw new StringLengthException(nameof(TagRepository<T>), typeof(T),
                        item.ItemTag.Length, MaxTagLength);
                }


                _tagToItem[item.ItemTag] = item;
            }

            IsRepositoryInit = true;

        }

        public override void SetDatabaseDirty()
        {
            base.SetDatabaseDirty();
            IsRepositoryInit = false;
        }

    }

    internal class StringLengthException : Exception
    {
        /// <summary>
        /// <para>0 - database name.</para>
        /// <para>1 - type.</para>
        /// <para>2 - length.</para>
        /// <para>3 - max length.</para>
        /// </summary>
        internal const string InvalidLengthMessage = "[{0}] Requested item tag exceeded " +
            "max length: (current - '{2}'). Change the tagMaxLength if you are aware that the tag's hash code " +
            "will be much harder to get and {0} will overuse memory. " +
            "Current max length: {3} Repository type: {1}";

        public StringLengthException(params object[] args) 
            : base(string.Format(InvalidLengthMessage, args)) { }
    }

    internal class EmptyTagException : Exception
    {
        /// <summary>
        /// <para>0 - database type.</para>
        /// <para>1 - type.</para>
        /// </summary>
        internal const string EmptyTagExceptionMessage = "[{0}] Requested item tag " +
            "is null or empty string. Repository type: {1}";

        public EmptyTagException(params object[] args)
            : base(string.Format(EmptyTagExceptionMessage, args)) { }
    }

}

