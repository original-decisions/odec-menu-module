using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Menu.Abstractions.Models.Generic
{
    /// <summary>
    /// Обобщенный класс связи Html атрибутов и меню 
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="THtmlAttr">Тип html-атрибутов</typeparam>
    /// <typeparam name="TMenu">Тип меню</typeparam>
    public class MenuHtmlAttr<TKey,THtmlAttr, TMenu>
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
        /// html - атрибут
        /// </summary>
        public THtmlAttr HtmlAttr { get; set; }
        /// <summary>
        /// Идентификатор html-атрибута
        /// </summary>
        [Key, Column(Order = 1)]
        public TKey HtmlAttrId { get; set; }
    }
}
