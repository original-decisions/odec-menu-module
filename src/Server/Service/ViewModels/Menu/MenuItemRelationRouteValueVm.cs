using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odec.Menu.ViewModels
{
    public class MenuItemRelationRouteValueVm<TKey>
    {
        public string RouteName { get; set; }

        public TKey RouteNameId { get; set; }

        public IEnumerable<RouteParameterVm<TKey>> RouteParameters { get; set; }  
    }

    public class RouteParameterVm<TKey>:GlossaryVm<TKey>
    {
        public string Value { get; set; }
    }
}
