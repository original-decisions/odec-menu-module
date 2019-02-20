using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL.Interop;
using odec.Server.Model.Menu;

namespace odec.Menu.DAL
{
    public class RouteParamRepository:OrmEntityOperationsRepository<int,RouteParam,DbContext>, IContextRepository<DbContext>,
        IRouteParamRepository<int,RouteParam,MenuItemRelationRouteValue>
    {
        public IEnumerable<RouteParam> Get()
        {
            try
            {
                LogEventManager.Logger.Info("Get() execution has started");
                
                    return Db.Set<RouteParam>().ToList();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("Get() execution has ended");
            }
        }

        public IEnumerable<MenuItemRelationRouteValue> GetRouteParams()
        {

            try
            {
                LogEventManager.Logger.Info("GetRouteParams() execution has started");

                return (from menuRelationParam in Db.Set<MenuItemRelationRouteValue>()
                    join menuItemRelation in Db.Set<MenuItemRelation>() on menuRelationParam.MenuItemRelationId equals
                        menuItemRelation.Id
                    join menuItem in Db.Set<MenuItem>() on menuItemRelation.MenuItemId equals menuItem.Id
                        join routeName in Db.Set<RouteName>() on menuItemRelation.RouteNameId equals routeName.Id
                        join routeParam in Db.Set<RouteParam>() on menuRelationParam.RouteParamId equals routeParam.Id
                    join menuItemRelationGroup in Db.Set<MenuItemRelationGroup>() on
                        menuItemRelation.MenuItemRelationGroupId equals menuItemRelationGroup.Id
                    select new MenuItemRelationRouteValue
                    {
                        
                        MenuItemRelation = new MenuItemRelation
                        {
                            Id = menuItemRelation.Id,
                            RouteNameId = menuItemRelation.RouteNameId,
                            IsDocked = menuItemRelation.IsDocked,
                            MenuItem= menuItem,
                            MenuItemId= menuItemRelation.MenuItemId,
                            MenuItemRelationGroup = menuItemRelationGroup,
                            MenuItemRelationGroupId = menuItemRelation.MenuItemRelationGroupId,
                            RouteName = routeName
                        },
                        MenuItemRelationId = menuRelationParam.MenuItemRelationId,
                        RouteParam = routeParam,
                        RouteParamId = menuRelationParam.RouteParamId,
                        Value = menuRelationParam.Value
                    }).ToList();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("GetRouteParams() execution has ended");
            }
        }

        public void AddToRoute(MenuItemRelationRouteValue rootEntity, RouteParam parameter)
        {
            throw new NotImplementedException();
        }

        public void AddToRoute(int rootEntityId, int parameterId)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromRoute(MenuItemRelationRouteValue rootEntity, RouteParam parameter)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromRoute(int rootEntityId, int parameterId)
        {
            throw new NotImplementedException();
        }

        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }
    }
}
