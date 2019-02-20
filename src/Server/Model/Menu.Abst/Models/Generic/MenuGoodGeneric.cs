using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Menu.Abstractions.Models.Generic
{
    /// <summary>
    /// Обобщенный класс связи меню и товара
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="TGood">Тип товара</typeparam>
    /// <typeparam name="TMenu">Тип меню</typeparam>
    public class MenuGoodGeneric<TKey, TGood, TMenu>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Key,Column(Order=0)]
        public TKey GoodId { get; set; }
        /// <summary>
        /// Товар
        /// </summary>
        public TGood Good { get; set; }
        /// <summary>
        /// Идентификатор меню
        /// </summary>
        [Key, Column(Order = 1)]
        public TKey MenuId { get; set; }
        /// <summary>
        /// Меню
        /// </summary>
        public TMenu Menu { get; set; }
    }
}
