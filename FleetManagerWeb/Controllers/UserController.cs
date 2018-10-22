namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class UserController : Controller
    {
        /// <summary>   Zero-based index of the cls role and cls user. </summary>
        private readonly IClsRole objiClsRole = null;
        private readonly IClsUser objiClsUser = null;
        /// <summary>   Zero-based index of the cls role and cls user. </summary>

        public UserController(IClsUser objIClsUser, IClsRole objIClsRole)
        {
            this.objiClsUser = objIClsUser;
            this.objiClsRole = objIClsRole;
        }

        public void BindDropDownListForUser(ClsUser objClsUser, bool blBindDropDownFromDb, long? CompanyId, long? RoleId)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsUser.lstRole = this.objiClsRole.GetAllRoleForDropDown(CompanyId).ToList();
                    objClsUser.lstBranch = new List<SelectListItem>();
                    objClsUser.lstAdminUser = this.objiClsUser.GetAllAdminUserForDropDownByRole(RoleId ?? 0).ToList();
                }
                else
                {
                    objClsUser.lstRole = new List<SelectListItem>();
                    objClsUser.lstBranch = new List<SelectListItem>();
                    objClsUser.lstAdminUser = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
            }
        }
        public ActionResult BindUserTrackerPermissionGrid(long lgUserId)
        {
            try
            {
                List<GetUserTrackerPermissionResult> lstUserPermission = Functions.GerUserTrackerPermissionByGroupId(lgUserId);
                if (lstUserPermission != null)
                {
                    return this.FillUserTrackerPermissionGrid(lstUserPermission);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        private ActionResult FillUserTrackerPermissionGrid(List<GetUserTrackerPermissionResult> lstUserPermission)
        {
            try
            {
                int pageSize = lstUserPermission.Count;
                int totalRecords = lstUserPermission != null ? lstUserPermission.Count : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page = 1,
                    records = totalRecords,
                    rows = (from objRole in lstUserPermission
                            select new
                            {
                                Id = objRole.Id,
                                UserId = objRole.UserId,
                                TripStartDate = objRole.TripStartDate,
                                TripEndDate = objRole.TripEndDate,
                                LocationStart = objRole.LocationStart,
                                LocationEnd = objRole.LocationEnd,
                                TripReasonName = objRole.TripReasonName,
                                ReasonRemarks = objRole.ReasonRemarks,
                                KmStart = objRole.KmStart,
                                KmEnd = objRole.KmEnd,
                                KmDriven = objRole.KmDriven,
                                FuelStart = objRole.FuelStart,
                                FuelEnd = objRole.FuelEnd,
                                Username = objRole.Username,
                                EntryDatetime = objRole.EntryDatetime,
                                EntryMethod = objRole.EntryMethod,
                                Editable = objRole.Editable,
                                fgfd = objRole.KmDriven,
                                Active = objRole.Active,
                                CompanyName = objRole.CompanyName,
                                EditColumn = objRole.EditColumn,
                                DeleteColumn = objRole.DeleteColumn
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
        public ActionResult BindUserGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchUserResult> lstUser = this.objiClsUser.SearchUser(rows, page, search, sidx + " " + sord);
                if (lstUser != null)
                {
                    return this.FillGrid(page, rows, lstUser);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteUser(string strUserId)
        {
            try
            {
                string[] strUser = strUserId.Split(',');
                strUserId = string.Empty;
                foreach (var item in strUser)
                {
                    strUserId += item.Decode() + ",";
                }

                strUserId = strUserId.Substring(0, strUserId.Length - 1);
                DeleteUserResult result = this.objiClsUser.DeleteUser(strUserId, mySession.Current.UserId);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));
            }
        }

        [HttpPost]
        public JsonResult BindAdminUserList(long RoleId)
        {
            try
            {
                var result = this.objiClsUser.GetAllAdminUserForDropDownByRole(RoleId).ToList();
                return this.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("User", MessageType.Fail));
            }
        }
        public JsonResult GetUser()
        {
            try
            {
                return this.Json(this.objiClsUser.GetAllUserForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public new ActionResult User(string CompanyId = "")
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsUser objClsUser = this.objiClsUser as ClsUser;
                long lgUserId = 0;

                if (string.IsNullOrEmpty(CompanyId))
                {
                    if (this.Request.QueryString.Count > 0)
                    {
                        if (this.Request.QueryString["iFrame"] != null)
                        {
                            if (!objPermission.Add_Right)
                            {
                                return this.RedirectToAction("PermissionRedirectPage", "Home");
                            }

                            objClsUser.hdniFrame = true;
                        }
                        else
                        {
                            if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                            {
                                return this.RedirectToAction("PermissionRedirectPage", "Home");
                            }

                            lgUserId = this.Request.QueryString.ToString().Decode().longSafe();
                            objClsUser = this.objiClsUser.GetUserByUserId(lgUserId);
                        }
                    }
                    else
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }
                    }
                    objClsUser.lgCompanyID = 0;
                }
                else
                    objClsUser.lgCompanyID = Convert.ToInt64(CompanyId.Decode());


                #region Menu Access
                Controllers.BaseController baseController = new Controllers.BaseController();
                this.ViewData = baseController.MenuAccessPermissions(objPermission);
                #endregion Menu Access

                this.BindDropDownListForUser(objClsUser, true, objClsUser.lgCompanyID, objClsUser.lgRoleId);
                return this.View(objClsUser);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        [ActionName("User")]
        public ActionResult UserInfo(ClsUser objUser)
        {
            try
            {

                ////bool blEmailFlag = false;
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objUser.lgId == 0)
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

                if (objUser.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objiClsUser.IsUserExists(objUser.lgId, objUser.strUserName);
                bool blExists1 = this.objiClsUser.IsUserEmailExists(objUser.lgId, objUser.strEmailID);
                bool blRoleExists = this.objiClsRole.IsRoleDeleted(objUser.lgRoleId);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.AlreadyExist);
                }
                else if (blExists1)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Email Address", MessageType.AlreadyExist);
                }
                else if (blRoleExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Role Deleted", MessageType.AlreadyRoleDeleted);
                }
                else
                {
                    string strErrorMsg = this.ValidateUser(objUser);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        //In case the User do not have any Role the account should be inactive
                        if (objUser.lgRoleId == 0)
                        {
                            objUser.blIsActive = false;
                        }
                        if (objUser.lgAdminUserId == 0)
                            objUser.lgAdminUserId = mySession.Current.UserId;

                        long resultId = this.objiClsUser.SaveUser(objUser);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Success);
                            this.BindDropDownListForUser(objUser, false, objUser.lgCompanyID, objUser.lgRoleId);
                            return this.View(objUser);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Fail);
                        }
                    }
                }

                this.BindDropDownListForUser(objUser, true, objUser.lgCompanyID, objUser.lgRoleId);
                return this.View(objUser);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View(objUser);
            }
        }

        public ActionResult UserView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGrid(int page, int rows, List<SearchUserResult> lstUser)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstUser != null && lstUser.Count > 0 ? lstUser[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedUserCol = lstUser;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objUser in pagedUserCol
                            select new
                            {
                                id = objUser.Id.ToString().Encode(),
                                EmployeeCode = objUser.EmployeeCode,
                                FirstName = objUser.FirstName,
                                SurName = objUser.SurName,
                                MobileNo = objUser.MobileNo,
                                EmailID = objUser.EmailID,
                                UserName = objUser.UserName,
                                Address = objUser.Address,
                                RoleName = objUser.RoleName,
                                BranchName = objUser.BranchName,
                                CompanyName  = objUser.CompanyName,
                                IsActive = objUser.IsActive ? "Active" : "Inactive",
                                RoleId = objUser.RoleId,
                                CompanyCount = objUser.CompanyCount,
                                AssignBy = objUser.AssignBy
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

        private string ValidateUser(ClsUser objUser)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objUser.strFirstName))
                {
                    strErrorMsg += Functions.AlertMessage("First Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strSurName))
                {
                    strErrorMsg += Functions.AlertMessage("Surname", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strMobileNo))
                {
                    strErrorMsg += Functions.AlertMessage("Mobile No", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strEmailID))
                {
                    strErrorMsg += Functions.AlertMessage("Email Id", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strUserName))
                {
                    strErrorMsg += Functions.AlertMessage("User Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strPassword))
                {
                    strErrorMsg += Functions.AlertMessage("Password", MessageType.InputRequired) + "<br/>";
                }
                if (string.IsNullOrEmpty(objUser.strConfirmPassword))
                {
                    strErrorMsg += Functions.AlertMessage("Confirm Password", MessageType.InputRequired) + "<br/>";
                }
                if (objUser.strConfirmPassword != objUser.strPassword)
                {
                    strErrorMsg += Functions.AlertMessage("Password", MessageType.PasswordNotMatch) + "<br/>";
                }

                //if (objUser.lgRoleId == 0)
                //{
                //    strErrorMsg += Functions.AlertMessage("Role", MessageType.SelectRequired) + "<br/>";
                //}

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}