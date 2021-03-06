﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Membership.Menu.DAL;
using odec.Menu.DAL;
using odec.Menu.DAL.Interop;
using odec.Menu.ViewModels;
using odec.Menu.ViewModels.Specific;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.MembershipMenu.Context;
using odec.Server.Model.Menu;
using odec.Server.Model.User;
using UserE = odec.Server.Model.User.User;
namespace odec.Menu.WebUI.Controllers
{
    // [Authorize(Roles = "admin")]
    
    public class MenuUserAdminController : Controller
    {
        public MenuUserAdminController(UserBasedMenuContext db)
        {
            InitCtx(_routeNameRepository as IContextRepository<DbContext>, db);
            InitCtx(_routeParamRepository as IContextRepository<DbContext>, db);
            InitCtx(_menuRelationRepository as IContextRepository<DbContext>, db);
            InitCtx(_menuItemRelationGroupRepository as IContextRepository<DbContext>, db);
            InitCtx(_menuItemRepository as IContextRepository<DbContext>, db);
        }

        public void InitCtx(IContextRepository<DbContext> repository,DbContext db)
        {
            if (repository == null)
                throw new InvalidCastException("No db Context used");
            repository.SetContext(db);
        }

        private readonly IRouteNameRepository<int, RouteName> _routeNameRepository =new RouteNameRepository();
           // IocHelper.GetObject<IRouteNameRepository<int, RouteName>>();

        private readonly IRouteParamRepository<int, RouteParam, MenuItemRelationRouteValue> _routeParamRepository = new RouteParamRepository();
            //IocHelper.GetObject<IRouteParamRepository<int, RouteParam, MenuItemRelationRouteValue>>();

        private IMenuItemRelationRepository<int, UserMenuItemRelation,MenuItemRelationRouteValue> _menuRelationRepository = new MenuItemUserRelationRepository();
           // IocHelper.GetObject<IMenuItemRelationRepository<int, UserMenuItemRelation, MenuItemRelationRouteValue>>();
        private readonly IMenuItemRelationGroupRepository<int, MenuItemRelationGroup> _menuItemRelationGroupRepository = new MenuItemRelationGroupRepository();
          //  IocHelper.GetObject<IMenuItemRelationGroupRepository<int, MenuItemRelationGroup>>();

        private readonly IMenuItemRepository<int, MenuItem> _menuItemRepository = new MenuItemRepository();
          //  IocHelper.GetObject<IMenuItemRepository<int, MenuItem>>();

        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                return View(new MenuAdminIndexVm<int, int?>());
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }

        }

        public ActionResult GetRoles()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }

        }
        

        public ActionResult GetMenuRelations(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _menuRelationRepository.Get().ToList().Select(it => new
                {
                    Id = it.Id,
                    IsDocked = it.IsDocked,
                    IsPublic = !it.UserId.HasValue,
                    Login = it.User == null ? "Is public menu" : it.User.UserName,
                    MenuCode = it.MenuItem.Code,
                    RouteCode = it.RouteName.Code,
                    MenuItemRelationGroupCode = it.MenuItemRelationGroup.Code
                }).ToList();

                var totalRowCount = result.Count;

                return Json(new
                {
                    page = page,
                    total = Math.Ceiling((decimal)totalRowCount / rows),
                    records = totalRowCount,
                    rows = result
                });
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                return Json(new { success = false });
            }
            finally
            {

            }

        }

        public ActionResult SaveMenuItemRelation(MenuItemRelationAdminVm<int> vm,string oper)
        {
            try
            {
                if (oper.Equals("del"))
                {
                    _menuRelationRepository.Delete(vm.Id);
                    return Json(new { success = 1 });
                }
                var menuItem = _menuItemRepository.Get()
                    .Single(it => it.Code.Equals(vm.MenuCode, StringComparison.OrdinalIgnoreCase));
                var routeName = _routeNameRepository.Get().Single(it => it.Code.Equals(vm.RouteCode, StringComparison.OrdinalIgnoreCase));
                var relationGroup = _menuItemRelationGroupRepository.Get().Single(it => it.Code.Equals(vm.MenuItemRelationGroupCode, StringComparison.OrdinalIgnoreCase));
                var menuRelation = new UserMenuItemRelation
                {
                    Id = vm.Id,
                    IsDocked = vm.IsDocked,
                    RouteNameId = routeName.Id,
                    MenuItemRelationGroupId = relationGroup.Id,
                    MenuItemId = menuItem.Id,
                };
                if (vm.Login != "Is public menu" && !string.IsNullOrEmpty(vm.Login))
                    menuRelation.User = new User
                    {
                        UserName = vm.Login
                    };
               
                _menuRelationRepository.Save(menuRelation);
                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                return Json(new { success = false });
            }
            finally
            {

            }
        }

        public ActionResult GetMenuItemRelationParams(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _routeParamRepository.GetRouteParams().ToList();//.Select(it=> new
                //{
                //    it.MenuItemRelation,
                //    it.MenuItemRelationId,
                //    it.RouteParam,
                //    it.RouteParamId,
                //    it.Value,
                //    Info = "Menu Item Name: " + it.MenuItemRelation.MenuItem.Name + " for user -" + it.MenuItemRelation.  + "<br/>"
                //        +
                //});

                var totalRowCount = result.Count;

                return Json(new
                {
                    page = page,
                    total = Math.Ceiling((decimal)totalRowCount / rows),
                    records = totalRowCount,
                    rows = result
                });
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                return Json(new { success = false });
            }
            finally
            {

            }
        }

        public ActionResult SaveMenuItemRelationParam(MenuItemRelationRouteParamVm<int> vm,string oper)
        {
            try
            {
                if (!string.IsNullOrEmpty(vm.RouteParamCode) && (vm.RouteParam == null || string.IsNullOrEmpty(vm.RouteParam.Code)))
                    if (vm.RouteParam == null)
                        vm.RouteParam = new RouteParameterVm<int>
                        {
                            Code = vm.RouteParamCode
                        };
                    else
                        vm.RouteParam.Code = vm.RouteParamCode;
                var routeParamName = _routeParamRepository.Get()
                     .Single(it => it.Code.Equals(vm.RouteParam.Code, StringComparison.OrdinalIgnoreCase));
                var menuRelationRouteParam = new MenuItemRelationRouteValue
                {
                    MenuItemRelationId = vm.MenuItemRelationId,
                    RouteParamId = routeParamName.Id,
                    Value = vm.Value
                };
                if (oper.Equals("del"))
                {
                    _menuRelationRepository.RemoveRouteParameter(menuRelationRouteParam);
                    return Json(new { success = 1 });
                }
                _menuRelationRepository.AddRouteParameter(menuRelationRouteParam);
                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                return Json(new { success = false });
            }
            finally
            {

            }
        }
    }
}