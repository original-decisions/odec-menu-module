using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odec.Menu.ViewModels.Specific
{
    public class MenuItemRelationAdminVm<TKey>
    {
        public TKey Id { get; set; }
        public bool IsDocked { get; set; }
        public bool IsPublic { get; set; }
        public string RoleName { get; set; }
        public string Login { get; set; }
        public string MenuCode { get; set; }
        public string MenuItemRelationGroupCode { get; set; }
        public string RouteCode { get; set; }
    }
}
