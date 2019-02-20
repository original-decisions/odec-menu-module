using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odec.Menu.ViewModels
{
    public class MenuItemRelationRouteParamVm<TKey>
    {
        public TKey MenuItemRelationId { get; set; }
        public TKey RouteParamId { get; set; }
        public string RouteParamCode { get; set; }
        public RouteParameterVm<TKey> RouteParam { get; set; }
        public string Value { get; set; }
    }
}
