using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL.Interop;
using odec.Server.Model.Menu;

namespace odec.Menu.DAL
{
    public class MenuItemRelationRepository : IContextRepository<DbContext>, IMenuItemRelationRepository<int, MenuItemRelation, MenuItemRelationRouteValue>
    {
        public MenuItemRelationRepository()
        {
        }
        public MenuItemRelation GetById(int id)
        {
            try
            {
                LogEventManager.Logger.Error("GetById(int id) execution has started");
                return (from menuRelation in Db.Set<MenuItemRelation>()
                        join menuItem in Db.Set<MenuItem>() on menuRelation.MenuItemId equals menuItem.Id
                        join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                        join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id
                        where menuRelation.Id == id
                        select new MenuItemRelation
                        {
                            MenuItem = menuItem,
                            MenuItemRelationGroup = menuItemRelationGroup,
                            RouteName = routeName,
                            Id = menuRelation.Id,
                            MenuItemRelationGroupId = menuRelation.MenuItemRelationGroupId,
                            IsDocked = menuRelation.IsDocked,
                            MenuItemId = menuRelation.MenuItemId,
                            RouteNameId = menuRelation.RouteNameId
                        }
                ).SingleOrDefault();

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

        public void Save(MenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("Save(MenuItemRelation entity) execution has started");

                Db.Set<MenuItemRelation>().Add(entity);
                if (Db.Set<MenuItemRelation>().Any(it => it.Id == entity.Id))
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
                LogEventManager.Logger.Error("Save(MenuItemRelation entity) execution has finished");
            }
        }

        public void SaveById(MenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("SaveById(MenuItemRelation entity) execution has started");
                Save(entity);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("SaveById(MenuItemRelation entity) execution has finished");
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

        public void Delete(MenuItemRelation entity)
        {
            try
            {
                LogEventManager.Logger.Error("Delete(MenuItemRelation entity) execution has started");
                var victim = Db.Set<MenuItemRelation>().SingleOrDefault(it => entity.Id==it.Id);
                if (victim ==null)
                {
                    return;
                }
                Db.Set<MenuItemRelation>().Remove(victim);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("Delete(MenuItemRelation entity) execution has finished");
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

        public IEnumerable<MenuItemRelation> Get()
        {
            try
            {
                LogEventManager.Logger.Error("Get() execution has started");
                return (from menuRelation in Db.Set<MenuItemRelation>()
                        join menuItem in Db.Set<MenuItem>() on menuRelation.MenuItemId equals menuItem.Id
                        join routeName in Db.Set<RouteName>() on menuRelation.RouteNameId equals routeName.Id
                        join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on menuRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id
                        select new MenuItemRelation
                        {
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
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Error("Get() execution has finished");
            }

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