namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class OrderController : Controller
    {
        private readonly IClsOrderCategory objiClsOrderCategory = null;
        private readonly IClsOrder objiClsOrder = null;
        private readonly IClsUser objiClsUser = null;

        public OrderController(IClsOrder objiClsOrder, IClsOrderCategory objiClsOrderCategory, IClsUser objiClsUser)
        {
            this.objiClsOrder = objiClsOrder;
            this.objiClsOrderCategory = objiClsOrderCategory;
            this.objiClsUser = objiClsUser;
        }

        public ActionResult BindOrderGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchOrderResult> lstOrder = this.objiClsOrder.SearchOrder(rows, page, search, sidx + " " + sord);
                if (lstOrder != null)
                {
                    return this.FillGrid(page, rows, lstOrder);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteOrder(string strOrderId)
        {
            try
            {
                string[] strOrder = strOrderId.Split(',');
                strOrderId = string.Empty;
                foreach (var item in strOrder)
                {
                    strOrderId += item.Decode() + ",";
                }

                strOrderId = strOrderId.Substring(0, strOrderId.Length - 1);
                DeleteOrderResult result = this.objiClsOrder.DeleteOrder(strOrderId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Order", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Order", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Order", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Order", MessageType.DeleteFail));
            }
        }

        public ActionResult Order()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Order);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsOrder objClsOrder = this.objiClsOrder as ClsOrder;
                long lgId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsOrder.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsOrder = this.objiClsOrder.GetOrderById(lgId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                this.BindDropDownListForOrder(objClsOrder, true);
                return this.View(objClsOrder);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult Order(ClsOrder objOrder)
        {
            try
            {

                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Order);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objOrder.lgId == 0)
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

                this.BindDropDownListForOrder(objOrder, true);

                if (objOrder.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateOrder(objOrder);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    objOrder.lgId = this.objiClsOrder.SaveOrder(objOrder);
                    if (objOrder.lgId > 0)
                    {
                        this.ViewData["Success"] = "1";
                        this.ViewData["Message"] = Functions.AlertMessage("Order", MessageType.Success);
                        return this.View(objOrder);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Order", MessageType.Fail);
                    }
                }

                
                return this.View(objOrder);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Order", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return this.View(objOrder);
            }
        }

        public ActionResult OrderView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Order);
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

                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGrid(int page, int rows, List<SearchOrderResult> lstOrder)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstOrder != null && lstOrder.Count > 0 ? lstOrder[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedOrderCol = lstOrder;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objOrder in pagedOrderCol
                            select new
                            {
                                id = objOrder.Id.ToString().Encode(),
                                Name = objOrder.Name,
                                OrderCategory = objOrder.OrderCategory,
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateOrder(ClsOrder objOrder)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objOrder.strName))
                {
                    strErrorMsg += Functions.AlertMessage("Name", MessageType.InputRequired) + "<br/>";
                }
                if (objOrder.lgOrderCategoryId == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Order Category", MessageType.SelectRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return string.Empty;
            }
        }

        public void BindDropDownListForOrder(ClsOrder objClsOrder, bool blBindDropDownFromDb)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsOrder.lstOrderCategory = this.objiClsOrderCategory.GetAllOrderCategoryForDropDown().ToList();
                    objClsOrder.lstUsers = this.objiClsUser.GetUsersOfUserForDropDown().ToList();
                }
                else
                {
                    objClsOrder.lstOrderCategory = new List<SelectListItem>();
                    objClsOrder.lstUsers = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order);
            }
        }

    }
}