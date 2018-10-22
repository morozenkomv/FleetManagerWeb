namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class MailController : Controller
    {
        private readonly IClsUser objiClsUser = null;
        private readonly IClsMail objiClsMail = null;

        public MailController(IClsUser objIClsUser, IClsMail objClsMail)
        {
            this.objiClsUser = objIClsUser;
            this.objiClsMail = objClsMail;
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Compose, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public ActionResult Compose()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Compose);
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

                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Compose, mySession.Current.UserId);
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
                                CompanyCount = objUser.CompanyCount
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Compose);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SendMesaage(string strUserId, string message)
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

                ClsMail objClsMail = new ClsMail();
                objClsMail.strMessage = message;
                objClsMail.strUserIds = strUserId;


                long result = this.objiClsMail.SaveMail(objClsMail);
                if (result > 0)
                {
                    return this.Json(Functions.AlertMessage("Mail", MessageType.Success));
                }
               
                return this.Json(Functions.AlertMessage("Mail", MessageType.Fail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Compose, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Mail", MessageType.Fail));
            }
        }

        public ActionResult Inbox()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Inbox);
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

                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;

                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View();
            }
        }

        public ActionResult BindInbox(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchMailResult> lstMail = this.objiClsMail.SearchMail(rows, page, search, sidx + " " + sord);
                if (lstMail != null)
                {
                    return this.FillInboxGrid(page, rows, lstMail);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Inbox, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        private ActionResult FillInboxGrid(int page, int rows, List<SearchMailResult> lstMail)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstMail != null && lstMail.Count > 0 ? lstMail[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedMailCol = lstMail;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMail in pagedMailCol
                            select new
                            {
                                id = objMail.Id.ToString().Encode(),
                                Mail = objMail.Mail,
                                Date = objMail.Date == null ? "" : objMail.Date.Value.ToString("dd-MMM-yyyy HH:mm:ss")
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Inbox);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteMail(string strMailId)
        {
            try
            {
                string[] strUser = strMailId.Split(',');
                strMailId = string.Empty;
                foreach (var item in strUser)
                {
                    strMailId += item.Decode() + ",";
                }

                strMailId = strMailId.Substring(0, strMailId.Length - 1);
                DeleteMailResult result = this.objiClsMail.DeleteMail(strMailId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Mail", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Mail", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Mail", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Inbox, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Mail", MessageType.DeleteFail));
            }
        }
    }
}