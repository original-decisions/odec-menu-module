using System.Collections.Generic;
using System.Threading.Tasks;
using odec.Server.Model.Menu.Abstractions;
using odec.Server.Model.Menu.Abstractions.Filters;

namespace odec.Menu.DAL.Interop
{
    public interface IGoodMenuRepository<TKey, TGood, TMenuItem, TNavigation, TMenuFilter,TChildFilter> :
        IMenuRepository<TKey,TMenuItem, TNavigation, TMenuFilter, TChildFilter>
        where TMenuItem : class 
        where TNavigation : Navigation
        where TMenuFilter : MenuParentFilter<int?, string> 
        where TChildFilter : MenuChildFilter<int?, string>
    {
        IList<TGood> GetGoodList(TKey menuId);

        IList<TGood> GetGoodList(TMenuItem menuItem);
        Task<IList<TGood>> GetGoodListAsync(TKey menuId);

        Task<IList<TGood>> GetGoodListAsync(TMenuItem menuItem);
    }
}