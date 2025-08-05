using UnityEngine;

namespace Krolti.DatabaseSO.Examples
{
    [CreateAssetMenu(fileName = "Color Database", menuName = "DatabaseSO/Examples/Color Database")]
    internal class ColorDatabase : Database<ColorData>
    {

#if UNITY_EDITOR


        [SerializeField] private int fillElementCount;

        [ContextMenu("Fill random")]
        public void FillRandom()
        {
            for (int i = 0; i < fillElementCount; i++)
            {
                Color randomColor = new Color(
                    Random.value,
                    Random.value,
                    Random.value,
                    1f
                );

                var newColor = new ColorData(randomColor);
                Data.Add(newColor);
            }
        }
#endif
    }

    [System.Serializable]
    internal class ColorData : DatabaseItem
    {
        [SerializeField] private Color color;

        [field: SerializeField] public string DataTag { get; set; }
        public Color Color => color;
        /// <summary>
        /// Just transparensy check.
        /// </summary>
        public override bool IsValid => color.a > 0;



        public ColorData(Color color)
        {
            this.color = color;
        }

        public override bool TryFix()
        {
            if(color != null)
            {
                color.a = 1;
                return true;
            }
            return false;
        }

    }
}
