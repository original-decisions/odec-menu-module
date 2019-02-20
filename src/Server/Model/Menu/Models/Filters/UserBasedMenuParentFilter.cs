using odec.Server.Model.Menu.Abstractions.Filters;

namespace odec.Server.Model.Menu.Filters
{
    public class UserBasedMenuParentFilter<TKey, TParentId, TMenuRelationGroup> : MenuParentFilter<TParentId,TMenuRelationGroup>
    {
        public TKey UserId { get; set; }
    }
}
