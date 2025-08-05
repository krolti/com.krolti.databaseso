using UnityEngine;

namespace Krolti.DatabaseSO.Examples
{
    [CreateAssetMenu(fileName = "Simple Database", menuName = "Database SO/Examples/Simple Database")]
    public class SimpleDatabase : BenchmarkableDatabase<SimpleData> {}

    [System.Serializable]
    public class SimpleData : DatabaseItem
    {
        public string Tag;
    }
}
