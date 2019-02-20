using odec.Server.Model.Menu;

namespace odec.Server.Model.MembershipMenu
{
    public class UserMenuItemRelation: MenuItemRelation
    {
        public int? UserId { get; set; }
        public User.User User { get; set; }
    }
}
