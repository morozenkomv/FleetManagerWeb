using FleetManagerWeb.Common;
using FleetManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    public class BaseController : Controller
    {
        public ViewDataDictionary MenuAccessPermissions(GetPagePermissionResult objPermission)
        {
            #region Menu Access

            bool blUserAccess = true, blCompanyAccess = true,blGroupAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true, blComposeAccess = true, blInboxAccess = true, blOrderCategoryAccess = true, blOrderAccess = true;

            objPermission = Functions.CheckPagePermission(PageMaster.User);
            if (!objPermission.View_Right)
                blUserAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Company);
            if (!objPermission.View_Right)
                blCompanyAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Group);
            if (!objPermission.View_Right)
                blGroupAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Role);
            if (!objPermission.View_Right)
                blRoleAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
            if (!objPermission.View_Right)
                blTrackerAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.CarFleet);
            if (!objPermission.View_Right)
                blCarFleetAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.FleetMakes);
            if (!objPermission.View_Right)
                blFleetMakesAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.FleetModels);
            if (!objPermission.View_Right)
                blFleetModelsAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.FleetColors);
            if (!objPermission.View_Right)
                blFleetColorsAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.TripReason);
            if (!objPermission.View_Right)
                blTripReasonAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Compose);
            if (!objPermission.View_Right)
                blComposeAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Inbox);
            if (!objPermission.View_Right)
                blInboxAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.OrderCategory);
            if (!objPermission.View_Right)
                blOrderCategoryAccess = false;

            objPermission = Functions.CheckPagePermission(PageMaster.Order);
            if (!objPermission.View_Right)
                blOrderAccess = false;

            this.ViewData["CompanyAccess"] = blCompanyAccess;
            this.ViewData["GroupAccess"] = blGroupAccess;
            this.ViewData["RoleAccess"] = blRoleAccess;
            this.ViewData["UserAccess"] = blUserAccess;
            this.ViewData["TrackerAccess"] = blTrackerAccess;
            this.ViewData["CarFleetAccess"] = blCarFleetAccess;
            this.ViewData["FleetMakesAccess"] = blFleetMakesAccess;
            this.ViewData["FleetModelsAccess"] = blFleetModelsAccess;
            this.ViewData["FleetColorsAccess"] = blFleetColorsAccess;
            this.ViewData["TripReasonAccess"] = blTripReasonAccess;
            this.ViewData["ComposeAccess"] = blComposeAccess;
            this.ViewData["InboxAccess"] = blInboxAccess;
            this.ViewData["OrderCategoryAccess"] = blOrderCategoryAccess;
            this.ViewData["OrderAccess"] = blOrderAccess;
            return ViewData;

            #endregion
        }

    }
}
