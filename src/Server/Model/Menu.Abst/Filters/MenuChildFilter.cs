namespace odec.Server.Model.Menu.Abstractions.Filters
{
    public abstract class MenuChildFilter<TChildId, TMenuItemGroup> : MenuFilterBase<TMenuItemGroup>
    {
        public TChildId ChildId { get; set; }
    }
}