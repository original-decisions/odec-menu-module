using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Menu
{
    public class MenuItemRelation
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RouteNameId { get; set; }
        public RouteName RouteName { get; set; }
        public int MenuItemId { get; set; }
        public int MenuItemRelationGroupId { get; set; }
        public MenuItemRelationGroup MenuItemRelationGroup { get; set; }
        public MenuItem MenuItem { get; set; }
        //public int UserId { get; set; }

        //public User User { get; set; }
        public bool IsDocked { get; set; }
    }
}
