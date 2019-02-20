using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Menu.DAL.Interop
{
    public interface IMenuItemRepository<in TKey,TMenuItem>:
        IEntityOperations<TKey, TMenuItem>, 
        IActivatableEntity<TKey, TMenuItem> 
        where TKey : struct 
        where TMenuItem : class
    {
        IEnumerable<TMenuItem> Get();
    }
}