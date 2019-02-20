using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using odec.Server.Model.HtmlElements;

namespace odec.Server.Model.Menu
{
    public class MenuItemRelationHtmlAttribute
    {
        /// <summary>
        /// Идентификатор меню
        /// </summary>
        [Key, Column(Order = 0)]
        public int MenuItemRelationId { get; set; }
        /// <summary>
        /// Меню
        /// </summary>
        public MenuItemRelation Menu { get; set; }
        /// <summary>
        /// html - атрибут
        /// </summary>
        public HtmlAttr HtmlAttr { get; set; }
        /// <summary>
        /// Идентификатор html-атрибута
        /// </summary>
        [Key, Column(Order = 1)]
        public int HtmlAttrId { get; set; }
    }
}
