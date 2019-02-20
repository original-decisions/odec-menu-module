using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Framework.Logging;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.Menu;
using odec.Server.Model.User;

namespace Menu.DAL.Tests
{
    internal static class MenuTestHelper
    {
        internal static void PopulateDefaultMenuData(DbContext db)
        {
            try
            {
                var home = new MenuItem
                {
                    Code = "1",
                    Name = "Home",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItem>().Add(home);
                var admin= new MenuItem
                {
                    Code = "2",
                    Name = "Admin",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItem>().Add(admin);
                var roles = new MenuItem
                {
                    Code = "6",
                    Name = "Roles",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = admin.Id

                };
                db.Set<MenuItem>().Add(roles);
                var about = new MenuItem
                {
                    Code = "7",
                    Name = "About",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = home.Id

                };
                db.Set<MenuItem>().Add(about);
                var defaultRoute = new RouteName
                {
                    Code = "1",
                    Name = "Default",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteName>().Add(defaultRoute);
                var controllerParam = new RouteParam
                {
                    Code = "1",
                    Name = "controller",
                    // DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(controllerParam);
                var actionParam = new RouteParam
                {
                    Code = "2",
                    Name = "action",
                    // DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(actionParam);
                var areaParam= new RouteParam
                {
                    Code = "3",
                    Name = "area",
                    // DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(areaParam);
                var relGroup = new MenuItemRelationGroup
                {
                    Code = "1",
                    Name = "Layout",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItemRelationGroup>().Add(relGroup);
                var homeRel = new MenuItemRelation
                {
                    MenuItemId = home.Id,
                    MenuItemRelationGroupId = relGroup.Id,
                    RouteNameId = defaultRoute.Id
                };
                db.Set<MenuItemRelation>().Add(homeRel);
                var adminRel = new MenuItemRelation
                {
                    MenuItemId = admin.Id,
                    MenuItemRelationGroupId = relGroup.Id,
                    RouteNameId = defaultRoute.Id
                };
                db.Set<MenuItemRelation>().Add(adminRel);
                var aboutRel = new MenuItemRelation
                {
                    MenuItemId = about.Id,
                    MenuItemRelationGroupId = relGroup.Id,
                    RouteNameId = defaultRoute.Id
                };
                db.Set<MenuItemRelation>().Add(aboutRel);
                var rolesRel = new MenuItemRelation
                {
                    MenuItemId = roles.Id,
                    MenuItemRelationGroupId = relGroup.Id,
                    RouteNameId = defaultRoute.Id
                };
                db.Set<MenuItemRelation>().Add(rolesRel);
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controllerParam.Id,
                    Value = "Home",
                    MenuItemRelationId= homeRel.Id
                });


                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = actionParam.Id,
                    Value = "Index",
                    MenuItemRelationId= homeRel.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controllerParam.Id,
                    Value = "Admin",
                    MenuItemRelationId= adminRel.Id
                });

                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controllerParam.Id,
                    Value = "Admin",
                    MenuItemRelationId= rolesRel.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = actionParam.Id,
                    Value = "Users",
                    MenuItemRelationId= rolesRel.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = actionParam.Id,
                    Value = "Index",
                    MenuItemRelationId= adminRel.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = areaParam.Id,
                    Value = "marketing",
                    MenuItemRelationId= adminRel.Id
                });
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message,ex);
                throw;
            }
        }

