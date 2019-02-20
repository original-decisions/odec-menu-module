using System.Collections.Generic;

namespace odec.Menu.ViewModels.Specific
{
    public class MenuAdminIndexVm<TKey, TParentId>
    {
        public MenuAdminIndexVm()
        {
            RouteParameter = new RouteParameterVm<TKey>();
            RouteName = new RouteNameVm<TKey>();
            MenuItemRelationGroup = new MenuItemRelationGroupVm<TKey>();
            MenuItem = new MenuItemVm<TKey,TParentId>();
            CurrentRouteMenuItems = new List<RouteMenuItemVm<TKey,TParentId>>();
            CurrentRouteName = new List<RouteNameVm<TKey>>();
            CurrentRouteParameters =new List<RouteParameterVm<TKey>>();
        } 
        public RouteParameterVm<TKey> RouteParameter { get; set; }
        public ICollection<RouteParameterVm<TKey>> CurrentRouteParameters { get; set; }
        public RouteNameVm<TKey> RouteName { get; set; }
        public ICollection<RouteNameVm<TKey>> CurrentRouteName { get; set; }
        public MenuItemVm<TKey,TParentId> MenuItem { get; set; }

        public ICollection<RouteMenuItemVm<TKey,TParentId>> CurrentRouteMenuItems { get; set; }
        public MenuItemRelationGroupVm<TKey> MenuItemRelationGroup { get; set; }
    }
}
