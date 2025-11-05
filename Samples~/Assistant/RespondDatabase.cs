using UnityEngine;

namespace Krolti.DatabaseSO.Examples
{
    [CreateAssetMenu(fileName = "Respond Database", menuName = "Database SO/Examples/Respond Database")]
    public class RespondDatabase : TagRepository<RespondData>
    {

    }
    [System.Serializable]

    public class RespondData : TaggedData
    {
        // Take input as a database tag
        [SerializeField] private string output;

        public string GetRespond() => output;

        public override bool IsValid => string.IsNullOrEmpty(output) == false;
    }
}
