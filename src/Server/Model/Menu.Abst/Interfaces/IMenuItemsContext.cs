
using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Menu.Abstractions.Interfaces
{
    /// <summary>
    /// Interface for a menu Context 
    /// </summary>
    /// <typeparam name="TMenuItem">Menu item type</typeparam>
    /// <typeparam name="TMenuItemRelation">Menu items relations type</typeparam>
    /// <typeparam name="TMenuItemRelationGroup"></typeparam>
    public interface IMenuItemsContext<TMenuItem, TMenuItemRelation, TMenuItemRelationGroup>
        where TMenuItem : class
        where TMenuItemRelation : class 
        where TMenuItemRelationGroup : class
    {
        /// <summary>
        /// Menu Items table
        /// </summary>
        DbSet<TMenuItem> MenuItems { get; set; }
        /// <summary>
        /// Menu items relations
        /// </summary>
        DbSet<TMenuItemRelation> MenuItemRelations { get; set; }

         DbSet<TMenuItemRelationGroup> MenuItemRelationGroups { get; set; }
    }
}
