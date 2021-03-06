﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Membership.Menu.DAL;
using odec.Menu.DAL.Interop;
using odec.Server.Model.MembershipMenu;
using odec.Server.Model.MembershipMenu.Context;
using odec.Server.Model.Menu;

namespace Menu.DAL.Tests
{
    class RoleMenuItemRelationRepositoryTester:Tester<RoleBasedMenuContext>
    {
        protected DbContext Db { get; set; }
        public RoleMenuItemRelationRepositoryTester()
        {
            Repository = new MenuItemRoleRelationRepository();//IocHelper.GetObject<IMenuItemRelationRepository<int, RoleMenuItemRelation, MenuItemRelationRouteValue>>();
            //Connection = Effort.DbConnectionFactory.CreateTransient();
            //
            InitContext();
            
        }
        public IMenuItemRelationRepository<int, RoleMenuItemRelation, MenuItemRelationRouteValue> Repository { get; set; }

        [Test]
        public void InitContext()
        {
            var ctx = (Repository as IContextRepository<DbContext>);
            Assert.NotNull(ctx);
            var options = CreateNewContextOptions();
            Db = new RoleBasedMenuContext(options);
            MenuTestHelper.PopulateDefaultMenuDataRoleCtx(Db);
            Assert.DoesNotThrow(() => ctx.SetContext(Db));
        }
        
        /// <summary>
        /// Сохраняем серверный объект сообщения
        /// для корректной работы необходимо, чтобы отрабатывало удаление элемента
        /// </summary>
        [Test]//атрибут тест -это атрибут из фреймворка nUnit http://www.nunit.org/
        public void Save()
        {
            try
            {
                var item = GenerateModel();
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item));
                //проверяем, что он сохранился(присвоился новый идентификатор в базе)
                Assert.Greater(item.Id, 0);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        private RoleMenuItemRelation GenerateModel()
        {
            return new RoleMenuItemRelation
            {
                IsDocked = true,
                RouteNameId = 1,
                MenuItem = new MenuItem
                {
                    Code = "testMi",
                    Name = "test",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 100500
                },
                RoleId = 1,
                MenuItemRelationGroupId = 1
            };
        }

        /// <summary>
        /// Удаление серверного объекта сообщений
        /// Для корректной работы теста необходимо, чтобы отрабатывало сохранение
        /// </summary>
        [Test]
        public void Delete()
        {
            try
            {
                var item = GenerateModel();
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                item.RoleId = null;
                Assert.DoesNotThrow(() => Repository.Save(item));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item));

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Тест удаления серверного объекта  сообщений по его идентификатору 
        /// </summary>
        [Test]
        public void DeleteById()
        {
            try
            {
                var item = GenerateModel();
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item.Id));
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetById()
        {
            try
            {
                var id = 3;
                MenuItemRelation item = null;
                Assert.DoesNotThrow(() => item = Repository.GetById(id));
                Assert.NotNull(item);
                Assert.Greater(item.Id, 0);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Get()
        {
            try
            {
                IEnumerable<MenuItemRelation> menuRels = null;
                Assert.DoesNotThrow(() => menuRels = Repository.Get());
                Assert.NotNull(menuRels);
                Assert.Greater(menuRels.Count(), 0);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void AddRouteParameter()
        {
            try
            {
                var routeValue = new MenuItemRelationRouteValue
                {
                    MenuItemRelationId = Repository.Get().First().Id,
                    RouteParamId = 3,
                    Value = "rrr"
                };
                Assert.DoesNotThrow(() => Repository.AddRouteParameter(routeValue));
                Db.Set<MenuItemRelationRouteValue>().Remove(routeValue);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void RemoveRouteParam()
        {
            try
            {
                var routeValue = new MenuItemRelationRouteValue
                {
                    MenuItemRelationId = Repository.Get().First().Id,
                    RouteParamId = 3,
                    Value = "rrr"
                };
                Assert.DoesNotThrow(() => Repository.AddRouteParameter(routeValue));
                Assert.DoesNotThrow(() => Repository.RemoveRouteParameter(routeValue));
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }

        }
        [Test]
        public void RemoveRouteParamByIds()
        {
            try
            {
                var routeValue = new MenuItemRelationRouteValue
                {
                    MenuItemRelationId = Repository.Get().First().Id,
                    RouteParamId = 3,
                    Value = "rrr"
                };
                Assert.DoesNotThrow(() => Repository.AddRouteParameter(routeValue));
                Assert.DoesNotThrow(() => Repository.RemoveRouteParameter(routeValue.MenuItemRelationId, routeValue.RouteParamId));
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
    }
}
