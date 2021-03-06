﻿using System;
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
    public class MenuItemRepository : OrmEntityOperationsRepository<int, MenuItem, DbContext>, IMenuItemRepository<int, MenuItem>, IContextRepository<DbContext>
    {
        public IEnumerable<MenuItem> Get()
        {
            try
            {
                LogEventManager.Logger.Info("Get() execution has started");
                return Db.Set<MenuItem>().ToList();
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
