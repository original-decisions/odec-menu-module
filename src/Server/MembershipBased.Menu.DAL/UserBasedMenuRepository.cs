using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL.Interop;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.Menu;
using odec.Server.Model.Menu.Abstractions.Filters;
using odec.Server.Model.Menu.Filters;
using odec.Server.Model.Menu.Specific;
using Navigation = odec.Server.Model.Menu.Abstractions.Navigation;

namespace odec.Membership.Menu.DAL
{
    public class UserBasedMenuRepository :
        IContextRepository<DbContext>,
        IMenuRepository<int, RouteMenuItem, Navigation, MenuParentFilter<int?, string>, MenuChildFilter<int?, string>>
    {
        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }
        public void SetContext(DbContext db)
        {
            Db = db;
        }
        public UserBasedMenuRepository()
        {

            //Db = new UserBasedMenuContext();
            //Database.SetInitializer<UserBasedMenuContext>(null);
        }
        public UserBasedMenuRepository(DbContext db)
        {
            SetContext(db);
        }

        protected class RouteQueryResult
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string RouteName { get; set; }
            public int? ParentId { get; set; }
            public int SortOrder { get; set; }
            public IEnumerable<RouteParameterPair> RouteParams { get; set; }
        }

        protected class RouteParameterPair
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        protected IQueryable<RouteQueryResult> GetRouteMenuItems(MenuParentFilter<int?, string> parentFilter)
        {
            var realFilter = parentFilter as UserBasedMenuParentFilter<int?, int?, string>;
            var filterHasValue = realFilter != null;
            var query = (from menuItemRelation in Db.Set<UserMenuItemRelation>()
                         join menuItem in Db.Set<MenuItem>() on menuItemRelation.MenuItemId equals menuItem.Id
                         join routeName in Db.Set<RouteName>() on menuItemRelation.RouteNameId equals routeName.Id
                         join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on
                             menuItemRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id

                         
                         //from menuItemRouteValue in tmprelationRouteValues.DefaultIfEmpty()
                         //join routeParam in Db.Set<RouteParam>() on menuItemRouteValue.RouteParamId equals routeParam.Id into
                          //   tmpRouteParams
                       //  from routeParam in tmpRouteParams.DefaultIfEmpty()
                         where !filterHasValue ||
                               (filterHasValue && realFilter.ParentId == menuItem.ParentId &&
                                (!menuItemRelation.UserId.HasValue || menuItemRelation.UserId == realFilter.UserId) &&
                                (string.IsNullOrEmpty(parentFilter.MenuItemGroup) ||
                                 (!string.IsNullOrEmpty(parentFilter.MenuItemGroup) &&
                                  menuItemRelationGroup.Name == parentFilter.MenuItemGroup))) &&
                               menuItem.IsActive
                         select new { menuItemRelation, menuItem, routeName});
            var menuItemRelationsQuery = from menuItemRouteValue in Db.Set<MenuItemRelationRouteValue>()
                                         join routeParam in Db.Set<RouteParam>() on menuItemRouteValue.RouteParamId equals routeParam.Id
                                         select new
                                         {
                                             routeParam,
                                             menuItemRouteValue
                                         };



            var query2 = from q in query

                         join menuItemRouteValue in menuItemRelationsQuery
                             on q.menuItemRelation.Id equals menuItemRouteValue.menuItemRouteValue.MenuItemRelationId into tmprelationRouteValues
                         from menuItemRouteValue in tmprelationRouteValues.DefaultIfEmpty()
                         select new { q.routeName, q.menuItem, menuItemRouteValue };
            var query3 = (from q in query2


                          group new { q.menuItemRouteValue, q.menuItem, q.routeName } by
                              new
                              {
                                  q.menuItem.Id,
                                  q.menuItem.ParentId,
                                  MenuItemName = q.menuItem.Name,
                                  RouteName = q.routeName.Name,
                                  SortOrder = q.menuItem.SortOrder
                              }
                into tmp
                          select new RouteQueryResult
                          {
                              Id = tmp.Key.Id,
                              Name = tmp.Key.MenuItemName,
                              RouteName = tmp.Key.RouteName,
                              RouteParams =
                                  tmp.Where(it => it.menuItemRouteValue!= null && it.menuItemRouteValue.routeParam != null && it.menuItemRouteValue.routeParam.Name != null).Select(it => new RouteParameterPair
                                  {
                                      Name = it.menuItemRouteValue.routeParam.Name,
                                      Value = it.menuItemRouteValue.menuItemRouteValue.Value
                                  }).Distinct(),
                              // .Select(it => new {Key = it.routeParam.Name, Value = it.menuItemRouteValue.Value}),
                              ParentId = tmp.Key.ParentId,
                              // UserId = userId,
                              SortOrder = tmp.Key.SortOrder
                          }).OrderBy(it => it.SortOrder);
            return query3;
            //var query = (from menuItemRelation in Db.Set<UserMenuItemRelation>()
            //        join menuItem in Db.Set<MenuItem>() on menuItemRelation.MenuItemId equals menuItem.Id
            //        join routeName in Db.Set<RouteName>() on menuItemRelation.RouteNameId equals routeName.Id
            //        join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuItemRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id

            //        join menuItemRouteValue in Db.Set<MenuItemRelationRouteValue>()
            //            on menuItemRelation.Id equals menuItemRouteValue.MenuItemRelationId into tmprelationRouteValues
            //        from menuItemRouteValue in tmprelationRouteValues.DefaultIfEmpty()
            //        join routeParam in Db.Set<RouteParam>() on menuItemRouteValue.RouteParamId equals routeParam.Id into
            //            tmpRouteParams
            //        from routeParam in tmpRouteParams.DefaultIfEmpty()
            //        where !filterHasValue ||
            //               (filterHasValue && realFilter.ParentId == menuItem.ParentId &&
            //                    (!menuItemRelation.UserId.HasValue || menuItemRelation.UserId == realFilter.UserId) &&
            //                    (string.IsNullOrEmpty(parentFilter.MenuItemGroup) || (!string.IsNullOrEmpty(parentFilter.MenuItemGroup) && menuItemRelationGroup.Name == parentFilter.MenuItemGroup))) &&
            //              menuItem.IsActive
            //        group new { menuItemRouteValue, menuItem, routeName, routeParam } by
            //            new
            //            {
            //                menuItem.Id,
            //                menuItem.ParentId,
            //                MenuItemName = menuItem.Name,
            //                RouteName = routeName.Name,
            //                SortOrder = menuItem.SortOrder
            //            }
            //    into tmp
            //        select new RouteQueryResult
            //        {
            //            Id = tmp.Key.Id,
            //            Name = tmp.Key.MenuItemName,
            //            RouteName = tmp.Key.RouteName,
            //            RouteParams =
            //                tmp.Where(it => it.routeParam != null && it.routeParam.Name != null).Select(it => new RouteParameterPair
            //                {
            //                    Name = it.routeParam.Name,
            //                    Value = it.menuItemRouteValue.Value
            //                }).Distinct(),
            //            // .Select(it => new {Key = it.routeParam.Name, Value = it.menuItemRouteValue.Value}),
            //            ParentId = tmp.Key.ParentId,
            //            // UserId = userId,
            //            SortOrder = tmp.Key.SortOrder
            //        }).OrderBy(it => it.SortOrder);
            //return query;
            //.ToDictionary(it => it.routeParam.Name, it => it.menuItemRouteValue.Value)
        }
        public RouteMenuItem GetMenuById(int menuId)
        {
            return (from menu in Db.Set<MenuItem>()
                    join menuRelation in Db.Set<UserMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
                    join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId
                        equals menuItemRelationGroup.Id
                    join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                    let routeParams = (from entity in Db.Set<MenuItemRelationRouteValue>()
                                       join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                                       where entity.MenuItemRelationId == menuRelation.Id
                                       select new { Key = routeParam.Name, Value = entity.Value }
                        )
                    where menu.Id == menuId
                    select new { menu, routeName, routeParams }).ToList().Select(it => new RouteMenuItem
                    {
                        Id = it.menu.Id,
                        RouteName = it.routeName.Name,
                        ParentId = it.menu.ParentId,
                        Name = it.menu.Name,
                        RouteParams = it.routeParams.ToDictionary(it2 => it2.Key, it2 => it2.Value)
                    }).First();
        }

        public Task<IList<RouteMenuItem>> GetTopLevelMenusAsync(MenuParentFilter<int?, string> parentFilter)
        {
            return Task<IList<RouteMenuItem>>.Factory.StartNew(() => GetTopLevelMenus(parentFilter));
        }

        public IEnumerable<RouteMenuItem> GetMenuItems(MenuParentFilter<int?, string> parentFilter)
        {
            var result = new List<RouteMenuItem>();
            foreach (var ri in GetRouteMenuItems(parentFilter))
            {
                var item = new RouteMenuItem
                {
                    Id = ri.Id,
                    Name = ri.Name,
                    RouteName = ri.RouteName,
                    RouteParams = ri.RouteParams.ToDictionary(kv =>
                        kv.Name, kv => kv.Value),
                    ParentId = ri.ParentId
                };
                parentFilter.ParentId = ri.Id;
                if (parentFilter.InitHierarchy)
                    item.Children = GetMenuItems(parentFilter);
                result.Add(item);
            }
            return result;
        }

        public Task<IEnumerable<RouteMenuItem>> GetMenuItemsAsync(MenuParentFilter<int?, string> parentFilter)
        {
            return Task<IEnumerable<RouteMenuItem>>.Factory.StartNew(() => GetMenuItems(parentFilter));
        }

        public IList<RouteMenuItem> GetTopLevelMenus(MenuParentFilter<int?, string> parentFilter)
        {
            //TODO:think about rightness of this act
            parentFilter.InitHierarchy = false;
            parentFilter.ParentId = null;
            try
            {
                var result = GetRouteMenuItems(parentFilter).AsEnumerable().Select(it => new RouteMenuItem
                {
                    Id = it.Id,
                    Name = it.Name,
                    RouteName = it.RouteName,
                    RouteParams = it.RouteParams?.ToDictionary(kv =>
                        kv.Name, kv => kv.Value),
                    ParentId = it.ParentId
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
           
        }
        //TODO:Extract
        //public IList<Good> GetGoodList(int menuID)
        //{
        //    return (from menuGoods in Db.Set<MenuGood>()
        //            join good in Db.Set<Good>() on menuGoods.GoodId equals good.Id
        //            where menuID == menuGoods.MenuId
        //            select good).ToList();
        //}
        //public IList<Good> GetGoodList(MenuItem menu)
        //{
        //    return GetGoodList((int) menu.Id);
        //}

        //public Task<IList<Good>> GetGoodListAsync(int menuID)
        //{
        //    return Task<IList<Good>>.Factory.StartNew(() => GetGoodList(menuID));
        //}

        //public Task<IList<Good>> GetGoodListAsync(MenuItem menu)
        //{
        //    return Task<IList<Good>>.Factory.StartNew(() => GetGoodList(menu));
        //}

        public List<RouteMenuItem> GetLayoutRelatedMenus(MenuParentFilter<int?, string> parentFilter)
        {
            return GetRouteMenuItems(parentFilter).AsEnumerable().Select(it => new RouteMenuItem
            {
                Id = it.Id,
                Name = it.Name,
                RouteName = it.RouteName,
                RouteParams = it.RouteParams.ToDictionary(kv =>
                    kv.Name, kv => kv.Value),
                ParentId = it.ParentId
            }).ToList();

            //(from menu in Db.Set<MenuItem>()
            //join menuRelation in Db.Set<UserMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
            //join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId
            //    equals menuItemRelationGroup.Id
            //join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
            //let routeParams = (from entity in Db.Set<MenuItemRelationRouteValue>()

            //    join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
            //    where entity.MenuItemRelationId == menuRelation.Id
            //    select new { Key = routeParam.Name, Value = entity.Value }
            //    )
            //where menuItemRelationGroup.Name == filter.MenuItemGroup
            //select new { menu, routeName, routeParams }).ToList().Select(it => new RouteMenuItem
            //    {
            //        Id = it.menu.Id,
            //        RouteName = it.routeName.Name,
            //        ParentId = it.menu.ParentId,
            //        Name = it.menu.Name,
            //        RouteParams = it.routeParams.ToDictionary(it2 => it2.Key, it2 => it2.Value)
            //}).ToList();
            //return (from menu in Db.Set<MenuItem>()
            //        join menuRelation in Db.Set<MenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
            //        join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId
            //            equals menuItemRelationGroup.Id
            //        where menuItemRelationGroup.Name == filter.MenuItemGroup
            //        select menu).ToList().Select(it => new RouteMenuItem
            //        {

            //        }).ToList();
        }

        public Task<List<RouteMenuItem>> GetLayoutRelatedMenusAsync(MenuParentFilter<int?, string> parentFilter)
        {
            return Task<List<RouteMenuItem>>.Factory.StartNew(() => GetLayoutRelatedMenus(parentFilter));
        }
        public List<RouteMenuItem> GetSubMenus(MenuParentFilter<int?, string> parentFilter)
        {
            if (!parentFilter.ParentId.HasValue)
                throw new ArgumentNullException("filter.ParentId");
            return GetRouteMenuItems(parentFilter).ToList().Select(it => new RouteMenuItem
            {
                Id = it.Id,
                RouteName = it.RouteName,
                ParentId = it.ParentId,
                Name = it.Name,
                RouteParams = it.RouteParams.ToDictionary(it2 => it2.Name, it2 => it2.Value)
            }).ToList(); //GetSubMenus(filter.ParentId.Value);
        }


        public Task<RouteMenuItem> GetParentMenuAsync(MenuChildFilter<int?, string> filter)
        {
            return Task<RouteMenuItem>.Factory.StartNew(() => GetParentMenu(filter));
        }



        public Task<List<RouteMenuItem>> GetSubMenusAsync(MenuParentFilter<int?, string> parentFilter)
        {
            return Task<List<RouteMenuItem>>.Factory.StartNew(() => GetSubMenus(parentFilter));
        }


        public RouteMenuItem GetParentMenu(MenuChildFilter<int?, string> filter)
        {
            //TODO: refactor in protected IQuerible 
            var realFilter = filter as ChildUserBasedFilter<int?, int?, string>;
            if (realFilter == null)
                return null;
            return (from childMenu in Db.Set<MenuItem>()
                    join menu in Db.Set<MenuItem>() on childMenu.ParentId equals menu.Id
                    join menuRelation in Db.Set<UserMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
                    join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id
                    join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                    let routeParams = (from entity in Db.Set<MenuItemRelationRouteValue>()
                                       join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                                       where entity.MenuItemRelationId == menuRelation.Id
                                       select new { Key = routeParam.Name, Value = entity.Value }
                        )
                    where realFilter.ChildId == childMenu.Id && (string.IsNullOrEmpty(realFilter.MenuItemGroup) || realFilter.MenuItemGroup == menuItemRelationGroup.Name) &&
                            realFilter.UserId == menuRelation.UserId
                    select new { menu, menuRelation, routeName, routeParams }).ToList().Select(it => new RouteMenuItem
                    {
                        Id = it.menu.Id,
                        RouteName = it.routeName.Name,
                        ParentId = it.menu.ParentId,
                        Name = it.menu.Name,
                        UserId = it.menuRelation.UserId,
                        RouteParams = it.routeParams.ToDictionary(it2 => it2.Key, it2 => it2.Value)
                    }).FirstOrDefault();
        }


        public IList<RouteMenuItem> GetMenusByNavigation(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true)
        {
            var paramQuery = (from entity in Db.Set<MenuItemRelationRouteValue>()
                              join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                              group new { entity, routeParam }
                              by new { entity.MenuItemRelationId }
                                  into tmp
                              select new
                              {
                                  tmp.Key.MenuItemRelationId,
                                  RouteParams = tmp.ToDictionary(it => it.routeParam.Name, it => it.entity.Value)
                              });
            if (navigationGeneric.RouteParameters != null && navigationGeneric.RouteParameters.Any())
            {
                paramQuery = navigationGeneric.RouteParameters.Aggregate(paramQuery, (current, item) => current.Where(it => it.RouteParams.Any(it2 => it2.Key == item.Key && it2.Value == (string)item.Value)));
                //    paramQuery = navigationGeneric.RouteParameters.Aggregate(paramQuery, (current, item) => current.Where(it => it.Value == (string)item.Value && it.RouteParam.Name == item.Key));
            }
            // var selectedMiQuery = paramQuery.Select(it => it.MenuItemRelationId).Distinct();
            var result = (from menuItem in Db.Set<MenuItem>()
                          join menuItemRelation in Db.Set<UserMenuItemRelation>() on menuItem.Id equals
                              menuItemRelation.MenuItemId
                          //join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id

                          join routeName in Db.Set<RouteName>() on menuItemRelation.RouteNameId equals routeName.Id
                          join routeParam in paramQuery on menuItemRelation.Id equals routeParam.MenuItemRelationId
                          where navigationGeneric.RouteName == routeName.Name
                          select new RouteMenuItem
                          {
                              Id = menuItem.Id,
                              RouteName = routeName.Name,
                              ParentId = menuItem.ParentId,
                              Name = menuItem.Name,
                              UserId = menuItemRelation.UserId,
                              RouteParams = routeParam.RouteParams
                          });

            return result.ToList();
        }

        public Task<IList<RouteMenuItem>> GetMenusByNavigationAsync(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true)
        {
            return
                Task<IList<RouteMenuItem>>.Factory.StartNew(
                    () => GetMenusByNavigation(navigationGeneric, quantity, onlyIsActive));
        }




    }
}
