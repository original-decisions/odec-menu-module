using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Membership.Menu.DAL;
using odec.Menu.DAL;
using odec.Menu.DAL.Interop;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.MembershipMenu.Context;
using odec.Server.Model.Menu;
using odec.Server.Model.Menu.Abstractions.Filters;
using odec.Server.Model.Menu.Filters;
using odec.Server.Model.Menu.Specific;
using odec.Server.Model.User;
using Navigation = odec.Server.Model.Menu.Abstractions.Navigation;
//using IMenuRepository = odec.Menu.DAL.Interop.IMenuRepository<int, odec.Server.Model.Menu.Specific.RouteMenuItem, odec.Server.Model.Menu.Abstractions.Navigation, odec.Server.Model.Menu.Abstractions.Filters.MenuParentFilter<int?, string>, odec.Server.Model.Menu.Abstractions.Filters.MenuChildFilter<int?, string>>;
namespace Menu.DAL.Tests
{
    public class RoleBasedMenuRepositoryTester : Tester<RoleBasedMenuContext>
    {
        private IMenuRepository<int, RouteMenuItem, Navigation, MenuParentFilter<int?, string>, MenuChildFilter<int?, string>> _repository;
        public RoleBasedMenuRepositoryTester()
        {
            _repository = new RoleBasedMenuRepository();//(IMenuRepository)IocHelper.GetObjects<IMenuRepository>().First(it => it.Key.Contains("Role")).Value;
            //Db = new RoleBasedMenuContext(Connection);
            InitContext();

        }
        protected DbContext Db
        {
            get;
            set;
        }
        [Test]
        public void InitContext()
        {
            var ctx = (_repository as IContextRepository<DbContext>);
            if (ctx == null)
                throw new InvalidCastException("No db Context used");
            Assert.NotNull(ctx);
            var options = CreateNewContextOptions();
            Db = new RoleBasedMenuContext(options);
            ;
            Assert.DoesNotThrow(() => ctx.SetContext(Db));
            MenuTestHelper.PopulateDefaultMenuDataRoleCtx(Db);
        }
        [Test]
        public void GetMenuById()
        {
            try
            {
                LogEventManager.Logger.Info("GetMenuById(TKey id) execution have started");
                var id = 1;
                RouteMenuItem item = null;
                Assert.DoesNotThrow(() => item = _repository.GetMenuById(id));
                Assert.True(item.Id == id);
                Assert.NotNull(item);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetMenuById(TKey id) execution have ended");
            }
        }
        [Test]
        public void GetMenuItems()
        {
            try
            {
                LogEventManager.Logger.Info("GetMenuItems(menuFilter) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetMenuItems(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1
                }));
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                Assert.DoesNotThrow(() => items = _repository.GetMenuItems(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 2
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 1);
                Assert.DoesNotThrow(() => items = _repository.GetMenuItems(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 3
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetMenuItems(menuFilter) execution have ended");
            }
        }
        [Test]
        public void GetMenuItemsAsync()
        {
            try
            {
                LogEventManager.Logger.Info("GetMenuItemsAsync(MenuFilter<int?, string> filter) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetMenuItemsAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1
                }).Result);
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                Assert.DoesNotThrow(() => items = _repository.GetMenuItemsAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 2
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 1);
                Assert.DoesNotThrow(() => items = _repository.GetMenuItemsAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 3
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetMenuItemsAsync(MenuFilter<int?, string> filter) execution have ended");
            }
        }
        [Test]
        public void GetTopLevelMenus()
        {
            try
            {
                LogEventManager.Logger.Info("GetTopLevelMenus(MenuFilter<int?, string> filter) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetTopLevelMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1
                }));
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                Assert.DoesNotThrow(() => items = _repository.GetTopLevelMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 2
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 1);
                Assert.DoesNotThrow(() => items = _repository.GetTopLevelMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 3
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetTopLevelMenus(MenuFilter<int?, string> filter) execution have ended");
            }
        }
        [Test]
        public void GetTopLevelMenusAsync()
        {
            try
            {
                LogEventManager.Logger.Info("GetTopLevelMenusAsync(MenuFilter<int?, string> filter) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetTopLevelMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1
                }).Result);
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                Assert.DoesNotThrow(() => items = _repository.GetTopLevelMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 2
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 1);
                Assert.DoesNotThrow(() => items = _repository.GetTopLevelMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 3
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetTopLevelMenusAsync(MenuFilter<int?, string> filter) execution have ended");
            }
        }
        [Test]
        public void GetLayoutRelatedMenus()
        {
            try
            {
                LogEventManager.Logger.Info("GetLayoutRelatedMenus(MenuFilter<int?, string> filter) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetLayoutRelatedMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout"
                }));
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                Assert.DoesNotThrow(() => items = _repository.GetLayoutRelatedMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 2,
                    MenuItemGroup = "Layout"

                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 1);
                Assert.DoesNotThrow(() => items = _repository.GetLayoutRelatedMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 3,
                    MenuItemGroup = "Layout2"
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetLayoutRelatedMenus(MenuFilter<int?, string> filter) execution have ended");
            }
        }
        [Test]
        public void GetLayoutRelatedMenusAsync()
        {
            try
            {
                LogEventManager.Logger.Info("GetLayoutRelatedMenusAsync(MenuFilter<int?, string> filter) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetLayoutRelatedMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout"
                }).Result);
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                Assert.DoesNotThrow(() => items = _repository.GetLayoutRelatedMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 2,
                    MenuItemGroup = "Layout"
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 1);
                Assert.DoesNotThrow(() => items = _repository.GetLayoutRelatedMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 3,
                    MenuItemGroup = "Layout2"
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetLayoutRelatedMenusAsync(MenuFilter<int?, string> filter) execution have ended");
            }
        }
        [Test]
        public void GetSubMenus()
        {
            try
            {
                LogEventManager.Logger.Info("GetSubMenus(RouteMenuItem menu) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                var parent = Db.Set<MenuItem>().Single(it => it.Code == "1");
                Assert.DoesNotThrow(() => items = _repository.GetSubMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout",
                    ParentId = parent.Id
                }));
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                parent = Db.Set<MenuItem>().Single(it => it.Code == "2");
                Assert.DoesNotThrow(() => items = _repository.GetSubMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout",
                    ParentId = parent.Id
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                parent = Db.Set<MenuItem>().Single(it => it.Code == "7");
                Assert.DoesNotThrow(() => items = _repository.GetSubMenus(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout",
                    ParentId = parent.Id
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetSubMenus(RouteMenuItem menu) execution have ended");
            }
        }
        [Test]
        public void GetSubMenusAsync()
        {
            try
            {
                LogEventManager.Logger.Info("GetSubMenusAsync(RouteMenuItem menu) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                var parent = Db.Set<MenuItem>().Single(it => it.Code == "1");
                Assert.DoesNotThrow(() => items = _repository.GetSubMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout",
                    ParentId = parent.Id
                }).Result);
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                parent = Db.Set<MenuItem>().Single(it => it.Code == "2");
                Assert.DoesNotThrow(() => items = _repository.GetSubMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout",
                    ParentId = parent.Id
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 2);
                parent = Db.Set<MenuItem>().Single(it => it.Code == "7");
                Assert.DoesNotThrow(() => items = _repository.GetSubMenusAsync(new RoleBasedParentFilter<int?, int?, string>
                {
                    RoleId = 1,
                    MenuItemGroup = "Layout",
                    ParentId = parent.Id
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetSubMenusAsync(RouteMenuItem menu) execution have ended");
            }
        }
        [Test]
        [TestCase(1, "Layout","7",ExpectedResult = true)]
        [TestCase(1, "Layout", "1", ExpectedResult = false)]
        public bool GetParentMenuAsync(int roleId,string menuItemGroup,string childCode )
        {
            try
            {
                LogEventManager.Logger.Info("GetParentMenuAsync(RouteMenuItem childMenu) execution have started");
                RouteMenuItem item = null;
                var child =  Db.Set<MenuItem>().Single(it => it.Code == childCode);
                Assert.DoesNotThrow(() => item = _repository.GetParentMenuAsync(new ChildRoleBasedFilter<int?, int?, string>
                {
                    RoleId = roleId,
                    ChildId = child.Id,
                    MenuItemGroup = menuItemGroup
                }).Result);
                return item != null && item.Id > 0;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetParentMenuAsync(RouteMenuItem childMenu) execution have ended");
            }
        }
        [Test]
        [TestCase(1, "Layout", "7", ExpectedResult = true)]
        [TestCase(1, "Layout", "1", ExpectedResult = false)]
        public bool GetParentMenu(int roleId, string menuItemGroup, string childCode)
        {
            try
            {
                LogEventManager.Logger.Info("GetParentMenu(RouteMenuItem childMenu) execution have started");
                RouteMenuItem item = null;
                var child = Db.Set<MenuItem>().Single(it => it.Code == childCode);
                Assert.DoesNotThrow(() => item = _repository.GetParentMenu(new ChildRoleBasedFilter<int?, int?, string>
                {
                    RoleId = roleId,
                    ChildId = child.Id,
                    MenuItemGroup = menuItemGroup
                }));
                return item != null && item.Id > 0;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetParentMenu(RouteMenuItem childMenu) execution have ended");
            }
        }
        [Test]
        public void GetMenusByNavigation_RoleBased()
        {
            try
            {
                LogEventManager.Logger.Info("GetMenusByNavigation(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigation(new RouteNavigation
                {
                    RouteName = "Default",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"controller","Home"},
                        {"action","Index"}
                    }
                }));
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 3);
                Assert.True(routeMenuItems.Count(it => it.RoleId == 1) == 2);
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigation(new RouteNavigation
                {
                    RouteName = "Default",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"action","Index"}
                    }
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 4);
                Assert.True(routeMenuItems.Count(it => it.RoleId == 1) == 3);
                Assert.True(routeMenuItems.Count(it => it.RoleId == 2) == 1);
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigation(new RouteNavigation
                {
                    RouteName = "Ololo",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"controller","Home"},
                        {"action","Index"}
                    }
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigation(new RouteNavigation
                {
                    RouteName = "Default",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"controller","Home"},
                        {"action","Index"},
                        {"area","marketing" }
                    }
                }));
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetMenusByNavigation(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true) execution have ended");
            }
        }
        [Test]
        public void GetMenusByNavigationAsync()
        {
            try
            {
                LogEventManager.Logger.Info("GetMenusByNavigationAsync(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true) execution have started");
                IEnumerable<RouteMenuItem> items = null;
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigationAsync(new RouteNavigation
                {
                    RouteName = "Default",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"controller","Home"},
                        {"action","Index"}
                    }
                }).Result);
                Assert.NotNull(items);
                var menuItems = items as IList<RouteMenuItem> ?? items.ToList();
                var routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 3);
                Assert.True(routeMenuItems.Count(it => it.RoleId == 1) == 2);
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigationAsync(new RouteNavigation
                {
                    RouteName = "Default",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"action","Index"}
                    }
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());
                Assert.True(routeMenuItems.Count() == 4);
                Assert.True(routeMenuItems.Count(it => it.RoleId == 1) == 3);
                Assert.True(routeMenuItems.Count(it => it.RoleId == 2) == 1);
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigationAsync(new RouteNavigation
                {
                    RouteName = "Ololo",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"controller","Home"},
                        {"action","Index"}
                    }
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.False(routeMenuItems.Any());
                Assert.DoesNotThrow(() => items = _repository.GetMenusByNavigationAsync(new RouteNavigation
                {
                    RouteName = "Default",
                    RouteParameters = new Dictionary<string, object>
                    {
                        {"controller","Home"},
                        {"action","Index"},
                        {"area","marketing" }
                    }
                }).Result);
                Assert.NotNull(items);
                routeMenuItems = items as IList<RouteMenuItem> ?? menuItems.ToList();
                Assert.True(routeMenuItems.Any());

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetMenusByNavigationAsync(Navigation navigationGeneric, int quantity = 5, bool onlyIsActive = true) execution have ended");
            }
        }
    }
}
