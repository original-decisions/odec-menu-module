
namespace odec.Server.Model.Menu.Filters
{
    public class InternetShopFilter<TKey,TParentId, TMenuRelationGroup> : RoleBasedParentFilter<TKey, TParentId, TMenuRelationGroup>
    {
        public TKey AssemblageId { get; set; }

        public TKey GoodTypeId { get; set; }

    }
}
