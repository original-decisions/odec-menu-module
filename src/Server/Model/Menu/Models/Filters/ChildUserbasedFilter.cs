using odec.Server.Model.Menu.Abstractions.Filters;
namespace odec.Server.Model.Menu.Filters
{
    public class ChildUserBasedFilter<TUserId, TParentId, TMenuRelationGroup> : MenuChildFilter<TParentId, TMenuRelationGroup>
    {
        public TUserId UserId { get; set; }
    }
}
