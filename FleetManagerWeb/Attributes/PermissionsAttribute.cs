using FleetManagerWeb.Common;
using FleetManagerWeb.Models;
using System;
using System.Web.Mvc;

namespace FleetManagerWeb.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionsAttribute : ActionFilterAttribute
    {
        private readonly Page _page;

        public PermissionsAttribute(Page page)
        {
            _page = page;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission((long)_page);

                if (!objPermission.IsActive)
                    actionContext.Result = new RedirectToRouteResult("Logout", null);

                if (!objPermission.View_Right)
                    actionContext.Result = new RedirectToRouteResult("PermissionRedirectPage", null);

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                actionContext.Controller.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                actionContext.Controller.ViewData["blAddRights"] = objPermission.Add_Right;
                actionContext.Controller.ViewData["blEditRights"] = objPermission.Edit_Right;
                actionContext.Controller.ViewData["blDeleteRights"] = objPermission.Delete_Right;
                actionContext.Controller.ViewData["blExportRights"] = objPermission.Export_Right;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.AdditionalCost, mySession.Current.UserId);
            }            
        }
    }
}