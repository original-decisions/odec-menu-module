using Microsoft.EntityFrameworkCore;
using odec.Server.Model.Menu.Abstractions.Interfaces;

namespace odec.Server.Model.Menu.Abstractions.Contexts
{
    public class MenuContext<TMenuItem, TMenuItemRelation, TRouteName, TRouteParam, TMenuItemRelationRouteValue, TMenuItemRelationGroup> :
        IRouteMenuItemsContext<TMenuItem, TMenuItemRelation, TMenuItemRelationRouteValue,
            TRouteParam, TRouteName, TMenuItemRelationGroup> 
        where TRouteParam : class
        where TRouteName : class
        where TMenuItemRelationRouteValue : class
        where TMenuItemRelation : class
        where TMenuItem : class 
        where TMenuItemRelationGroup : class
    {
        public DbSet<TMenuItem> MenuItems { get; set; }
        public DbSet<TMenuItemRelation> MenuItemRelations { get; set; }
        public DbSet<TMenuItemRelationGroup> MenuItemRelationGroups { get; set; }
        public DbSet<TRouteName> RouteNames { get; set; }
        public DbSet<TRouteParam> RouteParams { get; set; }
        public DbSet<TMenuItemRelationRouteValue> MenuItemRelationRouteValues { get; set; }
    }
}
