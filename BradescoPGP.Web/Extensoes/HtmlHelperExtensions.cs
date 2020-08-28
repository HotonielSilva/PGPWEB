using System;
using System.Linq;
using System.Web.Mvc;

namespace BradescoPGP.Web
{
    public static class HtmlHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active", string area = "")
        {
            
            var viewContext = html.ViewContext;
            var isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            var routeValues = viewContext.RouteData.Values;
            var currentArea = viewContext.RouteData.DataTokens["area"] as string ?? string.Empty;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            if (string.IsNullOrEmpty(controllers))
            {
                return acceptedActions.Contains(currentAction) && area == currentArea ? cssClass : string.Empty;
            }

            if (string.IsNullOrEmpty(actions))
            {
                return acceptedControllers.Contains(currentController) && area == currentArea ? cssClass : string.Empty;
            }

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) && area == currentArea ?
                cssClass : String.Empty;
        }
    }
}