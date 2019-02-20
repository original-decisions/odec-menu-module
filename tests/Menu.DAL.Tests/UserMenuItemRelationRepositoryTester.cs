using System;
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
    class UserMenuItemRelationRepositoryTester:Tester<UserBasedMenuContext>
    {
        public UserMenuItemRelationRepositoryTester()
        {
            Repository = new MenuItemUserRelationRepository();//IocHelper.GetObject<IMenuItemRelationRepository<int, UserMenuItemRelation, MenuItemRelationRouteValue>>();
          //  Connection = Effort.DbConnectionFactory.CreateTransient();
          //  Db = new UserBasedMenuContext(Connection);
            InitContext();
            
        }

        public UserBasedMenuContext Db { get; set; }

        public IMenuItemRelationRepository<int, UserMenuItemRelation, MenuItemRelationRouteValue> Repository { get; set; }

        [Test]
        public void InitContext()
        {
            var ctx = (Repository as IContextRepository<DbContext>);
            if (ctx == null)
                throw new InvalidCastException("No db Context used");
            Assert.NotNull(ctx);
            var options = CreateNewContextOptions();
            Db = new UserBasedMenuContext(options);
            MenuTestHelper.PopulateDefaultMenuDataUserCtx(Db);
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
                var menu = GenerateMenu();
                Db.MenuItems.Add(menu);
                var item = GenerateModel(menu);
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                item.UserId = null;
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

        private MenuItem GenerateMenu()
        {
           return new MenuItem
            {
                Code = "testMi",
                Name = "test",
                DateCreated = DateTime.Now,
                IsActive = true,
                SortOrder = 100500
            };
        }
        private UserMenuItemRelation GenerateModel(MenuItem item)
        {
            return new UserMenuItemRelation
            {
                IsDocked = true,
                RouteNameId = 1,
                MenuItemId = item.Id,
                //MenuItem = new MenuItem
                //{
                //    Code = "testMi",
                //    Name = "test",
                //    DateCreated = DateTime.Now,
                //    IsActive = true,
                //    SortOrder = 100500
                //},
                MenuItemRelationGroupId = 1,
                UserId = 1
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
                InitContext();
                var item = Db.Set<UserMenuItemRelation>().First();
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
                var options = CreateNewContextOptions();
                using (var db = new UserBasedMenuContext(options))
                {
                     MenuTestHelper.PopulateDefaultMenuDataUserCtx(db);
                }
                using (var db = new UserBasedMenuContext(options))
                {
                    var repo = new MenuItemUserRelationRepository(db);
                    var item = db.Set<UserMenuItemRelation>().First();
                    //Удаляем созданный объект
                    Assert.DoesNotThrow(() => repo.Delete(item.Id));
                }
                
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        [TestCase(3)]
        public void GetById(int id)
        {
            try
            {
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
