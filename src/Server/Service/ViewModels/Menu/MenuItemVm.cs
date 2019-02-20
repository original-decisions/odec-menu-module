namespace odec.Menu.ViewModels
{
    public class MenuItemVm<TKey,TParentId> : GlossaryVm<TKey>
    {
        public TParentId ParentId { get; set; }
    }
}
