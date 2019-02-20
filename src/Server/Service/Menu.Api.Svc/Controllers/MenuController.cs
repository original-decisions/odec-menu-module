using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using odec.Membership.Menu.DAL;
using odec.Menu.ViewModels;
using odec.Menu.ViewModels.Specific;
using odec.Server.Model.Menu.Filters;
using IMenuRepo = odec.Menu.DAL.Interop.IMenuRepository
                <int, odec.Server.Model.Menu.Specific.RouteMenuItem, odec.Server.Model.Menu.Abstractions.Navigation, odec.Server.Model.Menu.Abstractions.Filters.MenuParentFilter<int?, string>, odec.Server.Model.Menu.Abstractions.Filters.MenuChildFilter<int?, string>>;
namespace odec.Menu.WebUI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuRepo _repository;

        public MenuController()
        {
            _repository = new RoleBasedMenuRepository();//(IMenuRepo)AppContext.GetObjects<IMenuRepo>().FirstOrDefault(it => it.Key == "RoleBasedMenuRepository").Value;
        }
        public async Task<ActionResult> Index(string menuItemGroup = null)
        {
            var result = await _repository.GetLayoutRelatedMenusAsync(new RoleBasedParentFilter<int?, int?, string> { MenuItemGroup = menuItemGroup });
            return View(Mapper.Map<IEnumerable<RouteMenuItemVm<int, int?>>>(result));
        }
        // GET: Menu
        //public async Task<ActionResult> GetMenuItems(string menuItemGroup = null)
        //{
        //    var result = await _repository.GetLayoutRelatedMenusAsync(new RoleBasedParentFilter<int, int?, string> { MenuItemGroup = menuItemGroup });
        //    return View("_bootstrapMenu", Mapper.Map<ICollection<RouteMenuItemVm<int>>>(result));
        //}

        public async Task<ActionResult> GetMenuItems(MenuFilterVm<string,int?,int?,int?> filter)
        {
            var result = await _repository.GetMenuItemsAsync(new RoleBasedParentFilter<int?, int?, string>
            {
                MenuItemGroup = filter.MenuItemGroup,
                RoleId = filter.RoleId,
                InitHierarchy = filter.InitHierarchy,
                ParentId = filter.ParentId
                
            });
            //var result = await _repository.GetLayoutRelatedMenusAsync(new RoleBasedParentFilter<int?, int?, string>
            //{
            //    MenuItemGroup = filter.MenuItemGroup,
            //    RoleId = filter.RoleId
            //});
            
            return View("_bootstrapMenu", Mapper.Map<ICollection<RouteMenuItemVm<int,int?>>>(result));
        }

    }
}