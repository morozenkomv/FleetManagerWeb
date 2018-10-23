namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Extensions;
    using FleetManagerWeb.Models;

    public class TrackerController : Controller
    {
        private readonly IClsTracker objiClsTracker = null;
        private readonly IClsTripReason objiClsTripReason = null;
        private readonly IClsCompany objiClsCompany = null;
        private readonly IClsGroup objiClsGroup = null;
        private readonly IClsCarFleet objiClsCarFleet = null;
        public TrackerController(IClsTracker objIClsTracker, IClsTripReason objiClsTripReason, IClsCompany objiClsCompany, IClsGroup objiClsGroup, IClsCarFleet objiClsCarFleet)
        {
            this.objiClsTracker = objIClsTracker;
            this.objiClsTripReason = objiClsTripReason;
            this.objiClsCompany = objiClsCompany;
            this.objiClsGroup = objiClsGroup;
            this.objiClsCarFleet = objiClsCarFleet;
        }

        public void BindDropDownListForTracker(ClsTracker objClsTracker, bool blBindDropDownFromDb)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsTracker.lstTripReason = this.objiClsTripReason.GetAllTripReasonForDropDown().ToList();
                    objClsTracker.lstCompany = this.objiClsCompany.GetCompanyForTrackerDropDown().ToList();
                    //objClsTracker.lstGroup = this.objiClsGroup.GetGroupByCompanyId(CompanyId).ToList();
                    objClsTracker.lstCar = this.objiClsCarFleet.GetAllCarFleetForDropDown().ToList();
                    objClsTracker.lstRegisteration = this.objiClsCarFleet.GetAllCarFleetRegisterationForDropDown().ToList();
                    objClsTracker.lstCode = this.objiClsCarFleet.GetAllCarFleetCodeForDropDown().GetAwaiter().GetResult().ToList();
                }
                else
                {
                    objClsTracker.lstTripReason = new List<SelectListItem>();
                    objClsTracker.lstCompany = new List<SelectListItem>();
                    //objClsTracker.lstGroup = new List<SelectListItem>();
                    objClsTracker.lstCar = new List<SelectListItem>();
                    objClsTracker.lstRegisteration = new List<SelectListItem>();
                    objClsTracker.lstCode = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
            }
        }

        public ActionResult BindTrackerGrid(string sidx, string sord, int page, int rows, string filters, string search, string tripstartdate, string tripenddate, string locationstart, string locationend)
        {
            try
            {
                List<SearchTrackerResult> lstTracker = this.objiClsTracker.SearchTracker(rows, page, search, sidx + " " + sord, tripstartdate, tripenddate, locationstart, locationend);
                if (lstTracker != null)
                {
                    return this.FillGridTracker(page, rows, lstTracker);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public ActionResult BindChangeLogTracker(string sidx, string sord, int page, int rows, string filters, string search, int trackerId)
        {
            try
            {
                List<SearchChangeLogTrackerResult> lstLog = this.objiClsTracker.SearchChangeLogTracker(rows, page, search, sidx + " " + sord, trackerId);
                if (lstLog != null)
                {
                    return this.FillGridChangeLogTracker(page, rows, lstLog);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteTracker(string strTrackerId)
        {
            try
            {
                string[] strTracker = strTrackerId.Split(',');
                strTrackerId = string.Empty;
                foreach (var item in strTracker)
                {
                    strTrackerId += item.Decode() + ",";
                }

                strTrackerId = strTrackerId.Substring(0, strTrackerId.Length - 1);

                DeleteTrackerResult result = this.objiClsTracker.DeleteTracker(strTrackerId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Tracker", MessageType.DeleteFail));
            }
        }

        public JsonResult GetUserTrackerPermission()
        {
            try
            {
                GetUserTrackerPermissionResult lstUserPermission = Functions.GerUserTrackerPermissionByGroupId(mySession.Current.UserId).FirstOrDefault();

                if (lstUserPermission != null)
                    return this.Json(lstUserPermission, JsonRequestBehavior.AllowGet);
                else
                    return this.Json("0", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Tracker", MessageType.DeleteFail));
            }
        }

        public ActionResult Tracker()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsTracker objClsTracker = this.objiClsTracker as ClsTracker;
                long lgTrackerId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsTracker.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgTrackerId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsTracker = this.objiClsTracker.GetTrackerByTrackerId(lgTrackerId);
                        objClsTracker.inCodeId = objClsTracker.inCarId;
                        objClsTracker.inCarIdForRegistration = objClsTracker.inCarId;
                    }
                }
                else
                {
                    objClsTracker.strEntryDatetime = DateTime.Now.ToString("dd/MM/yyyy");
                    objClsTracker.strEntryMethod = "W";
                    objClsTracker.blActive = true;
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                this.ViewData["UserRoleID"] = mySession.Current.RoleId;

                this.BindDropDownListForTracker(objClsTracker, true);
                return this.View(objClsTracker);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult Tracker(ClsTracker objTracker)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objTracker.inId == 0)
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (!objPermission.Edit_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objTracker.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateTracker(objTracker);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    var carFleet = objiClsCarFleet.GetCarFleetByCarFleetId((long)objTracker.inCarId);
                    objTracker.inId = this.objiClsTracker.SaveTracker(objTracker, carFleet);
                    if (objTracker.inId > 0)
                    {
                        this.ViewData["Success"] = "1";
                        this.ViewData["Message"] = Functions.AlertMessage("Tracker", MessageType.Success);
                        this.BindDropDownListForTracker(objTracker, true);
                        return this.View(objTracker);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Tracker", MessageType.Fail);
                    }
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                this.ViewData["UserRoleID"] = mySession.Current.RoleId;
                this.BindDropDownListForTracker(objTracker, true);
                return this.View(objTracker);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Tracker", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View(objTracker);
            }
        }

        public ActionResult TrackerFormattedPrint()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                this.ViewData["UserName"] = mySession.Current.UserName;
                this.ViewData["UserRoleID"] = mySession.Current.RoleId;
                this.ViewData["lstCar"] = this.objiClsCarFleet.GetAllCarFleetForDropDown().ToList();
                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult GenerateTrackerFormattedReport(string tripStartDate, string tripEndDate, int carId)
        {
            try
            {
                List<GenerateTrackerFormattedReportResult> lstTracker = this.objiClsTracker.GenerateTrackerFormattedReport(tripStartDate, tripEndDate, carId);
                if (lstTracker != null)
                {
                    var data = lstTracker.ToReportList();
                    return Json(new
                    {
                        KMDriven = lstTracker.Select(x => x.Km_Driven).Sum(),
                        Data = data,
                        IsValid = !data.Any(_ => !_.IsValid)
                    });
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public ActionResult TrackerView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                this.ViewData["blEditRights"] = objPermission.Edit_Right;
                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;
                this.ViewData["blExportRights"] = objPermission.Export_Right;

                this.ViewData["UserRoleID"] = mySession.Current.RoleId;
                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGridTracker(int page, int rows, List<SearchTrackerResult> lstTracker)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstTracker != null && lstTracker.Count > 0 ? lstTracker[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objTracker in lstTracker
                            select new
                            {
                                TripStartDate = objTracker.TripStartDate,
                                TripEndDate = objTracker.TripEndDate,
                                LocationStart = objTracker.LocationStart,
                                LocationEnd = objTracker.LocationEnd,
                                ReasonRemarks = objTracker.ReasonRemarks,
                                KmStart = objTracker.KmStart,
                                KmEnd = objTracker.KmEnd,
                                KmDriven = objTracker.KmDriven,
                                FuelStart = objTracker.FuelStart,
                                FuelEnd = objTracker.FuelEnd,
                                Username = objTracker.Username,
                                EntryDatetime = objTracker.EntryDatetime,
                                EntryMethod = objTracker.EntryMethod,
                                Editable = objTracker.Editable,
                                Active = objTracker.Active,
                                TripReasonName = objTracker.TripReasonName,
                                Id = objTracker.Id.ToString().Encode(),
                                CompanyName = objTracker.CompanyName
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult FillGridChangeLogTracker(int page, int rows, List<SearchChangeLogTrackerResult> lstLog)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstLog != null && lstLog.Count > 0 ? lstLog[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objTracker in lstLog
                            select new
                            {
                                AuditComments = objTracker.AuditComments,
                                TripStartDate = objTracker.Trip_Start.ToString("dd/MM/yyyy"),
                                TripEndDate = objTracker.Trip_End.ToString("dd/MM/yyyy"),
                                LocationStart = objTracker.Location_Start,
                                LocationEnd = objTracker.Location_End,
                                TripReasonId = objTracker.Reason_Id,
                                ReasonRemarks = objTracker.Reason_Remarks,
                                KmStart = objTracker.Km_Start,
                                KmEnd = objTracker.Km_End,
                                KmDriven = objTracker.Km_Driven,
                                FuelStart = objTracker.Fuel_Start,
                                FuelEnd = objTracker.Fuel_End,
                                EntryMethod = objTracker.Entry_Method,
                                Editable = objTracker.Editable,
                                Active = objTracker.Active,
                                CompanyId = objTracker.CompanyId,
                                Username = objTracker.ModifiedBy,
                                EntryDate = objTracker.ModifiedOn.ToString(),
                                Id = objTracker.Id.ToString().Encode(),

                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateTracker(ClsTracker objTracker)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (objTracker.inCarId == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Car", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.strTripStart))
                {
                    strErrorMsg += Functions.AlertMessage("Trip Start Date", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.strTripEnd))
                {
                    strErrorMsg += Functions.AlertMessage("Trip End Date", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.strLocationStart))
                {
                    strErrorMsg += Functions.AlertMessage("Location Start", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.strLocationEnd))
                {
                    strErrorMsg += Functions.AlertMessage("Location End", MessageType.InputRequired) + "<br/>";
                }
                else if (objTracker.lgReasonId == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Trip Reason", MessageType.SelectRequired) + "<br/>";
                }
                else if (objTracker.inKmStart == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Km Start", MessageType.InputRequired) + "<br/>";
                }
                else if (objTracker.inKmEnd == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Km End", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.inFuelStart.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Fuel Start", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.inFuelEnd.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Fuel End", MessageType.InputRequired) + "<br/>";
                }
                else if (objTracker.lgCompanyId == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Company", MessageType.InputRequired) + "<br/>";
                }
                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return string.Empty;
            }
        }

        [HttpPost]
        public JsonResult BindGroupListByCompany(long CompanyId)
        {
            try
            {
                var result = this.objiClsGroup.GetGroupByCompanyId(CompanyId).ToList();
                return this.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("User", MessageType.Fail));
            }
        }
    }
}