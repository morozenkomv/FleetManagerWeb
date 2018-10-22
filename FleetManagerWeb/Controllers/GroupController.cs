namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class GroupController : Controller
    {
        /// <summary>   Zero-based index of the cls Groups, role,company and user. </summary>
        private readonly IClsGroup objiClsGroup = null;
        private readonly IClsRole objiClsRole = null;
        private readonly IClsCompany objiClsCompany = null;
        private readonly IClsUser objiClsUser = null;
        /// <summary>   Zero-based index of the cls role,company and user. </summary>
        /// 
        public GroupController(IClsGroup objiClsGroup, IClsCompany objiClsCompany, IClsRole objIClsRole, IClsUser objiClsUser)
        {
            this.objiClsGroup = objiClsGroup;
            this.objiClsCompany = objiClsCompany;
            this.objiClsRole = objIClsRole;
            this.objiClsUser = objiClsUser;
        }

        public void BindDropDownListForGroup(ClsGroup objClsGroup, bool blBindDropDownFromDb, long? CompanyId, long? ParentGroupId)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsGroup.lstGroups = this.objiClsGroup.GetParentGroupForDropDown().ToList();
                    objClsGroup.lstCompany = this.objiClsCompany.GetCompanyForGroupDropDown().ToList();
                    objClsGroup.lstUsers = this.objiClsUser.GetAllUsersByCompany(CompanyId ?? 0, ParentGroupId ?? 0).ToList();
                }
                else
                {
                    objClsGroup.lstGroups = new List<SelectListItem>();
                    objClsGroup.lstCompany = new List<SelectListItem>();
                    objClsGroup.lstUsers = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group);
            }
        }

        [HttpPost]
        public JsonResult BindAllUsersByCompany(long? CompanyId, long? ParentGroupId)
        {
            try
            {
                var result = this.objiClsUser.GetAllUsersByCompany(CompanyId, ParentGroupId).ToList();
                return this.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("User", MessageType.Fail));
            }
        }
        [HttpGet]
        public JsonResult BindSubGroupGrid(string sidx, string sord, int page, int rows, string filters, string search, long? ParentGroupId)
        {
            try
            {
                List<SearchGroupSubGroupResult> lstGroup = this.objiClsGroup.SearchGroup(rows, page, search, sidx + " " + sord, ParentGroupId ?? 0);
                if (lstGroup != null)
                {
                    return this.FillJsonGrid(page, rows, lstGroup);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteGroup(string strGroupId)
        {
            try
            {
                string[] strGroup = strGroupId.Split(',');
                strGroupId = string.Empty;
                foreach (var item in strGroup)
                {
                    strGroupId += item.Trim() + ",";
                }

                strGroupId = strGroupId.Substring(0, strGroupId.Length - 1);
                DeleteGroupResult result = this.objiClsGroup.DeleteGroup(strGroupId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Group", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Group", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Group", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Group", MessageType.DeleteFail));
            }
        }

        [HttpGet]
        public ActionResult BindGroupGrid(string sidx, string sord, int page, int rows, string filters, string search, long ParentGroupId)
        {
            try
            {
                List<SearchGroupSubGroupResult> lstGroup = this.objiClsGroup.SearchGroup(rows, page, search, sidx + " " + sord, ParentGroupId);
                if (lstGroup != null)
                {
                    return this.FillGrid(page, rows, lstGroup);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }
        private ActionResult FillGrid(int page, int rows, List<SearchGroupSubGroupResult> lstGroup)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstGroup != null && lstGroup.Count > 0 ? lstGroup[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedGroupCol = lstGroup;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objGroup in pagedGroupCol
                            select new
                            {
                                id = objGroup.Id, //.ToString().Encode(),
                                GroupId = objGroup.Id.ToString().Encode(),
                                GroupName = objGroup.GroupName,
                                CompanyId = objGroup.CompanyId,
                                CompanyName = objGroup.CompanyName,
                                ParentGroupId = objGroup.ParentGroupId,
                                UserCount = objGroup.UserCount
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult FillJsonGrid(int page, int rows, List<SearchGroupSubGroupResult> lstGroup)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstGroup != null && lstGroup.Count > 0 ? lstGroup[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedGroupCol = lstGroup;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objGroup in pagedGroupCol
                            select new
                            {
                                id = objGroup.Id.ToString().Encode(),
                                GroupId = objGroup.Id,
                                GroupName = objGroup.GroupName,
                                CompanyId = objGroup.CompanyId,
                                CompanyName = objGroup.CompanyName,
                                ParentGroupId = objGroup.ParentGroupId,
                                UserCount = objGroup.UserCount
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Group()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Group);
                if (!objPermission.IsActive)
                    return this.RedirectToAction("Logout", "Home");

                ClsGroup objClsGroup = this.objiClsGroup as ClsGroup;
                long lgUserId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                            return this.RedirectToAction("PermissionRedirectPage", "Home");

                        objClsGroup.hdniFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString()))
                            return this.RedirectToAction("PermissionRedirectPage", "Home");

                        lgUserId = this.Request.QueryString.ToString().longSafe();
                        objClsGroup = this.objiClsGroup.GetGroupByGroupId(lgUserId);
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
                #endregion Menu Access

                this.ViewData["UserRoleID"] = mySession.Current.RoleId;

                this.BindDropDownListForGroup(objClsGroup, true, objClsGroup.lgCompanyId, objClsGroup.lgParentGroupId);
                return this.View(objClsGroup);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult Group(ClsGroup objGroup)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Group);
                if (!objPermission.IsActive)
                    return this.RedirectToAction("Logout", "Home");

                if (objGroup.lgId == 0)
                {
                    if (!objPermission.Add_Right)
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                else
                {
                    if (!objPermission.Edit_Right)
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                if (objGroup.hdniFrame)
                    this.ViewData["iFrame"] = "iFrame";

                bool blExists = this.objiClsGroup.IsGroupExists(objGroup.lgId, objGroup.strGroupName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Group", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateGroup(objGroup);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objiClsGroup.SaveGroup(objGroup);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Group", MessageType.Success);
                            this.BindDropDownListForGroup(objGroup, true, objGroup.lgCompanyId, objGroup.lgParentGroupId);
                            return this.View(objGroup);

                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Group", MessageType.Fail);
                        }
                    }
                }
                this.BindDropDownListForGroup(objGroup, true, objGroup.lgCompanyId, objGroup.lgParentGroupId);
                return this.View(objGroup);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Group", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.View(objGroup);
            }
        }
        public ActionResult GroupView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Group);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.View();
            }
        }

        public ActionResult GroupTreeView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Group);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return this.View();
            }
        }
        private string ValidateGroup(ClsGroup objGroup)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objGroup.strGroupName))
                {
                    strErrorMsg += Functions.AlertMessage("GroupName", MessageType.InputRequired) + "<br/>";
                }

                if (objGroup.lgParentGroupId == 0)
                {
                    if (objGroup.lgCompanyId == 0)
                    {
                        strErrorMsg += Functions.AlertMessage("Company", MessageType.InputRequired) + "<br/>";
                    }
                }
                if (string.IsNullOrEmpty(objGroup.hdnstrUserIds))
                {
                    strErrorMsg += Functions.AlertMessage("User", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return string.Empty;
            }
        }

        public ActionResult GetUsersByGroup(string strGroupId, string sidx, string sord, int page, int rows, string filters, string search)
        {
            strGroupId = strGroupId.Decode();

            try
            {
                List<GetUsersByGroupResult> lstusers = this.objiClsUser.GetUsersByGroup(Convert.ToInt64(strGroupId), rows, page, search, sidx + " " + sord);
                if (lstusers != null)
                {
                    return this.FillUserDetailGrid(lstusers);
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
        private ActionResult FillUserDetailGrid(List<GetUsersByGroupResult> lstusers)
        {
            try
            {
                var pagedUserCol = lstusers;
                var jsonData = new
                {
                    rows = (from objcompany in pagedUserCol
                            select new
                            {
                                UserId = objcompany.UserId,
                                EmployeeCode = objcompany.EmployeeCode,
                                FirstName = objcompany.FirstName,
                                SurName = objcompany.SurName,
                                MobileNo = objcompany.MobileNo,
                                EmailID = objcompany.EmailID,
                                UserName = objcompany.UserName,
                                Address = objcompany.Address,
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
    }
}