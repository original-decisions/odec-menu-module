using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Menu.Abstractions.Interfaces.Links
{
    /// <summary>
    /// Прокси объект контекста связи меню и хтмл атрибутов
    /// </summary>
    /// <typeparam name="TMenuItemRelationHtmlAttribute"></typeparam>
    public interface IMenuItemRelationHtmlAttributeContext<TMenuItemRelationHtmlAttribute>
        where TMenuItemRelationHtmlAttribute : class
    {
        DbSet<TMenuItemRelationHtmlAttribute> MenuItemRelationHtmlAttrs { get; set; }
    }
}
