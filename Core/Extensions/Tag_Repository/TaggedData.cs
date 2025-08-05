using UnityEngine;

namespace Krolti.DatabaseSO
{
    public class TaggedData : DatabaseItem, ITagData
    {
        [SerializeField] private string itemTag;
        public string ItemTag => itemTag;

        public override bool IsValid => !string.IsNullOrEmpty(itemTag);
    }
}
