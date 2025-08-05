
namespace Krolti.DatabaseSO
{
    public interface IFixable
    {
        /// <summary>
        /// Method to fix corrupted database items.
        /// 
        /// <para>Called if IsValid == false on IDatabaseItem.</para>
        /// </summary>
        /// <returns></returns>
        bool TryFix();
    }
}
