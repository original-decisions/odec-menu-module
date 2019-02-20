using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL;
using odec.Menu.DAL.Interop;
using odec.Menu.ViewModels;
using odec.Menu.ViewModels.Specific;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.Menu;
using odec.Server.Model.Menu.Context;
using odec.Server.Model.User;

namespace odec.Menu.WebUI.Controllers
{
    //[Route(Name = "Admin")]
    public class MenuAdminController : Controller
    {
        public MenuAdminController(MenuContext db)
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
        private readonly IRouteNameRepository<int, RouteName> _routeNameRepository = new RouteNameRepository();
          //  IocHelper.GetObject<IRouteNameRepository<int, RouteName>>();

        private readonly IRouteParamRepository<int, RouteParam, MenuItemRelationRouteValue> _routeParamRepository =
            new RouteParamRepository();
          //  IocHelper.GetObject<IRouteParamRepository<int, RouteParam, MenuItemRelationRouteValue>>();

        private IMenuItemRelationRepository<int, MenuItemRelation, MenuItemRelationRouteValue> _menuRelationRepository =
            new MenuItemRelationRepository();
         //   IocHelper.GetObject<IMenuItemRelationRepository<int, MenuItemRelation, MenuItemRelationRouteValue>>();
        private readonly IMenuItemRelationGroupRepository<int, MenuItemRelationGroup> _menuItemRelationGroupRepository =
            new MenuItemRelationGroupRepository();
        //    IocHelper.GetObject<IMenuItemRelationGroupRepository<int, MenuItemRelationGroup>>();

        private readonly IMenuItemRepository<int, MenuItem> _menuItemRepository = 
            new MenuItemRepository();
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
        public ActionResult GetRouteNames(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _routeNameRepository.Get().ToList();
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
                return Json(null);
            }
            finally
            {

            }

            //return Json(_routeNameRepository.Get(),JsonRequestBehavior.AllowGet);
            //return View(_routeNameRepository.Get());
        }
        public ActionResult GetRouteParams(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _routeParamRepository.Get().ToList();
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
                return Json(null);
            }
            finally
            {

            }

            //return Json(_routeParamRepository.Get(),JsonRequestBehavior.AllowGet);
            // return View(_routeParamRepository.Get());
        }

        public ActionResult GetMenuRelationGroups(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _menuItemRelationGroupRepository.Get().ToList();
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
                return Json(null);
            }
            finally
            {

            }

            //return Json(_menuItemRelationGroupRepository.Get(),JsonRequestBehavior.AllowGet);
            //return View(_menuItemRelationGroupRepository.Get());
        }

        public ActionResult GetMenuItems(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _menuItemRepository.Get().ToList();
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
                return Json(null);
            }
            finally
            {

            }

            //return View(_menuItemRepository.Get());
        }

        public ActionResult SaveMenuItem(MenuItemVm<int, int?> vm)
        {
            try
            {
                _menuItemRepository.SaveById(new MenuItem
                {
                    Id = vm.Id,
                    Code = vm.Code,
                    IsActive = vm.IsActive,
                    Name = vm.Name,
                    SortOrder = vm.SortOrder,
                    DateCreated = DateTime.Now,
                    ParentId = vm.ParentId
                });
                return Json(new { success = true });
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


        public ActionResult AddRouteParameters(MenuItemRelationRouteValueVm<int> vm)
        {
            foreach (var param in vm.RouteParameters)
            {
                _routeParamRepository.AddToRoute(new MenuItemRelationRouteValue
                {
                    MenuItemRelationId = vm.RouteNameId,
                    RouteParamId = param.Id
                }, new RouteParam
                {
                    Id = param.Id,
                    Code = param.Code,
                    IsActive = param.IsActive,
                    Name = param.Name,
                    DateCreated = DateTime.Today,
                    SortOrder = param.SortOrder
                });
            }

            return View();
        }
        public ActionResult DeleteParamFromMenuItemRoute(int menuItemrelationId, int paramId)
        {
            _routeParamRepository.DeleteFromRoute(menuItemrelationId, paramId);
            return View();
        }

        [HttpPost]
        public ActionResult SaveRouteParamName(RouteParameterVm<int> vm)
        {
            try
            {
                _routeParamRepository.SaveById(new RouteParam
                {
                    Id = vm.Id,
                    Code = vm.Code,
                    IsActive = vm.IsActive,
                    Name = vm.Name,
                    SortOrder = vm.SortOrder,
                    DateCreated = DateTime.Now,
                });
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
        [HttpPost]
        public ActionResult SaveMenuItemRelationGroup(MenuItemRelationGroupVm<int> vm)
        {
            try
            {
                _menuItemRelationGroupRepository.SaveById(new MenuItemRelationGroup
                {
                    Id = vm.Id,
                    Code = vm.Code,
                    IsActive = vm.IsActive,
                    Name = vm.Name,
                    SortOrder = vm.SortOrder,
                    DateCreated = DateTime.Now,
                });
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
        [HttpPost]
        public ActionResult SaveRouteName(RouteNameVm<int> vm)
        {
            try
            {
                _routeNameRepository.SaveById(new RouteName
                {
                    Id = vm.Id,
                    Code = vm.Code,
                    IsActive = vm.IsActive,
                    Name = vm.Name,
                    SortOrder = vm.SortOrder,
                    DateCreated = DateTime.Now,
                });
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



        public ActionResult GetMenuRelations(int page, int rows, string sidx, string sord)
        {
            try
            {
                var result = _menuRelationRepository.Get().ToList().Select(it => new
                {
                    Id = it.Id,
                    IsDocked = it.IsDocked,
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

        public ActionResult SaveMenuItemRelation(MenuItemRelationAdminVm<int> vm, string oper)
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
                var result = _routeParamRepository.GetRouteParams().ToList();

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

        public ActionResult SaveMenuItemRelationParam(MenuItemRelationRouteParamVm<int> vm, string oper)
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

                //_menuRelationRepository.RemoveRouteParameter(menuRelationRouteParam);
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
