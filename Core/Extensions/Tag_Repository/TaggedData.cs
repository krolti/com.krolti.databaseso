using UnityEngine;

namespace Krolti.DatabaseSO
{
    public class TaggedData : DatabaseItem, ITagData
    {
        [SerializeField] private string itemTag;

        private readonly object _overwriteLock = new object();

        public string ItemTag => itemTag;

        public override bool IsValid => !string.IsNullOrEmpty(itemTag);


        public virtual void OverwriteTag(string tagToOverwrite)
        {
            if (string.IsNullOrEmpty(itemTag))
            {
                throw DebugDB.Exception("Requested null or empty string", this);
            }

            lock (_overwriteLock)
            {
                itemTag = tagToOverwrite;
            }
        }
    }
}
