using System.Collections.Generic;

namespace odec.Server.Model.Menu.Abstractions
{
    public abstract class Navigation
    {
        public string RouteName { get; set; }
        public IDictionary<string,object> RouteParameters { get; set; }
    }
}
