using UnityEngine;

namespace Krolti.DatabaseSO.Examples
{
    [CreateAssetMenu(fileName = "Simple Database", menuName = "DatabaseSO/Examples/Simple Database")]
    public class SimpleDatabase : BenchmarkableDatabase<SimpleData> {}

    [System.Serializable]
    public class SimpleData : DatabaseItem
    {
        public string Tag;
    }
}
