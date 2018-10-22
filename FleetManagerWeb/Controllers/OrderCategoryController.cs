namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class OrderCategoryController : Controller
    {
        private readonly IClsOrderCategory objiClsOrderCategory = null;

        public OrderCategoryController(IClsOrderCategory objiClsOrderCategory)
        {
            this.objiClsOrderCategory = objiClsOrderCategory;
        }

        public ActionResult BindOrderCategoryGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchOrderCategoryResult> lstOrderCategory = this.objiClsOrderCategory.SearchOrderCategory(rows, page, search, sidx + " " + sord);
                if (lstOrderCategory != null)
                {
                    return this.FillGrid(page, rows, lstOrderCategory);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteOrderCategory(string strOrderCategoryId)
        {
            try
            {
                string[] strOrderCategory = strOrderCategoryId.Split(',');
                strOrderCategoryId = string.Empty;
                foreach (var item in strOrderCategory)
                {
                    strOrderCategoryId += item.Decode() + ",";
                }

                strOrderCategoryId = strOrderCategoryId.Substring(0, strOrderCategoryId.Length - 1);
                DeleteOrderCategoryResult result = this.objiClsOrderCategory.DeleteOrderCategory(strOrderCategoryId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Order Category", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Order Category", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Order Category", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Order Category", MessageType.DeleteFail));
            }
        }

        public ActionResult OrderCategory()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.OrderCategory);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsOrderCategory objClsOrderCategory = this.objiClsOrderCategory as ClsOrderCategory;
                long lgId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsOrderCategory.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsOrderCategory = this.objiClsOrderCategory.GetOrderCategoryById(lgId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                    objClsOrderCategory.blIsActive = true;
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                return this.View(objClsOrderCategory);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult OrderCategory(ClsOrderCategory objOrderCategory)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.OrderCategory);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objOrderCategory.lgId == 0)
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

                if (objOrderCategory.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objiClsOrderCategory.IsOrderCategoryExists(objOrderCategory.lgId, objOrderCategory.strName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Order Category", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateOrderCategory(objOrderCategory);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objOrderCategory.lgId = this.objiClsOrderCategory.SaveOrderCategory(objOrderCategory);
                        if (objOrderCategory.lgId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Order Category", MessageType.Success);
                            return this.View(objOrderCategory);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Order Category", MessageType.Fail);
                        }
                    }
                }

                return this.View(objOrderCategory);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Order Category", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return this.View(objOrderCategory);
            }
        }

        public ActionResult OrderCategoryView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.OrderCategory);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGrid(int page, int rows, List<SearchOrderCategoryResult> lstOrderCategory)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstOrderCategory != null && lstOrderCategory.Count > 0 ? lstOrderCategory[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedOrderCategoryCol = lstOrderCategory;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objOrderCategory in pagedOrderCategoryCol
                            select new
                            {
                                id = objOrderCategory.Id.ToString().Encode(),
                                Name = objOrderCategory.Name,
                                IsActive = objOrderCategory.IsActive == true ? "Active" : "Inactive"
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateOrderCategory(ClsOrderCategory objCategory)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objCategory.strName))
                {
                    strErrorMsg += Functions.AlertMessage("Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return string.Empty;
            }
        }

    }
}