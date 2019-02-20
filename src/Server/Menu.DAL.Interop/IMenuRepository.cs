using System.Collections.Generic;
using System.Threading.Tasks;
using odec.Server.Model.Menu.Abstractions;
using odec.Server.Model.Menu.Abstractions.Filters;

namespace odec.Menu.DAL.Interop
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TMenuItem"></typeparam>
    /// <typeparam name="TNavigation"></typeparam> 
    /// <typeparam name="TMenuFilter"></typeparam>
    /// <typeparam name="TChildFilter"></typeparam>
    public interface IMenuRepository<in TKey, TMenuItem, in TNavigation, in TMenuFilter, in TChildFilter> 
        where TMenuItem : class
        where TNavigation : Navigation
        //TODO:remove hard coded values  
        where TMenuFilter : MenuParentFilter<int?, string>
        where TChildFilter: MenuChildFilter<int?,string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TMenuItem GetMenuById(TKey id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<TMenuItem> GetMenuItems(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<TMenuItem>> GetMenuItemsAsync(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<TMenuItem> GetTopLevelMenus(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IList<TMenuItem>> GetTopLevelMenusAsync(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<TMenuItem> GetLayoutRelatedMenus(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<TMenuItem>> GetLayoutRelatedMenusAsync(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<TMenuItem> GetSubMenus(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<TMenuItem>> GetSubMenusAsync(TMenuFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        TMenuItem GetParentMenu(TChildFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TMenuItem> GetParentMenuAsync(TChildFilter filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="quantity"></param>
        /// <param name="onlyIsActive"></param>
        /// <returns></returns>
        IList<TMenuItem> GetMenusByNavigation(TNavigation navigation, int quantity = 5, bool onlyIsActive = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="quantity"></param>
        /// <param name="onlyIsActive"></param>
        /// <returns></returns>
        Task<IList<TMenuItem>> GetMenusByNavigationAsync(TNavigation navigation, int quantity = 5, bool onlyIsActive = true);
    }
}