        internal static void PopulateDefaultMenuDataUserCtx(DbContext db)
        {
            try
            {
                var home = new MenuItem
                {
                    //Id = 1,
                    Code = "1",
                    Name = "Home",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItem>().Add(home);
                var admin = new MenuItem
                {
                    //Id = 2,
                    Code = "2",
                    Name = "Admin",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItem>().Add(admin);
                var roles =
                new MenuItem
                {
                    //Id = 5,
                    Code = "6",
                    Name = "Roles",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = admin.Id

                };
                db.Set<MenuItem>().Add(roles);
                var about = new MenuItem
                {
                    //Id = 7,
                    Code = "7",
                    Name = "About",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = home.Id

                };
                db.Set<MenuItem>().Add(about);
                var marketing = new MenuItem
                {
                    //Id = 7,
                    Code = "10",
                    Name = "Marketing",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = home.Id

                };
                db.Set<MenuItem>().Add(marketing);
                var users = new MenuItem
                {
                    //Id = 6,
                    Code = "6",
                    Name = "Users",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = admin.Id

                };
                db.Set<MenuItem>().Add(users);
                var andrew = new User
                {
                    Id = 1,
                    UserName = "Andrew",

                };
                db.Set<User>().Add(andrew);
                var alex = new User
                {
                    Id = 2,
                    UserName = "Alex",
                };
                db.Set<User>().Add(alex);
                var defaultName = new RouteName
                {
                    // Id = 1,
                    Code = "1",
                    Name = "Default",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteName>().Add(defaultName);
                var controller = new RouteParam
                {
                    // Id = 1,
                    Code = "1",
                    Name = "controller",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(controller);
                var action = new RouteParam
                {
                    // Id = 2,
                    Code = "2",
                    Name = "action",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(action);
                var area = new RouteParam
                {
                    // Id = 3,
                    Code = "3",
                    Name = "area",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(area);
                var layoutRelationGroup = new MenuItemRelationGroup
                {
                    //  Id = 1,
                    Code = "1",
                    Name = "Layout",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItemRelationGroup>().Add(layoutRelationGroup);
                var homeMenuRelation = new UserMenuItemRelation
                {
                    //  Id = 1,
                    MenuItemId = home.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = andrew.Id
                };
                db.Set<UserMenuItemRelation>().Add(homeMenuRelation);
                var homeAlexRelation = new UserMenuItemRelation
                {
                    //  Id = 2,
                    MenuItemId = home.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = alex.Id
                };
                db.Set<UserMenuItemRelation>().Add(homeAlexRelation);
                var adminAndrewRelation = new UserMenuItemRelation
                {
                    // Id = 3,
                    MenuItemId = admin.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = andrew.Id
                };
                db.Set<UserMenuItemRelation>().Add(adminAndrewRelation);
                var aboutAndrewRelation = new UserMenuItemRelation
                {
                    // Id = 4,
                    MenuItemId = about.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = andrew.Id
                };
                db.Set<UserMenuItemRelation>().Add(aboutAndrewRelation);
                var aboutAlexRelation = new UserMenuItemRelation
                {
                    // Id = 4,
                    MenuItemId = about.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = alex.Id
                };
                db.Set<UserMenuItemRelation>().Add(aboutAlexRelation);
                var usersAndrewRelation = new UserMenuItemRelation
                {
                    // Id = 6,
                    MenuItemId = users.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = andrew.Id
                };
                db.Set<UserMenuItemRelation>().Add(usersAndrewRelation);
                var rolesAndrewRelation = new UserMenuItemRelation
                {
                    //Id = 7,
                    MenuItemId = roles.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = andrew.Id
                };
                db.Set<UserMenuItemRelation>().Add(rolesAndrewRelation);
                var andrewMarketing = new UserMenuItemRelation
                {
                    MenuItemId = marketing.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    UserId = andrew.Id
                };
                db.Set<UserMenuItemRelation>().Add(andrewMarketing);

               
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = andrewMarketing.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = andrewMarketing.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = area.Id,
                    Value = "marketing",
                    MenuItemRelationId = andrewMarketing.Id
                });

                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = homeMenuRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = homeAlexRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = aboutAndrewRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "About",
                    MenuItemRelationId = aboutAndrewRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = homeAlexRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = homeMenuRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Admin",
                    MenuItemRelationId = adminAndrewRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Admin",
                    MenuItemRelationId = rolesAndrewRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Roles",
                    MenuItemRelationId = rolesAndrewRelation.Id
                });

                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Admin",
                    MenuItemRelationId = usersAndrewRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Users",
                    MenuItemRelationId = usersAndrewRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = adminAndrewRelation.Id
                });
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        internal static void PopulateDefaultMenuDataRoleCtx(DbContext db)
        {
            try
            {
                var home = new MenuItem
                {
                    //Id = 1,
                    Code = "1",
                    Name = "Home",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItem>().Add(home);
                var admin = new MenuItem
                {
                    //Id = 2,
                    Code = "2",
                    Name = "Admin",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItem>().Add(admin);
                var roles =
                new MenuItem
                {
                    //Id = 5,
                    Code = "6",
                    Name = "Roles",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = admin.Id

                };
                db.Set<MenuItem>().Add(roles);
                var about = new MenuItem
                {
                    //Id = 7,
                    Code = "7",
                    Name = "About",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = home.Id

                };
                db.Set<MenuItem>().Add(about);
                var marketing = new MenuItem
                {
                    //Id = 7,
                    Code = "10",
                    Name = "Marketing",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = home.Id

                };
                db.Set<MenuItem>().Add(marketing);
                var users = new MenuItem
                {
                    //Id = 6,
                    Code = "6",
                    Name = "Users",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0,
                    ParentId = admin.Id

                };
                db.Set<MenuItem>().Add(users);
                var roleAdmin = new Role
                {
                    Id = 1,
                    Name = "Admin",

                };
                db.Set<Role>().Add(roleAdmin);
                var roleUser = new Role
                {
                    Id = 2,
                    Name = "User",
                };
                db.Set<Role>().Add(roleUser);
                var defaultName = new RouteName
                {
                    // Id = 1,
                    Code = "1",
                    Name = "Default",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteName>().Add(defaultName);
                var controller = new RouteParam
                {
                    // Id = 1,
                    Code = "1",
                    Name = "controller",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(controller);
                var action = new RouteParam
                {
                    // Id = 2,
                    Code = "2",
                    Name = "action",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(action);
                var area = new RouteParam
                {
                    // Id = 3,
                    Code = "3",
                    Name = "area",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<RouteParam>().Add(area);
                var layoutRelationGroup = new MenuItemRelationGroup
                {
                    //  Id = 1,
                    Code = "1",
                    Name = "Layout",
                    DateCreated = DateTime.Today,
                    IsActive = true,
                    SortOrder = 0
                };
                db.Set<MenuItemRelationGroup>().Add(layoutRelationGroup);
                var homeAdminMenuRelation = new RoleMenuItemRelation
                {
                    //  Id = 1,
                    MenuItemId = home.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleAdmin.Id
                };
                db.Set<RoleMenuItemRelation>().Add(homeAdminMenuRelation);
                var homeUsrRelation = new RoleMenuItemRelation
                {
                    //  Id = 2,
                    MenuItemId = home.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleUser.Id
                };
                db.Set<RoleMenuItemRelation>().Add(homeUsrRelation);
                var adminAdminRelation = new RoleMenuItemRelation
                {
                    // Id = 3,
                    MenuItemId = admin.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleAdmin.Id
                };
                db.Set<RoleMenuItemRelation>().Add(adminAdminRelation);
                var aboutAdminRelation = new RoleMenuItemRelation
                {
                    // Id = 4,
                    MenuItemId = about.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleAdmin.Id
                };
                db.Set<RoleMenuItemRelation>().Add(aboutAdminRelation);
                var aboutUserRelation = new RoleMenuItemRelation
                {
                    // Id = 4,
                    MenuItemId = about.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleUser.Id
                };
                db.Set<RoleMenuItemRelation>().Add(aboutUserRelation);
                var usersAdminRelation = new RoleMenuItemRelation
                {
                    // Id = 6,
                    MenuItemId = users.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleAdmin.Id
                };
                db.Set<RoleMenuItemRelation>().Add(usersAdminRelation);
                var rolesAdminRelation = new RoleMenuItemRelation
                {
                    //Id = 7,
                    MenuItemId = roles.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleAdmin.Id
                };
                db.Set<RoleMenuItemRelation>().Add(rolesAdminRelation);
                var adminMarketing = new RoleMenuItemRelation
                {
                    MenuItemId = marketing.Id,
                    MenuItemRelationGroupId = layoutRelationGroup.Id,
                    RouteNameId = defaultName.Id,
                    RoleId = roleAdmin.Id
                };
                db.Set<RoleMenuItemRelation>().Add(adminMarketing);


                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = adminMarketing.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = adminMarketing.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = area.Id,
                    Value = "marketing",
                    MenuItemRelationId = adminMarketing.Id
                });

                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = homeAdminMenuRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = homeUsrRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Home",
                    MenuItemRelationId = aboutAdminRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "About",
                    MenuItemRelationId = aboutAdminRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = homeUsrRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = homeAdminMenuRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Admin",
                    MenuItemRelationId = adminAdminRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Admin",
                    MenuItemRelationId = rolesAdminRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Roles",
                    MenuItemRelationId = rolesAdminRelation.Id
                });

                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = controller.Id,
                    Value = "Admin",
                    MenuItemRelationId = usersAdminRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Users",
                    MenuItemRelationId = usersAdminRelation.Id
                });
                db.Set<MenuItemRelationRouteValue>().Add(new MenuItemRelationRouteValue
                {
                    RouteParamId = action.Id,
                    Value = "Index",
                    MenuItemRelationId = adminAdminRelation.Id
                });
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
