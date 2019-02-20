using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.Menu.Abstractions.Models.Generic
{
    /// <summary>
    /// Обобщенный класс - навигация
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    public class NavigationGeneric<TKey> : Glossary<TKey>
    {
        /// <summary>
        /// Название
        /// </summary>
        [StringLength(100)]
        public override string Name { get; set; }
        /// <summary>
        /// контроллер
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// действие
        /// </summary>
        public string Action { get; set; }
    }
}
