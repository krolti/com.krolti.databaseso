using UnityEngine;

namespace Krolti.DatabaseSO.Examples
{
    public class Assistant : MonoBehaviour
    {
        [SerializeField] private TagRepository<RespondData> respondData;
        [SerializeField] private string input;
        [SerializeField] private string tryInput;

        private void Awake()
        {
            string respond = respondData.GetByTag(input).GetRespond();
            Debug.Log($"Assistant's respond: {respond}");

            if(respondData.TryGetValue(tryInput, out var data))
            {
                Debug.Log($"Assistant's second respond, successfully: {data.GetRespond()}");
            }
        }
    }
}
