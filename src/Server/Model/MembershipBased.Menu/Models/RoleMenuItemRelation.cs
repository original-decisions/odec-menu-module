using odec.Server.Model.Menu;
using odec.Server.Model.User;

namespace odec.Server.Model.MembershipMenu
{
    public class RoleMenuItemRelation :MenuItemRelation
    {
        public Role Role { get; set; }
        public int? RoleId { get; set; }
    }
}
