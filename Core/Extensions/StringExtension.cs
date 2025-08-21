
namespace Krolti.DatabaseSO
{
    internal static class StringExtension
    {
        internal static string Format<T>(this string format) where T : class, IDatabaseItem
        {
            string newformatted = string.Format("<color=#ff9d00>DATABASE SO</color> [{0}<{2}>] {1} Type: {2}",
                nameof(Database<T>),
                format,
                typeof(T));
            return newformatted;
        }

        internal static string Format<T>(this string format, T item) where T : class, IDatabaseItem
        {
            string newformatted = string.Format("<color=#ff9d00>DATABASE SO</color> [{0}<{2}>] {1} Type: {2}, ID: {3}",
                nameof(Database<T>),
                format,
                typeof(T),
                item.ID);
            return newformatted;
        }
    }
}
