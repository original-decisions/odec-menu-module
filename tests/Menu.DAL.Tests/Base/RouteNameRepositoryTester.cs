using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Menu.DAL;
using odec.Menu.DAL.Interop;
using odec.Server.Model.Menu;
using odec.Server.Model.Menu.Context;

namespace Menu.DAL.Tests
{
    class RouteNameRepositoryTester:Tester<MenuContext>
    {
        public RouteNameRepositoryTester()
        {
            Repository = new RouteNameRepository();//IocHelper.GetObject<IRouteNameRepository<int, RouteName>>();
           // Connection = Effort.DbConnectionFactory.CreateTransient();
           // Db = new MenuContext(Connection);
            InitContext();
        }
        public IRouteNameRepository<int, RouteName> Repository { get; set; }

        [Test]
        public void InitContext()
        {
            var ctx = (Repository as IContextRepository<DbContext>);
            if (ctx == null)
                throw new InvalidCastException("No db Context used");
            Assert.NotNull(ctx);
            var options = CreateNewContextOptions();
            Assert.DoesNotThrow(() => ctx.SetContext(new MenuContext(options)));
        }
        /// <summary>
        /// Генерирует тестовый серверный объект - сообщения
        /// </summary>
        /// <returns>тестовый серверный объект - сообщения</returns>
        private RouteName GenerateModel()
        {
            return new RouteName
            {
                Code = "Testitem",
                IsActive = true,
                Name = "Testitem",
                SortOrder = 0
            };
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
        /// <summary>
        /// Тест деактивации записи серверного объекта сообщений
        /// </summary>
        [Test]
        public void Deactivate()
        {
            try
            {
                var item = GenerateModel();
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                //вызов деактивации серверного объекта
                Assert.DoesNotThrow(() => Repository.Deactivate(item));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item));
                //Проверка, что деактивация сработала
                Assert.IsFalse(item.IsActive);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Тест деактивации записи серверного объекта сообщений по его идентификатору 
        /// </summary>
        [Test]
        public void DeactivateById()
        {
            try
            {
                var item = GenerateModel();
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                //вызов деактивации серверного объекта
                Assert.DoesNotThrow(() => item = Repository.Deactivate(item.Id));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item));
                //Проверка, что деактивация сработала
                Assert.IsFalse(item.IsActive);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Тест активации записи серверного объекта сообщений по его идентификатору 
        /// </summary>
        [Test]
        public void Activate()
        {
            try
            {
                var item = GenerateModel();
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                //вызов активации серверного объекта
                Assert.DoesNotThrow(() => Repository.Activate(item));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item));
                //Проверка, что активация сработала
                Assert.IsTrue(item.IsActive);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Тест активации записи серверного объекта сообщения по его идентификатору 
        /// </summary>
        [Test]
        public void ActivateById()
        {
            try
            {
                var item = GenerateModel();
                item.IsActive = false;
                //сохраняем генерированный объект
                Assert.DoesNotThrow(() => Repository.Save(item));
                //вызов активации серверного объекта
                Assert.DoesNotThrow(() => item = Repository.Activate(item.Id));
                //Удаляем созданный объект
                Assert.DoesNotThrow(() => Repository.Delete(item));
                //Проверка, что активация сработала
                Assert.IsTrue(item.IsActive);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// Тест проверяющий возможность получения по идентификатору сущности
        /// (для удачного прохождения теста необходимо, чтобы объект сохранялся и удалялся)
        /// </summary>
        [Test]
        public void GetById()
        {
            try
            {
                var item = GenerateModel();
                Assert.DoesNotThrow(() => Repository.Save(item));

                Assert.DoesNotThrow(() => item = Repository.GetById(item.Id));
                Assert.DoesNotThrow(() => Repository.Delete(item));
                Assert.NotNull(item);
                Assert.Greater(item.Id, 0);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
    }
}
