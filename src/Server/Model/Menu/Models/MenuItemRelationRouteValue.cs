using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Menu
{
    public class MenuItemRelationRouteValue
    {
      //  [Key, Column(Order = 0)]
        public int MenuItemRelationId { get; set; }
        public MenuItemRelation MenuItemRelation { get; set; }
    //    [Key, Column(Order = 1)]
        public int RouteParamId { get; set; }
        public RouteParam RouteParam { get; set; }

        public string Value { get; set; }
    }
}
