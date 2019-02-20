

using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Menu.Abstractions.Interfaces
{
    /// <summary>
    /// Прокси объект контекста меню
    /// </summary>
    /// <typeparam name="TMenuItem">тип меню</typeparam>
    /// <typeparam name="TMenuItemRelation">тип отношения меню</typeparam>
    /// <typeparam name="TMenuItemRelationRouteValue">type of Route values of the MenuItemRelation</typeparam>
    /// <typeparam name="TRouteParam">Route param type</typeparam>
    /// <typeparam name="TRouteName">Route name Type</typeparam>
    /// <typeparam name="TMenuItemRelationGroup"></typeparam>
    public interface IRouteMenuItemsContext<TMenuItem, TMenuItemRelation,  TMenuItemRelationRouteValue,
        TRouteParam, TRouteName, TMenuItemRelationGroup> : IMenuItemsContext<TMenuItem, TMenuItemRelation, TMenuItemRelationGroup>
        where TMenuItem : class
        where TMenuItemRelation : class
        where TMenuItemRelationRouteValue : class
        where TRouteParam : class
        where TRouteName : class 
        where TMenuItemRelationGroup : class
    {
        /// <summary>
        /// Route names (default etc)
        /// </summary>
        DbSet<TRouteName> RouteNames { get; set; }

        /// <summary>
        /// Route params (like controller, action)
        /// </summary>
        DbSet<TRouteParam> RouteParams { get; set; }

        /// <summary>
        /// RouteValues for special MenuItemRelation
        /// </summary>
        DbSet<TMenuItemRelationRouteValue> MenuItemRelationRouteValues { get; set; }
    }
}
