using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Framework.Infrastructure;
using odec.Server.Model.Menu.Abstractions.Interfaces;

namespace odec.Server.Model.Menu.Context
{
    public class MenuContext : DbContext,  
        IRouteMenuItemsContext<MenuItem, MenuItemRelation, MenuItemRelationRouteValue, RouteParam, RouteName, MenuItemRelationGroup>
    {
        private string MenuScheme = "menu";
        public MenuContext(DbContextOptions<MenuContext> options)
            : base(options)
        {
           
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemRelation> MenuItemRelations { get; set; }
        public DbSet<MenuItemRelationGroup> MenuItemRelationGroups { get; set; }
        public DbSet<RouteName> RouteNames { get; set; }
        public DbSet<RouteParam> RouteParams { get; set; }
        public DbSet<MenuItemRelationRouteValue> MenuItemRelationRouteValues { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .ToTable("MenuItems", MenuScheme);
            modelBuilder.Entity<MenuItemRelationGroup>()
                .ToTable("MenuItemRelationGroups", MenuScheme);
            modelBuilder.Entity<MenuItemRelation>()
                .ToTable("MenuItemRelations", MenuScheme);
            modelBuilder.Entity<RouteName>()
                .ToTable("RouteNames", MenuScheme);
            modelBuilder.Entity<RouteParam>()
                .ToTable("RouteParams", MenuScheme);
            modelBuilder.Entity<MenuItemRelationRouteValue>()
                .ToTable("MenuItemRelationRouteValues", MenuScheme)
                .HasKey(it => new { it.MenuItemRelationId,it.RouteParamId});
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
    }
}