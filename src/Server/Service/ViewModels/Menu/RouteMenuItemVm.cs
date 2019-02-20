using System.Collections.Generic;
using System.Linq;

namespace odec.Menu.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class RouteMenuItemVm<TKey,TReferenceKey>
    {
        /// <summary>
        /// 
        /// </summary>
        public TKey Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TReferenceKey ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TReferenceKey UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TReferenceKey RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> RouteParams { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool HasChild {get {return Children.Any(); } }
        /// <summary>
        /// /
        /// </summary>
        public IEnumerable<RouteMenuItemVm<TKey,TReferenceKey>> Children { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> HtmlAttributes { get; set; }
    }
}