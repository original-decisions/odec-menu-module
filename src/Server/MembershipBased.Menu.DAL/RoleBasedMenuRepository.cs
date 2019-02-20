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
    public class RoleBasedMenuRepository :
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
        public RoleBasedMenuRepository()
        {
        }
        public RoleBasedMenuRepository(DbContext db)
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
            var realFilter = parentFilter as RoleBasedParentFilter<int?, int?, string>;
            var filterHasValue = realFilter != null;
            var query = (from menuItemRelation in Db.Set<RoleMenuItemRelation>()
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
                                (!menuItemRelation.RoleId.HasValue || menuItemRelation.RoleId == realFilter.RoleId) &&
                                (string.IsNullOrEmpty(parentFilter.MenuItemGroup) ||
                                 (!string.IsNullOrEmpty(parentFilter.MenuItemGroup) &&
                                  menuItemRelationGroup.Name == parentFilter.MenuItemGroup))) &&
                               menuItem.IsActive
                         select new { menuItemRelation, menuItem, routeName });
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
                                  tmp.Where(it => it.menuItemRouteValue != null && it.menuItemRouteValue.routeParam != null && it.menuItemRouteValue.routeParam.Name != null).Select(it => new RouteParameterPair
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
            //(from menuItemRelation in Db.Set<RoleMenuItemRelation>()
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
            //                   (!menuItemRelation.RoleId.HasValue || menuItemRelation.RoleId == realFilter.RoleId) &&
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
            return query3;
            //.ToDictionary(it => it.routeParam.Name, it => it.menuItemRouteValue.Value)
        }
        public RouteMenuItem GetMenuById(int menuId)
        {
            return (from menu in Db.Set<MenuItem>()
                    join menuRelation in Db.Set<RoleMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
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
                    RouteParams = ri.RouteParams?.ToDictionary(kv =>
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
            var res = GetRouteMenuItems(parentFilter).AsEnumerable().Select(it => new RouteMenuItem
            {
                Id = it.Id,
                Name = it.Name,
                RouteName = it.RouteName,
                RouteParams = it.RouteParams?.ToDictionary(kv =>
                    kv.Name, kv => kv.Value),
                ParentId = it.ParentId
            }).ToList();
            return res;
            //return (from menu in Db.Set<MenuItem>()
            //    join menuRelation in Db.Set<RoleMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
            //    join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId
            //        equals menuItemRelationGroup.Id
            //    join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
            //    let routeParams = Enumerable.ToDictionary((from entity in Db.Set<MenuItemRelationRouteValue>()

            //        join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
            //        where entity.MenuItemRelationId == menuRelation.Id
            //        select new { Key = routeParam.Name, Value = entity.Value }
            //        ), it => it.Key, it => it.Value)
            //    where menuItemRelationGroup.Name == filter.MenuItemGroup && menu.ParentId == null
            //    select new { menu, routeName, routeParams }).ToList().Select(it => new RouteMenuItem
            //        {
            //            Id = it.menu.Id,
            //            RouteName = it.routeName.Name,
            //            ParentId = it.menu.ParentId,
            //            Name = it.menu.Name,
            //            RouteParams = it.routeParams
            //        }).ToList();
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
            //join menuRelation in Db.Set<RoleMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
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

        public List<RouteMenuItem> GetSubMenus(int parentId)
        {
            return (from menu in Db.Set<MenuItem>()
                    join menuRelation in Db.Set<MenuItemRelation>() on menu.Id equals menuRelation.MenuItemId

                    join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                    let routeParams = (from entity in Db.Set<MenuItemRelationRouteValue>()

                                       join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                                       where entity.MenuItemRelationId == menuRelation.Id
                                       select new { Key = routeParam.Name, Value = entity.Value }
                        )
                    where menu.ParentId == parentId
                    select new { menu, routeName, routeParams }).ToList().Select(it => new RouteMenuItem
                    {
                        Id = it.menu.Id,
                        RouteName = it.routeName.Name,
                        ParentId = it.menu.ParentId,
                        Name = it.menu.Name,
                        RouteParams = it.routeParams.ToDictionary(it2 => it2.Key, it2 => it2.Value)
                    }).ToList();
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
            var realFilter = filter as ChildRoleBasedFilter<int?, int?, string>;
            if (realFilter == null)
                return null;
            return (from childMenu in Db.Set<MenuItem>()
                    join menu in Db.Set<MenuItem>() on childMenu.ParentId equals menu.Id
                    join menuRelation in Db.Set<RoleMenuItemRelation>() on menu.Id equals menuRelation.MenuItemId
                    join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id
                    join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                    let routeParams = (from entity in Db.Set<MenuItemRelationRouteValue>()
                                       join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                                       where entity.MenuItemRelationId == menuRelation.Id
                                       select new { Key = routeParam.Name, Value = entity.Value }
                        )
                    where realFilter.ChildId == childMenu.Id && (string.IsNullOrEmpty(realFilter.MenuItemGroup) || realFilter.MenuItemGroup == menuItemRelationGroup.Name) &&
                            realFilter.RoleId == menuRelation.RoleId
                    select new { menu, menuRelation, routeName, routeParams }).ToList().Select(it => new RouteMenuItem
                    {
                        Id = it.menu.Id,
                        RouteName = it.routeName.Name,
                        ParentId = it.menu.ParentId,
                        Name = it.menu.Name,
                        RoleId = it.menuRelation.RoleId,
                        RouteParams = it.routeParams.ToDictionary(it2 => it2.Key, it2 => it2.Value)
                    }).FirstOrDefault();
        }

        class NavResult
        {
            public MenuItem MenuItem { get; set; }
            public RouteName RouteName { get; set; }
            public Dictionary<string, string> RouteParams { get; set; }
            public RoleMenuItemRelation MenuItemRelation { get; set; }
            public int? RoleId { get; set; }
        }

        public IList<RouteMenuItem> GetMenusByNavigation(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true)
        {
            try
            {
                var paramQuery = (from entity in Db.Set<MenuItemRelationRouteValue>()
                                  join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                                  group new { entity, routeParam }
                                  by new { entity.MenuItemRelationId }
                                  into tmp
                                  select new
                                  {
                                      tmp.Key.MenuItemRelationId,
                                      RouteParams = tmp.ToDictionary(it=>it.routeParam.Name,it=> it.entity.Value)
                                  });
                if (navigationGeneric.RouteParameters != null && navigationGeneric.RouteParameters.Any())
                {
                    paramQuery = navigationGeneric.RouteParameters.Aggregate(paramQuery, (current, item) => current.Where(it => it.RouteParams.Any(it2 => it2.Key == item.Key && it2.Value == (string) item.Value)));
                    //    paramQuery = navigationGeneric.RouteParameters.Aggregate(paramQuery, (current, item) => current.Where(it => it.Value == (string)item.Value && it.RouteParam.Name == item.Key));
                }
               // var selectedMiQuery = paramQuery.Select(it => it.MenuItemRelationId).Distinct();
                var result = (from menuItem in Db.Set<MenuItem>()
                             join menuItemRelation in Db.Set<RoleMenuItemRelation>() on menuItem.Id equals
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
                                 RoleId = menuItemRelation.RoleId,
                                 RouteParams = routeParam.RouteParams
                             });
                
                return result.ToList();

                //return result;
                //var query = (from menuItem in Db.Set<MenuItem>()
                //             join menuItemRelation in Db.Set<RoleMenuItemRelation>() on menuItem.Id equals
                //                 menuItemRelation.MenuItemId
                //             //join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id
                //             join routeName in Db.Set<RouteName>() on menuItemRelation.RouteNameId equals routeName.Id
                //             where navigationGeneric.RouteName == routeName.Name
                //             select new
                //             {
                //                 MenuItemId = menuItem.Id,
                //                 MenuItemName = menuItem.Name,
                //                 RouteName = routeName.Name,
                //                 MenuItemRelationId = menuItemRelation.Id,
                //                 menuItemRelation.RoleId,
                //                 ParentId = menuItem.ParentId
                //             }).Distinct();
                //if (navigationGeneric.RouteParameters != null && navigationGeneric.RouteParameters.Any())
                //{
                //    query = (from item in query
                //             join entity in Db.Set<MenuItemRelationRouteValue>() on item.MenuItemRelationId equals
                //                 entity.MenuItemRelationId
                //             join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                //             group new
                //             {
                //                 item.MenuItemId,
                //                 item.MenuItemName,
                //                 item.MenuItemRelationId,
                //                 item.ParentId,
                //                 item.RouteName,
                //                 item.RoleId,
                //                 entity,
                //                 routeParam
                //             } by
                //             new
                //             {
                //                 item.MenuItemId,
                //                 item.MenuItemName,
                //                 item.MenuItemRelationId,
                //                 item.ParentId,
                //                 item.RouteName,
                //                 item.RoleId
                //             } into tmp
                //             select new RouteMenuItem
                //             {
                //                 Id = tmp.Key.MenuItemId,
                //                 RouteName = tmp.Key.RouteName,
                //                 ParentId = tmp.Key.ParentId,
                //                 Name = tmp.Key.MenuItemName,
                //                 RoleId = tmp.Key.RoleId,

                //                 RouteParams = navigationGeneric.RouteParameters.Aggregate(tmp.ToDictionary(it=>it.entity.Value,it=>it.routeParam.Name), (current, routeParameter) => current.Where(it => it.Key == routeParameter.Key && it.Value == routeParameter.Value));
                //             });
                //}

                //var res = new List<RouteMenuItem>();
                // foreach (var item in query)
                // {
                //     var q = (from entity in Db.Set<MenuItemRelationRouteValue>()
                //         join routeParam in Db.Set<RouteParam>() on entity.RouteParamId equals routeParam.Id
                //         where entity.MenuItemRelationId == item.MenuItemRelationId
                //         select new {Key = routeParam.Name, Value = entity.Value});
                //     //if (q.Any())
                //     //{
                //     //    q = navigationGeneric.RouteParameters.Aggregate(q, (current, routeParameter) => current.Where(it => it.Key == routeParameter.Key && it.Value == routeParameter.Value));
                //     //}

                //     res.Add(new RouteMenuItem
                //     {
                //         Id = item.MenuItemId,
                //         RouteName = item.RouteName,
                //         ParentId = item.ParentId,
                //         Name = item.MenuItemName,
                //         RoleId = item.RoleId,
                //         RouteParams = q.Any() ? q.ToDictionary(it2 => it2.Key, it2 => it2.Value) : null// routeQ.Where(it=>it.MenuItemRelationId ==item.MenuItemRelationId).ToDictionary(it2 => it2.Key, it2 => it2.Value)
                //     });
                // }
                // return res;
                //group new
                //{
                //    menuItem,
                //    routeName,
                //    menuItemRelation.RoleId,
                //    tmpQ
                //} by
                // new
                // {
                //     MenuItemId = menuItem.Id,
                //     MenuItemName = menuItem.Name,
                //     RouteName = routeName.Name,
                //     menuItemRelation.RoleId,
                //     ParentId = menuItem.ParentId
                // } into tmp
                //select new RouteMenuItem
                //{
                //    Id = tmp.Key.MenuItemId,
                //    RouteName = tmp.Key.RouteName,
                //    ParentId = tmp.Key.ParentId,
                //    Name = tmp.Key.MenuItemName,
                //    RoleId = tmp.Key.RoleId,

                //    RouteParams = tmp.Where(it=>it.).ToDictionary(it2 => it2.Key, it2 => it2.Value)
                //});


                //query = navigationGeneric.RouteParameters.Aggregate(query,
                //    (current, routeParam) =>
                //        current.Where(
                //            it =>
                //                it.RouteParams != null &&
                //                it.RouteParams.Any(it2 => it2.Key == routeParam.Key && it2.Value == routeParam.Value)));

                //var result = query.AsEnumerable().Select(it => new RouteMenuItem
                //{
                //    Id = it.menuItem.Id,
                //    RouteName = it.routeName.Name,
                //    ParentId = it.menuItem.ParentId,
                //    Name = it.menuItem.Name,
                //    RoleId = it.RoleId,

                //    RouteParams = it.tmpQ.ToDictionary(it2 => it2.Key, it2 => it2.Value)
                //}).ToList();
                //return result;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public Task<IList<RouteMenuItem>> GetMenusByNavigationAsync(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true)
        {
            return
                Task<IList<RouteMenuItem>>.Factory.StartNew(
                    () => GetMenusByNavigation(navigationGeneric, quantity, onlyIsActive));
        }





    }
}
