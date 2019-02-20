using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odec.Menu.ViewModels.Specific
{
    public class MenuFilterVm<TMenuItemGroup, TParentKey, TUserKey, TRoleKey>
    {
        public TMenuItemGroup MenuItemGroup { get; set; }
        public bool InitHierarchy { get; set; }
        public TRoleKey RoleId { get; set; }
        public TUserKey UserId { get; set; }
        public TParentKey ParentId { get; set; }
    }
}
