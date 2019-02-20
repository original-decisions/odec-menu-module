using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Menu.Abstractions.Models.Generic
{
    /// <summary>
    /// Обобщенный класс отношений меню
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="TMenu">Тип меню</typeparam>
    /// <typeparam name="TCollectionType">Тип типа коллекции</typeparam>
    /// <typeparam name="TGoodType">Тип типа товара</typeparam>
    public class MenuRelationGeneric<TKey, TMenu, TCollectionType, TGoodType>
    {
        /// <summary>
        /// Идентификатор меню
        /// </summary>
        [Key,Column(Order = 0)]
        public TKey MenuId { get; set; }
        /// <summary>
        /// Меню
        /// </summary>
        public TMenu Menu { get; set; }
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [Key, Column(Order = 1)]
        public TKey CollectionTypeId { get; set; }
        /// <summary>
        /// Тип коллекции
        /// </summary>
        public TCollectionType CollectionType { get; set; }
        /// <summary>
        /// Идентификатор типа товара
        /// </summary>
        [Key, Column(Order = 2)]
        public TKey GoodTypeId { get; set; }
        /// <summary>
        /// Тип товара
        /// </summary>
        public TGoodType GoodType { get; set; }
    }
}
