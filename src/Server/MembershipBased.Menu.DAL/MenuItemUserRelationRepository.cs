using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL.Interop;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.Menu;
using odec.Server.Model.User;

namespace odec.Membership.Menu.DAL
{
    public class MenuItemUserRelationRepository :// OrmEntityOperationsRepository<int,, DbContext>,
    IContextRepository<DbContext>,
        IMenuItemRelationRepository<int, UserMenuItemRelation, MenuItemRelationRouteValue>
    {
        //TODO:add sub-repos for simple actions. Like repository about route Param
        public MenuItemUserRelationRepository()
        {
            // Db = new UserBasedMenuContext();
        }
        public MenuItemUserRelationRepository(DbContext db)
        {
            Db = db;
        }
        public UserMenuItemRelation GetById(int id)
        {
            try
            {
                LogEventManager.Logger.Error("GetById(int id) execution has started");
                return (from menuRelation in Db.Set<UserMenuItemRelation>()
                        join menuItem in Db.Set<MenuItem>() on menuRelation.MenuItemId equals menuItem.Id
                        join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                        join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on
                            menuRelation.MenuItemRelationGroupId
                            equals menuItemRelationGroup.Id
                        join usr in Db.Set<User>() on menuRelation.UserId equals usr.Id
                        where menuRelation.Id == id
                        select new UserMenuItemRelation
                        {
                            User = usr,
                            UserId = menuRelation.UserId,
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

        public void Save(UserMenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("Save(UserMenuItemRelation entity) execution has started");
                if (entity.User != null && entity.User.Id == 0 && entity.UserId == 0 && !string.IsNullOrEmpty(entity.User.UserName))
                    entity.User = Db.Set<User>().Single(it => it.UserName.Equals(entity.User.UserName));

                //AddOrUpdate(entity, e => e.Id == entity.Id);
                //if (Db.Entry(entity).State ==EntityState.Detached)
                //{
                //    Db.Attach(entity);
                //}
                
                
                if (Db.Set<UserMenuItemRelation>().Any(it => it.Id == entity.Id))
                {
                    Db.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    Db.Set<UserMenuItemRelation>().Add(entity);
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
                LogEventManager.Logger.Error("Save(UserMenuItemRelation entity) execution has finished");
            }
        }

        public void SaveById(UserMenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("SaveById(UserMenuItemRelation entity) execution has started");
                Save(entity);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("SaveById(UserMenuItemRelation entity) execution has finished");
            }
        }

        public void Delete(int id)
        {
            try
            {
                LogEventManager.Logger.Error("Delete(int id) execution has started");
                var entity = Db.Set<UserMenuItemRelation>().SingleOrDefault(it=> it.Id==id);
                if (entity ==null) return; 
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

        public void Delete(UserMenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("Delete(UserMenuItemRelation entity) execution has started");
                //Db.Set<UserMenuItemRelation>().Add(entity);
                var relatedParams = Db.Set<MenuItemRelationRouteValue>().Where(it => it.MenuItemRelationId == entity.Id);
                foreach (var menuItemRelationRouteValue in relatedParams)
                    RemoveRouteParameter(menuItemRelationRouteValue);
                //Db.ChangeTracker.DetectChanges();
                Db.Entry(entity).State = EntityState.Deleted;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("Delete(UserMenuItemRelation entity) execution has finished");
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

        public IEnumerable<UserMenuItemRelation> Get()
        {
            return ((from menuRelation in Db.Set<UserMenuItemRelation>()
                     join menuItem in Db.Set<MenuItem>() on menuRelation.MenuItemId equals menuItem.Id
                     join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                     join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on
                         menuRelation.MenuItemRelationGroupId
                         equals menuItemRelationGroup.Id
                     join usr in Db.Set<User>() on menuRelation.UserId equals usr.Id
                     select new UserMenuItemRelation
                     {
                         User = usr,
                         UserId = menuRelation.UserId,
                         MenuItem = menuItem,
                         MenuItemRelationGroup = menuItemRelationGroup,
                         RouteName = routeName,
                         Id = menuRelation.Id,
                         MenuItemRelationGroupId = menuRelation.MenuItemRelationGroupId,
                         IsDocked = menuRelation.IsDocked,
                         MenuItemId = menuRelation.MenuItemId,
                         RouteNameId = menuRelation.RouteNameId
                     })).ToList();
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
