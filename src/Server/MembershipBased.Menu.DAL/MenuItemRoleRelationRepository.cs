using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL.Interop;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.Menu;
using odec.Server.Model.User;

namespace odec.Membership.Menu.DAL
{
    public class MenuItemRoleRelationRepository :
        IContextRepository<DbContext>,
        IMenuItemRelationRepository<int, RoleMenuItemRelation, MenuItemRelationRouteValue>
    {
        public MenuItemRoleRelationRepository()
        {
            //Db = new UserBasedMenuContext();
        }
        public RoleMenuItemRelation GetById(int id)
        {
            try
            {
                LogEventManager.Logger.Error("GetById(int id) execution has started");
                return (from menuRelation in Db.Set<RoleMenuItemRelation>()
                    join menuItem in Db.Set<MenuItem>() on menuRelation.MenuItemId equals menuItem.Id
                    join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                    join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on
                        menuRelation.MenuItemRelationGroupId
                        equals menuItemRelationGroup.Id
                    join role in Db.Set<Role>() on menuRelation.RoleId equals role.Id
                    where menuRelation.Id == id
                    select new RoleMenuItemRelation
                    {
                        Role = role,
                        RoleId = menuRelation.RoleId,
                        MenuItem = menuItem,
                        MenuItemRelationGroup = menuItemRelationGroup,
                        RouteName = routeName,
                        Id = menuRelation.Id,
                        MenuItemRelationGroupId = menuRelation.MenuItemRelationGroupId,
                        IsDocked = menuRelation.IsDocked,
                        MenuItemId = menuRelation.MenuItemId,
                        RouteNameId = menuRelation.RouteNameId
                    }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("GetById(int id) execution has finished");
            }
        }
        public void Save(RoleMenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("Save(RoleMenuItemRelation entity) execution has started");
                if (entity.Role != null && entity.Role.Id == 0 && !entity.RoleId.HasValue && !string.IsNullOrEmpty(entity.Role.Name))
                    entity.Role = Db.Set<Role>().Single(it => it.Name.Equals(entity.Role.Name));
                Db.Set<RoleMenuItemRelation>().Add(entity);
                if (Db.Set<RoleMenuItemRelation>().Any(it => it.Id == entity.Id))
                {
                    Db.Entry(entity).State = EntityState.Modified;
                }
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("Save(RoleMenuItemRelation entity) execution has finished");
            }
        }

        public void SaveById(RoleMenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("SaveById(RoleMenuItemRelation entity) execution has started");
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("SaveById(RoleMenuItemRelation entity) execution has finished");
            }
        }

        public void Delete(int id)
        {
            try
            {
                LogEventManager.Logger.Error("Delete(int id) execution has started");
                var entity = GetById(id);
                Delete(entity);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("Delete(int id) execution has finished");
            }
        }

        public void Delete(RoleMenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("Delete(RoleMenuItemRelation entity) execution has started");
              //  Db.Set<RoleMenuItemRelation>().Add(entity);
                var victim = Db.Set<RoleMenuItemRelation>().SingleOrDefault(it=>it.Id ==entity.Id);
                Db.Set<RoleMenuItemRelation>().Remove(victim);
             //   Db.Entry(entity).State = EntityState.Deleted;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("Delete(RoleMenuItemRelation entity) execution has finished");
            }
        }

        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }

        public IEnumerable<RoleMenuItemRelation> Get()
        {
            return (from menuRelation in Db.Set<RoleMenuItemRelation>()
                join menuItem in Db.Set<MenuItem>() on menuRelation.MenuItemId equals menuItem.Id
                join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId
                    equals menuItemRelationGroup.Id
                join role in Db.Set<Role>() on menuRelation.RoleId equals role.Id
                select new RoleMenuItemRelation
                {
                    Role = role,
                    RoleId= menuRelation.RoleId,
                    MenuItem = menuItem,
                    MenuItemRelationGroup = menuItemRelationGroup,
                    RouteName = routeName,
                    Id = menuRelation.Id,
                    MenuItemRelationGroupId = menuRelation.MenuItemRelationGroupId,
                    IsDocked = menuRelation.IsDocked,
                    MenuItemId = menuRelation.MenuItemId,
                    RouteNameId = menuRelation.RouteNameId
                }).ToList();
        }


        public void AddRouteParameter(MenuItemRelationRouteValue menuRelationRouteParam)
        {
            try
            {
                LogEventManager.Logger.Error("AddRouteParameter(MenuItemRelationRouteValue menuRelationRouteParam) execution has started");

                Db.Set<MenuItemRelationRouteValue>().Add(menuRelationRouteParam);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("AddRouteParameter(MenuItemRelationRouteValue menuRelationRouteParam) execution has finished");
            }
        }
        public void RemoveRouteParameter(MenuItemRelationRouteValue menuRelationRouteParam)
        {
            RemoveRouteParameter(menuRelationRouteParam.MenuItemRelationId, menuRelationRouteParam.RouteParamId);
        }
        public void RemoveRouteParameter(int menuItemRelationId, int routeParamId)
        {
            try
            {
                LogEventManager.Logger.Error("RemoveRouteParameter(int menuItemRelationId, int routeParamId) execution has started");
                var entity = Db.Set<MenuItemRelationRouteValue>().Single(it => it.RouteParamId == routeParamId && it.MenuItemRelationId == menuItemRelationId);
                Db.Set<MenuItemRelationRouteValue>().Remove(entity);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("RemoveRouteParameter(int menuItemRelationId, int routeParamId) execution has finished");
            }
        }
    }
}
