using UnityEngine;

namespace Krolti.DatabaseSO.Examples
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField] private Database<ColorData> colorDatabase;
        [SerializeField] private int searchID;

        private void Start()
        {
            if(colorDatabase == null)
            {
                Debug.LogWarning("Missing color database reference");
                return;
            }

            ColorData founditem = colorDatabase.Search(searchID);
            founditem.DataTag = "Modified tag!";


            var elements = colorDatabase
                .Query()
                .Where(item => item.DataTag != "Modified tag!")
                .ToList();

            foreach (var item in elements)
            {
                item.DataTag = "Query modified tag!";
            }

            Debug.Log("Modified Items!");
        }
    }
}
