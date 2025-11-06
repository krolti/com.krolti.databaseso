using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Krolti.DatabaseSO.Examples
{
    public class BenchmarkUI : MonoBehaviour
    {
        [SerializeField] private BenchmarkUtility benchmarkUtility;
        [SerializeField] private Slider operationsCountSlider;
        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            operationsCountSlider.onValueChanged.AddListener(HandleValueChanged);
        }
        private void HandleValueChanged(float value)
        {
            text.SetText(((int)value).ToString());
        }

        public void BenchmarkToJson()
        {
            benchmarkUtility.BenchmarkToJson(GetOperationsCount());
        }

        public void BenchmarkSearch()
        {
            benchmarkUtility.BenchmarkSearch(GetOperationsCount());
        }
        public void BenchmarkFor()
        {
            benchmarkUtility.BenchmarkFor(GetOperationsCount());
        }
        public void BenchmarkForeach()
        {
            benchmarkUtility.BenchmarkForeach(GetOperationsCount());
        }
        public void BenchmarkContains()
        {
            benchmarkUtility.BenchmarkContains(GetOperationsCount());
        }

        private void OnDestroy()
        {
            operationsCountSlider.onValueChanged.RemoveAllListeners();   
        }

        public int GetOperationsCount()
            => (int)operationsCountSlider.value;
    }
}
