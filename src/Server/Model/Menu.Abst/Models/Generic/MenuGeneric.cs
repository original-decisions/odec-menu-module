using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.Menu.Abstractions.Models.Generic
{
    //TODO: Refactor MEnu Logic it is not universal
    /// <summary>
    /// Обобщенный класс - меню
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="TParentMenu">Тип родительского меню</typeparam>
    /// <typeparam name="TNavigation">Тип навигации(TODO:may be it should be refactored)</typeparam>
    public class MenuGeneric<TKey, TParentMenu, TNavigation> : Glossary<TKey>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MenuGeneric()
        {
            IsActive = true;
            Disabled = false;
        }
        /// <summary>
        /// Идентификатор навигации 
        /// </summary>
        public TKey NavigationId { get; set; }
        /// <summary>
        /// Навигация
        /// </summary>
        public TNavigation Navigation { get; set; }
        /// <summary>
        /// Идентификатор родительского меню
        /// </summary>
        public TKey ParentMenuId { get; set; }
        /// <summary>
        /// Родительское меню
        /// </summary>
        public TParentMenu MenuAct { get; set; }
        /// <summary>
        /// Имя категории
        /// </summary>
        [Required]
        [StringLength(150)]
        public string CategoryName { get; set; }
        /// <summary>
        /// Связанная страница
        /// </summary>
        [Required]
        [StringLength(30)]
        [DefaultValue("_Layout")]
        public string RelatedLayoutPage { get; set; }
        /// <summary>
        /// Флаг - что меню не активно
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool Disabled { get; set; }
    }
}