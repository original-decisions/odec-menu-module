using odec.Server.Model.Menu.Abstractions.Filters;

namespace odec.Server.Model.Menu.Filters
{
    public class ChildRoleBasedFilter<TRoleId,TParentId, TMenuRelationGroup> : MenuChildFilter<TParentId,TMenuRelationGroup>
    {
        public TRoleId RoleId { get; set; }
    }
}
