using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Menu.DAL.Interop
{
    public interface IMenuItemRelationGroupRepository<TKey, TGroup> : 
        IEntityOperations<TKey, TGroup>,
        IActivatableEntity<TKey, TGroup>
        where TGroup : class
        where TKey : struct
    {
        IEnumerable<TGroup> Get();
    }
}