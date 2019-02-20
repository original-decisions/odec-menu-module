
using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Menu.Abstractions.Interfaces
{
    /// <summary>
    /// Прокси объект контекста меню
    /// </summary>
    /// <typeparam name="TMenuItem">тип меню</typeparam>
    /// <typeparam name="TMenuItemRelation">тип отношения меню</typeparam>
    /// <typeparam name="TRouteValue">тип значения маршрута</typeparam>
    /// <typeparam name="TNavigation">тип навигации</typeparam>
    /// <typeparam name="TNavigationAttribute">тип атрибута навигации</typeparam>
    /// <typeparam name="TNavigationRouteValue">тип навигации значения маршрута </typeparam>
    /// <typeparam name="TMenuItemRelationGroup"></typeparam>
    internal interface INavigationItemsContext<TMenuItem, TMenuItemRelation, TNavigation, TNavigationAttribute, TNavigationRouteValue, TRouteValue, TMenuItemRelationGroup>
        : IMenuItemsContext<TMenuItem, TMenuItemRelation, TMenuItemRelationGroup>
        where TNavigation : class
        where TNavigationAttribute : class
        where TNavigationRouteValue : class
        where TMenuItem : class
        where TMenuItemRelation : class
        where TRouteValue : class 
        where TMenuItemRelationGroup : class
    {
        /// <summary>
        /// таблица связи навигации
        /// </summary>
        DbSet<TNavigation> Navigations { get; set; }

        /// <summary>
        /// таблица связи атрибута навигации
        /// </summary>
        DbSet<TNavigationAttribute> NavigationAttributes { get; set; }

        /// <summary>
        /// таблица связи навигации значения маршрута
        /// </summary>
        DbSet<TNavigationRouteValue> NavigationRouteValues { get; set; }

        /// <summary>
        /// таблица связи значения маршрута
        /// </summary>
        DbSet<TRouteValue> RouteValues { get; set; }
    }
}
