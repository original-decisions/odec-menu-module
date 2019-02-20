using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Menu.DAL.Interop
{
    public interface IMenuItemRelationRepository<in TKey,TMenuItemRelation,TMenuItemRelationRouteParameter>:
        IEntityOperations<TKey, TMenuItemRelation> 
        where TKey : struct
    {
        IEnumerable<TMenuItemRelation> Get();

        void AddRouteParameter(TMenuItemRelationRouteParameter menuRelationRouteParam);
        void RemoveRouteParameter(TMenuItemRelationRouteParameter menuRelationRouteParam);
        void RemoveRouteParameter(TKey menuItemRelationId, TKey routeParamId);
    }
}