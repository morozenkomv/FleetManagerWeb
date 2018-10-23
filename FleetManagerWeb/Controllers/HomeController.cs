namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class HomeController : Controller
    {
        private readonly IClsUser objiClsUser = null;

        public HomeController()
        {

        }

        public HomeController(IClsUser objIClsUser)
        {
            this.objiClsUser = objIClsUser;
        }

        public ActionResult Index()
        {

            GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
            #region Menu Access
            Controllers.BaseController baseController = new Controllers.BaseController();
            this.ViewData = baseController.MenuAccessPermissions(objPermission);
            #endregion Menu Access

            return this.View();
        }

        public ActionResult Login()
        {
            try
            {
                ClsUser objLogin = this.objiClsUser as ClsUser;
                if (Functions.GetRememberMe("rememberme") == "true")
                {
                    objLogin.strUserName = Functions.GetRememberMe("username");
                    objLogin.strPassword = Functions.GetRememberMe("password");
                    this.ViewBag.password = Functions.GetRememberMe("password");
                    objLogin.blRememberMe = Convert.ToBoolean(Functions.GetRememberMe("rememberme"));
                }

                return this.View(objLogin);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Login(ClsUser objLogin)
        {
            try
            {
                ClsUser objClsUser = this.objiClsUser.ValidateLogin(objLogin.strUserName, objLogin.strPassword.EncryptString());
                if (objClsUser != null)
                {
                    Functions.UpdateCookies(objClsUser.strUserName, objClsUser.strPassword.EncryptString(), 
                        objClsUser.lgId.ToString(), objClsUser.strFirstName + " " + objClsUser.strSurName, 
                        objLogin.blRememberMe.ToString(), objClsUser.lgRoleId.ToString(), objClsUser.lgBranchId.ToString(), 
                        "true", objClsUser.intSessionDurationHour, objClsUser.strFirstName, objClsUser.strSurName);
                    //objClsUser.blIsLogin = true;
                    //this.objiClsUser.SaveUser(objClsUser);
                }
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return this.View(objLogin);
            }
        }

        public ActionResult Logout()
        {
            try
            {
                ClsUser objClsUser = this.objiClsUser as ClsUser;
                objClsUser = this.objiClsUser.GetUserByUserId(mySession.Current.UserId);
                //if (objClsUser != null)
                //{
                //    objClsUser.blIsLogin = false;
                //    this.objiClsUser.SaveUser(objClsUser);
                //}

                this.objiClsUser.InserActivityLogOfUser(objClsUser.strUserName, objClsUser.strPassword.EncryptString(), "User", false); // true => User Logout

                Functions.LogoutUser();
                this.ViewData.Clear();
                this.TempData.Clear();
                return this.RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }

        public ActionResult PermissionRedirectPage()
        {
            if (mySession.Current.UserId == 0)
            {
                return this.RedirectToAction("Login");
            }

            return this.View();
        }

        public ActionResult UserUnAuthorize()
        {
            try
            {
                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return this.View();
            }
        }

        [HttpPost]
        public JsonResult ValidateLogin(ClsUser objLogin)
        {
            try
            {
                var UserValidation = this.objiClsUser.IsUserBlock(objLogin.strUserName);
                if (Convert.ToBoolean(UserValidation.IsUserBlock))
                {
                    return this.Json("7777");
                }
                else if(Convert.ToBoolean(UserValidation.IsPasswordExpire))
                {
                    return this.Json("8888");
                }
                else
                {
                    //Validate User And insert log for Successfull login or invalid credentials
                    string EmailID = this.objiClsUser.InserActivityLogOfUser(objLogin.strUserName, objLogin.strPassword.EncryptString(), "User", true); // true => User Login

                    if (!string.IsNullOrEmpty(EmailID))
                    {
                        return this.Json(EmailID);
                    }

                    //ClsUser objUser = this.objiClsUser.ValidateLogin(objLogin.strUserName, objLogin.strPassword.EncryptString());
                    //if (EmailID != null)
                    //{
                    //    return this.Json(objUser.strEmailID);
                    //}

                    return this.Json("2222");
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return this.Json("1111");
            }
        }

        public ActionResult ChangePassword(string strCurrentPwd, string strNewPwd)
        {
            try
            {
                if (mySession.Current.Password == strCurrentPwd.EncryptString())
                {
                    ClsUser objUser = this.objiClsUser.ChangePassword(mySession.Current.UserId, strNewPwd);
                    Functions.UpdateCookies(mySession.Current.UserName, strNewPwd.EncryptString(), mySession.Current.UserId.ToString(),
                        mySession.Current.Fullname, mySession.Current.Rememberme, mySession.Current.RoleId.ToString(), 
                        mySession.Current.BranchId.ToString(), "false", mySession.Current.SessionDurationHour,
                        objUser.strFirstName, objUser.strSurName);

                    return this.Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("CurrentWrong", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }
    }
}