
namespace Krolti.DatabaseSO
{
    public class BenchmarkableDatabase<T> : Database<T> where T : class, IDatabaseItem
    {
        public T Becnchmark_ForExample(int id)
        {
            int count = Data.Count;

            for (int i = 0; i < count; i++)
            {
                if (Data[i].ID == id)
                {
                    return Data[i];
                }
            }

            return null;
        }



        public T Becnchmark_ForeachExample(int id)
        {
            foreach (var item in Data)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
