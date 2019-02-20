using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Menu.DAL.Interop
{
    public interface IRouteNameRepository<in TKey,TRouteNameEntity>:IEntityOperations<TKey, TRouteNameEntity>,IActivatableEntity<TKey, TRouteNameEntity> 
        where TKey : struct 
        where TRouteNameEntity : class
    {
        IEnumerable<TRouteNameEntity> Get();
    }
}