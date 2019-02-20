using System.Collections.Generic;

namespace odec.Server.Model.Menu.Specific
{
    public class RouteMenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RouteName { get; set; }
        public int? ParentId { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public IDictionary<string, string> RouteParams { get; set; }
        public IEnumerable<RouteMenuItem> Children { get; set; }
    }
}
