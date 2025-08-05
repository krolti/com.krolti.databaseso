using UnityEngine;

namespace Krolti.DatabaseSO
{
    /// <summary>
    /// Base class for the Database. 
    /// 
    /// <para>Every inherited class should include [System.Serializable].</para>
    /// </summary>
    [System.Serializable]
    public abstract class DatabaseItem : IDatabaseItem, IRewritableItem, IFixable
    {
        [Tooltip("Unique id of the database element")]
        [SerializeField] private int id;

        public int ID => id;

        public virtual bool IsValid => true;
        public virtual bool TryFix() => false;


        public virtual void OverwriteID(int id)
        {
            if (id < 0)
            {
                throw new System.IndexOutOfRangeException(nameof(id));
            }

            this.id = id;
        }
    }

}
