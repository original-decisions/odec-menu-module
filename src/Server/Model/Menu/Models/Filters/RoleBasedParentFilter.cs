using odec.Server.Model.Menu.Abstractions.Filters;

namespace odec.Server.Model.Menu.Filters
{
    public class RoleBasedParentFilter<TKey,TParentId, TMenuRelationGroup> : MenuParentFilter<TParentId, TMenuRelationGroup>
    {
        public TKey RoleId { get; set; }
    }

}
