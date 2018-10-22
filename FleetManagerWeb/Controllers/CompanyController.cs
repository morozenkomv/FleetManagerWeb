namespace FleetManagerWeb
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class CompanyController : Controller
    {
        /// <summary>   Zero-based index of the cls role,company and user. </summary>
        private readonly IClsRole objiClsRole = null;
        private readonly IClsCompany objiClsCompany = null;
        private readonly IClsUser objiClsUser = null;
        /// <summary>   Zero-based index of the cls role,company and user. </summary>

        public CompanyController(IClsCompany objiClsCompany, IClsRole objIClsRole, IClsUser objiClsUser)
        {
            this.objiClsCompany = objiClsCompany;
            this.objiClsRole = objIClsRole;
            this.objiClsUser = objiClsUser;
        }

        public void BindDropDownListForCompany(ClsCompany objClsCompany, bool blBindDropDownFromDb)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsCompany.lstAdminuser = this.objiClsUser.GetAllAdminUserForDropDown().ToList();
                }
                else
                {
                    objClsCompany.lstAdminuser = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company);
            }
        }

        public ActionResult BindcompanyGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchCompanyResult> lstcompany = this.objiClsCompany.SearchCompany(rows, page, search, sidx + " " + sord);
                if (lstcompany != null)
                {
                    return this.FillGrid(page, rows, lstcompany);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteCompany(string strCompanyId)
        {
            try
            {
                string[] strCompany = strCompanyId.Split(',');
                strCompanyId = string.Empty;
                foreach (var item in strCompany)
                {
                    strCompanyId += item.Decode() + ",";
                }

                strCompanyId = strCompanyId.Substring(0, strCompanyId.Length - 1);
                DeleteCompanyResult result = this.objiClsCompany.DeleteCompany(strCompanyId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Company", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Company", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Company", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Company", MessageType.DeleteFail));
            }
        }
        public ActionResult GetComapnyByUser(string strUserId, string sidx, string sord, int page, int rows, string filters, string search)
        {
            strUserId = strUserId.Decode();

            try
            {
                List<GetCompanyByUserResult> lstCompany = this.objiClsCompany.GetCompanyByUser(Convert.ToInt64(strUserId), rows, page, search, sidx + " " + sord);
                if (lstCompany != null)
                {
                    return this.FillCompanyGrid(lstCompany);
                }
                else
                {
                    return this.Json(string.Empty);
                }
                //return this.Json(this.objiClsCompany.GetCompanyByUser(Convert.ToInt64(strUserId)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        private ActionResult FillCompanyGrid(List<GetCompanyByUserResult> lstCompany)
        {
            try
            {
                var pagedUserCol = lstCompany;
                var jsonData = new
                {
                    rows = (from objcompany in pagedUserCol
                            select new
                            {
                                ShortName = objcompany.ShortName,
                                FullName = objcompany.FullName,
                                Address1 = objcompany.Address1,
                                Vat = objcompany.Vat,
                                Email = objcompany.Email,
                                Person = objcompany.Person,
                                Contact = objcompany.Contact,
                                Phone = objcompany.Phone,
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Company()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Company);
                if (!objPermission.IsActive)
                    return this.RedirectToAction("Logout", "Home");

                ClsCompany objClsCompany = this.objiClsCompany as ClsCompany;
                long lgUserId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                            return this.RedirectToAction("PermissionRedirectPage", "Home");

                        objClsCompany.hdniFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                            return this.RedirectToAction("PermissionRedirectPage", "Home");

                        lgUserId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsCompany = this.objiClsCompany.GetCompanyByCompanyId(lgUserId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion
                this.ViewData["UserRoleID"] = mySession.Current.RoleId;

                this.BindDropDownListForCompany(objClsCompany, true);
                return this.View(objClsCompany);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return this.View();
            }
        }

       

        [HttpPost]
        public ActionResult Company(ClsCompany objCompany)
        {
            try
            {
                objCompany.strAdminUserIds = objCompany.hdnstrAdminUserIds;
                ////bool blEmailFlag = false;
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Company);
                if (!objPermission.IsActive)
                    return this.RedirectToAction("Logout", "Home");

                if (objCompany.lgId == 0)
                {
                    if (!objPermission.Add_Right)
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                else
                {
                    if (!objPermission.Edit_Right)
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                if (objCompany.hdniFrame)
                    this.ViewData["iFrame"] = "iFrame";

                bool blExists = this.objiClsCompany.IsCompanyExists(objCompany.lgId, objCompany.strShortName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Company", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateCompany(objCompany);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objiClsCompany.SaveCompany(objCompany);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Company", MessageType.Success);
                            this.BindDropDownListForCompany(objCompany, true);
                            return this.View(objCompany);

                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Company", MessageType.Fail);
                        }
                    }
                }
                this.BindDropDownListForCompany(objCompany, true);
                return this.View(objCompany);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Company", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return this.View(objCompany);
            }
        }

        public ActionResult CompanyView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Company);
                if (!objPermission.IsActive)
                    return this.RedirectToAction("Logout", "Home");

                if (!objPermission.View_Right)
                    return this.RedirectToAction("PermissionRedirectPage", "Home");

                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                this.ViewData["blEditRights"] = objPermission.Edit_Right;
                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;
                this.ViewData["blExportRights"] = objPermission.Export_Right;
                this.ViewData["UserRoleID"] = mySession.Current.RoleId; //Functions.GetRoleNameByRoleId(mySession.Current.RoleId);
                
                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGrid(int page, int rows, List<SearchCompanyResult> lstCompany)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstCompany != null && lstCompany.Count > 0 ? lstCompany[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedCompanyCol = lstCompany;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objCompany in pagedCompanyCol
                            select new
                            {
                                id = objCompany.Id.ToString().Encode(),
                                ShortName = objCompany.ShortName,
                                FullName = objCompany.FullName,
                                Address1 = objCompany.Address1,
                                Address2 = objCompany.Address2,
                                Address3 = objCompany.Address3,
                                Vat = objCompany.Vat,
                                Email = objCompany.Email,
                                Person = objCompany.Person,
                                Contact = objCompany.Contact,
                                Phone = objCompany.Phone,
                                AdminUserIds = objCompany.AdminUserIds,

                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateCompany(ClsCompany objCompany)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objCompany.strShortName))
                {
                    strErrorMsg += Functions.AlertMessage("ShortName", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objCompany.strFullName))
                {
                    strErrorMsg += Functions.AlertMessage("Full Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objCompany.strAddress1))
                {
                    strErrorMsg += Functions.AlertMessage("Address", MessageType.InputRequired) + "<br/>";
                }
                if (string.IsNullOrEmpty(Convert.ToString(objCompany.intVat)))
                {
                    strErrorMsg += Functions.AlertMessage("Vat", MessageType.InputRequired) + "<br/>";
                }
                if (string.IsNullOrEmpty(objCompany.strEmail))
                {
                    strErrorMsg += Functions.AlertMessage("Email Id", MessageType.InputRequired) + "<br/>";
                }
                if (string.IsNullOrEmpty(objCompany.strPerson))
                {
                    strErrorMsg += Functions.AlertMessage("Person", MessageType.InputRequired) + "<br/>";
                }
                if (string.IsNullOrEmpty(objCompany.strContact))
                {
                    strErrorMsg += Functions.AlertMessage("Contact", MessageType.InputRequired) + "<br/>";
                }
                if (string.IsNullOrEmpty(objCompany.strAdminUserIds))
                {
                    strErrorMsg += Functions.AlertMessage("AdminUserIds", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return string.Empty;
            }
        }

    }
}
