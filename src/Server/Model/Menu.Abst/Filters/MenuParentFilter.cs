namespace odec.Server.Model.Menu.Abstractions.Filters
{
    public abstract class MenuParentFilter<TParentId, TMenuItemGroup> : MenuFilterBase<TMenuItemGroup>
    {
        public TParentId ParentId { get; set; }
    }
}