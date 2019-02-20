using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using odec.Menu.ViewModels;

namespace odec.Menu.WebUI.Infrastructure.HtmlHelpers
{
    public static class BootstrapMenuHelper
    {
        public static HtmlString BootstrapMenu<TKey, TReferenceKey>(this HtmlHelper helper, IEnumerable<RouteMenuItemVm<TKey,TReferenceKey>> model)
        {
            var routeMenuItemVms = model as IList<RouteMenuItemVm<TKey, TReferenceKey>> ?? model.ToList();
            if (model == null || !routeMenuItemVms.Any())
                return new HtmlString(string.Empty);
            var tagBuilder = new TagBuilder("ul");
            var topLevel = routeMenuItemVms.Any(it => it.ParentId == null);

            if (topLevel)
            {
                tagBuilder.AddCssClass("nav");
                tagBuilder.AddCssClass("navbar-nav");
            }
            else
                tagBuilder.AddCssClass("dropdown-menu");

            foreach (var item in routeMenuItemVms)
            {


                var tagBuilder2 = new TagBuilder("li");
                tagBuilder2.AddCssClass("dropdown");
                var routeCollection = new RouteValueDictionary(
                                item.RouteParams.ToDictionary(it => it.Key, it => (object)it.Value));
                if (item.HtmlAttributes == null)
                    item.HtmlAttributes = new Dictionary<string, string>();
                if (item.HasChild)
                {

                    item.HtmlAttributes.Add("data-toggle", "dropdown");
                    item.HtmlAttributes.Add("class", "dropdown-toggle");
                }
                var htmlAttr = item.HtmlAttributes.ToDictionary(it => it.Key, it => (object)it.Value);
                var anchorBuilder = new TagBuilder("a");

                anchorBuilder.MergeAttributes(htmlAttr);
                var urlHelper = new UrlHelper(helper.ViewContext);

                anchorBuilder.Attributes["href"] = urlHelper.RouteUrl(item.RouteName, routeCollection);
                var anchorSb = new StringBuilder(item.Name);
                if (item.HasChild)
                {
                    anchorSb.Append("<b class=\"caret\"></b>");
                }
                anchorBuilder.InnerHtml.Append(anchorSb.ToString());

                var sb = new StringBuilder(anchorBuilder.ToString());

                sb.Append(helper.BootstrapMenu(item.Children));
                tagBuilder2.InnerHtml.Append(sb.ToString());
                tagBuilder.InnerHtml.Append(tagBuilder2.ToString());

            }

            return new HtmlString(tagBuilder.ToString());
        }

        //public static MvcHtmlString BootstrapAjaxMenu<TKey>(this HtmlHelper helper, IEnumerable<RouteMenuItemVm<TKey>> model, AjaxOptions options, MenuOrientation orientation = MenuOrientation.Horizontal)
        //{
        //    var routeMenuItemVms = model as IList<RouteMenuItemVm<TKey>> ?? model.ToList();
        //    if (model == null || !routeMenuItemVms.Any())
        //        return new MvcHtmlString(string.Empty);

        //    var tagBuilder = new TagBuilder("ul");
        //    var topLevel = routeMenuItemVms.Any(it => it.ParentId == null);

        //    if (topLevel)
        //    {
        //        tagBuilder.AddCssClass("nav");
        //        tagBuilder.AddCssClass("navbar-nav");
        //    }
        //    else
        //        tagBuilder.AddCssClass("dropdown-menu");

        //    foreach (var item in routeMenuItemVms)
        //    {


        //        var tagBuilder2 = new TagBuilder("li");
        //        tagBuilder2.AddCssClass("dropdown");
        //        var routeCollection = new RouteValueDictionary(
        //                        item.RouteParams.ToDictionary(it => it.Key, it => (object)it.Value));
        //        if (item.HtmlAttributes == null)
        //            item.HtmlAttributes = new Dictionary<string, string>();
        //        if (item.HasChild)
        //        {

        //            item.HtmlAttributes.Add("data-toggle", "dropdown");
        //            item.HtmlAttributes.Add("class", "dropdown-toggle");
        //        }
        //        var htmlAttr = item.HtmlAttributes.ToDictionary(it => it.Key, it => (object)it.Value);
        //        var anchorBuilder = new TagBuilder("a");

        //        anchorBuilder.MergeAttributes(htmlAttr);
        //        var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

        //        anchorBuilder.Attributes["href"] = urlHelper.RouteUrl(item.RouteName, routeCollection);
        //        var anchorSb = new StringBuilder(item.Name);
        //        if (item.HasChild)
        //        {
        //            anchorSb.Append("<b class=\"caret\"></b>");
        //        }
        //        anchorBuilder.InnerHtml = anchorSb.ToString();

        //        var sb = new StringBuilder(anchorBuilder.ToString());

        //        sb.Append(helper.BootstrapMenu(item.Children));
        //        tagBuilder2.InnerHtml = sb.ToString();
        //        tagBuilder.InnerHtml += tagBuilder2.ToString();

        //    }

        //    return new MvcHtmlString(tagBuilder.ToString());
        //}
    }

    public enum MenuOrientation
    {
        Horizontal,
        Vertical
    }
}
