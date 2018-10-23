using FleetManagerWeb.Attributes;
using FleetManagerWeb.Common;
using FleetManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    public class CostTypeController : Controller
    {
        #region Fields

        private readonly IClsCostType _objiClsCostType;

        #endregion

        #region Ctor

        public CostTypeController(IClsCostType objiClsCostType)
        {
            _objiClsCostType = objiClsCostType;
        }

        #endregion

        //public ActionResult BindCostTypeGrid(string sidx, string sord, int page, int rows, string filters, string search)
        //{
        //    try
        //    {
        //        List<SearchTripReasonResult> lstTripReason = this.objiClsTripReason.SearchTripReason(rows, page, search, sidx + " " + sord);
        //        if (lstTripReason != null)
        //        {
        //            return this.FillGridTripReason(page, rows, lstTripReason);
        //        }
        //        else
        //        {
        //            return this.Json(string.Empty);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
        //        return this.Json(string.Empty);
        //    }
        //}

        //private ActionResult FillGridCostType(int page, int rows, List<SearchTripReasonResult> lstTripReason)
        //{
        //    try
        //    {
        //        int pageSize = rows;
        //        int totalRecords = lstTripReason != null && lstTripReason.Count > 0 ? lstTripReason[0].Total : 0;
        //        int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
        //        var jsonData = new
        //        {
        //            total = totalPages,
        //            page,
        //            records = totalRecords,
        //            rows = (from objTripReason in lstTripReason
        //                    select new
        //                    {
        //                        TripReasonName = objTripReason.TripReasonName,
        //                        Id = objTripReason.Id.ToString().Encode()
        //                    }).ToArray()
        //        };
        //        return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
        //        return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
        //    }
        //}


        [Permissions(Page.AdditionalCost)]
        public ActionResult CostTypeView() => View();

        [Permissions(Page.AdditionalCost)]
        public ActionResult CostType() => View();

        [HttpPost]
        [Permissions(Page.AdditionalCost)]
        public ActionResult CostType(ClsCostType objCostType)
        {
            try
            {
                if (objCostType.HdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false;// _objiClsCostType.IsTripReasonExists(objCostType.Id, objCostType.TypeName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Trip Reason", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = "";//this.ValidateTripReason(objCostType);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objCostType.Id = _objiClsCostType.SaveCostType(objCostType);
                        if (objCostType.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Cost type", MessageType.Success);
                            return this.View(objCostType);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Cost type", MessageType.Fail);
                        }
                    }
                }

                return this.View(objCostType);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Trip Reason", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.View(objCostType);
            }
        }
    }
}
