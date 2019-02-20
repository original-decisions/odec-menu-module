namespace odec.Server.Model.Menu.Abstractions.Interfaces
{
    public interface IMenuContext<TMenuItem, TMenuItemRelation,TRoute, TMenuItemRelationGroup> :
        IMenuItemsContext<TMenuItem, TMenuItemRelation,TMenuItemRelationGroup> 
        where TMenuItem : class 
        where TMenuItemRelation : class 
        where TMenuItemRelationGroup : class
    {
    }
}
