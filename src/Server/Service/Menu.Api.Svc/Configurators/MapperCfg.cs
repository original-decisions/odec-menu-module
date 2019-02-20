using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using odec.Menu.ViewModels;
using odec.Server.Model.Menu.Specific;

namespace odec.Menu.WebUI.App_Start
{
    public static class MapperCfg
    {

        public static IMapper Mapper;
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RouteMenuItem, RouteMenuItemVm<int, int?>>();
                //dual maps
            });

            Mapper = config.CreateMapper();
            //config.AssertConfigurationIsValid();
        }
        /// <summary>
        /// Создает взаимно олнозначную конвертацию между двумя типами
        /// </summary>
        /// <typeparam name="TEntity">Один из типов</typeparam>
        /// <typeparam name="TAnotherEntity">Другйо тип</typeparam>
        static void CreateDualMap<TEntity, TAnotherEntity>(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TEntity, TAnotherEntity>();
            cfg.CreateMap<TAnotherEntity, TEntity>();
        }
        /// <summary>
        /// Создает взаимно олнозначную конвертацию между двумя типами, используя переддаваемые конвертаторы
        /// </summary>
        /// <typeparam name="TEntity">Один из типов</typeparam>
        /// <typeparam name="TAnotherEntity">Другйо тип</typeparam>
        /// <param name="directCorrespondenceConverter">Конвертатор прямого соответствия</param>
        /// <param name="inverseCorrespondenceConverter">Конвертатор обратного соответствия</param>
        static void CreateDualMap<TEntity, TAnotherEntity>(ITypeConverter<TEntity, TAnotherEntity> directCorrespondenceConverter, ITypeConverter<TAnotherEntity, TEntity> inverseCorrespondenceConverter, IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TEntity, TAnotherEntity>().ConvertUsing(directCorrespondenceConverter);
            cfg.CreateMap<TAnotherEntity, TEntity>().ConvertUsing(inverseCorrespondenceConverter);
        }
    }
}
