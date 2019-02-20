using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Server.Model.Menu;
using odec.Server.Model.Menu.Abstractions.Interfaces;
using odec.Server.Model.User;

using Usr = odec.Server.Model.User.User;
namespace odec.Server.Model.MembershipMenu.Context
{
    
    public class UserBasedMenuContext :
        DbContext, 
  //      IdentityDbContext<Usr, Role, int, UserClaim, UserRole, UserLogin, IdentityRoleClaim<int>, UserToken>,
        IRouteMenuItemsContext<MenuItem, UserMenuItemRelation, MenuItemRelationRouteValue, RouteParam, RouteName, MenuItemRelationGroup>
    {
        private string MembershipScheme = "AspNet";
        private string MenuScheme = "menu";
        public UserBasedMenuContext(DbContextOptions<UserBasedMenuContext> options)
        : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<UserMenuItemRelation> MenuItemRelations { get; set; }
        public DbSet<MenuItemRelationGroup> MenuItemRelationGroups { get; set; }
        public DbSet<RouteName> RouteNames { get; set; }
        public DbSet<RouteParam> RouteParams { get; set; }
        public DbSet<MenuItemRelationRouteValue> MenuItemRelationRouteValues { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usr>()
                .ToTable("Users", MembershipScheme);
            modelBuilder.Entity<Role>()
                .ToTable("Roles", MembershipScheme);
          //  modelBuilder.Entity<UserRole>().ToTable("UserRoles", MembershipScheme);
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims", MembershipScheme);
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins", MembershipScheme);
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", MembershipScheme);
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens", MembershipScheme);

            modelBuilder.Entity<MenuItem>()
                .ToTable("MenuItems", MenuScheme);
            modelBuilder.Entity<MenuItemRelation>()
                .ToTable("MenuItemRelations", MenuScheme);
            modelBuilder.Entity<UserMenuItemRelation>()
                .ToTable("MenuItemRelations", MenuScheme);
            modelBuilder.Entity<MenuItemRelationGroup>()
                .ToTable("MenuItemRelationGroups", MenuScheme);
            modelBuilder.Entity<RouteName>()
                .ToTable("RouteNames", MenuScheme);
            modelBuilder.Entity<RouteParam>()
                .ToTable("RouteParams", MenuScheme);
            modelBuilder.Entity<MenuItemRelationRouteValue>()
                .ToTable("MenuItemRelationRouteValues", MenuScheme)
                .HasKey(it => new { it.RouteParamId, it.MenuItemRelationId, });
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
    }
}
