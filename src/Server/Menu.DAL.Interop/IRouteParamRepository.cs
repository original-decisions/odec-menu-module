using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Menu.DAL.Interop
{
    public interface IRouteParamRepository<in TKey, TRouteParamEntity, TRouteEntity> : 
        IEntityOperations<TKey, TRouteParamEntity>,
        IActivatableEntity<TKey, TRouteParamEntity> 
        where TKey : struct 
        where TRouteParamEntity : class
        where TRouteEntity : class 
    {
        IEnumerable<TRouteParamEntity> Get();
        IEnumerable<TRouteEntity> GetRouteParams();
        void AddToRoute(TRouteEntity rootEntity, TRouteParamEntity parameter);
        void AddToRoute(TKey rootEntityId, TKey parameterId);

        void DeleteFromRoute(TRouteEntity rootEntity, TRouteParamEntity parameter);
        void DeleteFromRoute(TKey rootEntityId, TKey parameterId);
    }
}