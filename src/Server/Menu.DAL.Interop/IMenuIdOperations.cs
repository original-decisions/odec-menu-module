using System.Threading.Tasks;

namespace odec.Menu.DAL.Interop
{
    public interface IMenuIdOperations<TKey, TMenuItem> where TMenuItem : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="childId"></param>
        /// <returns></returns>
        TKey GetParentMenuId(TKey childId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childMenuItem"></param>
        /// <returns></returns>
        TKey GetParentMenuId(TMenuItem childMenuItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childMenuItem"></param>
        /// <returns></returns>
        Task<TKey> GetParentMenuIdAsync(TMenuItem childMenuItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childId"></param>
        /// <returns></returns>
        Task<TKey> GetParentMenuIdAsync(TKey childId);
    }
}