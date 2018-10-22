namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;
    using System.Transactions;

    public partial class ClsUser : IClsUser
    {
        /// <summary>   Context for the object data. </summary>
        private UserDataContext objDataContext = null;

        public bool blIsActive { get; set; }

        public bool blIsLogin { get; set; }

        public bool blRememberMe { get; set; }

        public bool hdniFrame { get; set; }
        public long lgCompanyID { get; set; }

        public long lgBranchId { get; set; }

        public long lgId { get; set; }

        public long lgRoleId { get; set; }

        public long lgVehicleDistributeId { get; set; }

        public List<SelectListItem> lstBranch { get; set; }

        public List<SelectListItem> lstRole { get; set; }
        public long lgAdminUserId { get; set; }
        public int intUnsuccessfulLoginAttempt { get; set; }
        public int intSessionDurationHour { get; set; }
        public int intPasswordExpirationCount { get; set; }
        public DateTime dtPasswordExpireOn { get; set; }

        public List<SelectListItem> lstAdminUser { get; set; }

        public List<ClsUser> lstUser { get; set; }

        public string strAddress { get; set; }

        public string strBranchName { get; set; }

        public string strEmailID { get; set; }

        public string strEmployeeCode { get; set; }

        public string strFirstName { get; set; }

        public string strMobileNo { get; set; }

        public string strPassword { get; set; }
        public string strConfirmPassword { get; set; }

        public string strRoleName { get; set; }

        public string strSurName { get; set; }

        public string strUserName { get; set; }

        public bool blTripStartDate { get; set; }
        public bool blTripEndDate { get; set; }
        public bool blLocationStart { get; set; }
        public bool blLocationEnd { get; set; }
        public bool blTripReasonName { get; set; }
        public bool blReasonRemarks { get; set; }
        public bool blKmStart { get; set; }
        public bool blKmEnd { get; set; }
        public bool blKmDriven { get; set; }
        public bool blFuelStart { get; set; }
        public bool blFuelEnd { get; set; }
        public bool blUsername { get; set; }
        public bool blEntryDatetime { get; set; }
        public bool blEntryMethod { get; set; }
        public bool blEditable { get; set; }
        public bool blActive { get; set; }
        public bool blCompanyName { get; set; }
        public bool blEditColumn { get; set; }
        public bool blDeleteColumn { get; set; }
        public static bool AppStart()
        {
            bool bflag = false;
            try
            {
                int inRowAffected = 0; // objDataContext.UpdateUserIsLogin(0, false);
                if (inRowAffected > 0)
                {
                    bflag = true;
                }
            }
            catch
            {
            }

            return bflag;
        }

        public ClsUser ChangePassword(long lgUserId, string strUserPwd)
        {
            ClsUser objUserMaster = new ClsUser();
            try
            {
                objUserMaster = this.GetUserByUserId(lgUserId);
                if (objUserMaster != null)
                {
                    objUserMaster.strPassword = strUserPwd;
                }
                this.SaveUser(objUserMaster);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objUserMaster;
        }

        public DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    DeleteUserResult result = this.objDataContext.DeleteUser(strUserId, lgDeletedBy, PageMaster.User).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteUserResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public List<SelectListItem> GetAllUserByBranchForDropDown(long lgBranchId)
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetUserAllResult> lstUserResult = this.objDataContext.GetUserAll().ToList();
                    if (lstUserResult != null && lstUserResult.Count > 0)
                    {
                        lstUserResult = lstUserResult.Where(x => x.BranchId == lgBranchId).ToList();
                        if (lstUserResult != null && lstUserResult.Count > 0)
                        {
                            foreach (var item in lstUserResult)
                            {
                                lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<SelectListItem> GetAllUserForDropDown()
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetUserAllResult> lstUserResult = this.objDataContext.GetUserAll().ToList();
                    if (lstUserResult.Count > 0)
                    {
                        foreach (var item in lstUserResult)
                        {
                            lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<SelectListItem> GetAllAdminUserForDropDown()
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    List<GetUserAllResult> lstUserResult = this.objDataContext.GetUserAll().ToList();
                    if (lstUserResult != null && lstUserResult.Count > 0)
                    {
                        lstUserResult = lstUserResult.Where(x => x.RoleId == Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode()).ToList();
                        if (lstUserResult != null && lstUserResult.Count > 0)
                        {
                            foreach (var item in lstUserResult)
                            {
                                lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<SelectListItem> GetUsersOfUserForDropDown()
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    List<GetUsersofUserResult> lstUserResult = this.objDataContext.GetUsersofUser(Common.mySession.Current.UserId).ToList();
                    if (lstUserResult != null && lstUserResult.Count > 0)
                    {
                        foreach (var item in lstUserResult)
                        {
                            lstUser.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<SelectListItem> GetAllAdminUserForDropDownByRole(long RoleId)
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    List<GetAdminUsersByRoleIdResult> lstUserResult = this.objDataContext.GetAdminUsersByRoleId(RoleId).ToList();
                    if (lstUserResult != null && lstUserResult.Count > 0)
                    {
                        foreach (var item in lstUserResult)
                        {
                            lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }
        public List<SelectListItem> GetAllUsersByCompany(long? CompanyId, long? ParentGroupId)
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    //lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetAllUsersByCompanyResult> lstUserResult = this.objDataContext.GetAllUsersByCompany(CompanyId, ParentGroupId, mySession.Current.UserId, mySession.Current.RoleId, Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode(), Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode()).ToList();
                    if (lstUserResult != null && lstUserResult.Count > 0)
                    {
                        foreach (var item in lstUserResult)
                        {
                            lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }
        public List<GetBranchManagerByBranchIdResult> GetBranchManagerByBranchId(long lgBranchId)
        {
            try
            {
                this.objDataContext = new UserDataContext(Functions.StrConnection);
                return this.objDataContext.GetBranchManagerByBranchId(lgBranchId).ToList();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return null;
        }

        public List<GetUserAllResult> GetUserAll()
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    List<GetUserAllResult> lstUserAll = this.objDataContext.GetUserAll().ToList();
                    return lstUserAll;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public ClsUser GetUserByEmailId(string strEmailId)
        {
            ClsUser objClsUser = new ClsUser();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    GetUserByEmailIdResult item = this.objDataContext.GetUserByEmailId(strEmailId).FirstOrDefault();
                    if (item != null)
                    {
                        objClsUser.lgId = item.Id;
                        objClsUser.strEmailID = item.EmailID;
                        objClsUser.strFirstName = item.FirstName;
                        objClsUser.strSurName = item.SurName;
                        objClsUser.strMobileNo = item.MobileNo;
                        objClsUser.strPassword = item.Password;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objClsUser;
        }

        public ClsUser GetUserByUserId(long lgUserId)
        {
            ClsUser objClsUser = new ClsUser();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    GetUserByIdResult item = this.objDataContext.GetUserById(lgUserId).FirstOrDefault();
                    if (item != null)
                    {
                        objClsUser.lgId = item.Id;
                        objClsUser.strFirstName = item.FirstName;
                        objClsUser.strSurName = item.SurName;
                        objClsUser.strMobileNo = item.MobileNo;
                        objClsUser.strEmailID = item.EmailID;
                        objClsUser.strUserName = item.UserName;
                        objClsUser.strPassword = item.Password.DecryptString();
                        objClsUser.strConfirmPassword = item.Password.DecryptString();
                        objClsUser.strAddress = item.Address;
                        objClsUser.lgRoleId = item.RoleId;
                        objClsUser.lgBranchId = item.BranchId;
                        objClsUser.strBranchName = item.UserBranchName;
                        objClsUser.blIsActive = item.IsActive;
                        objClsUser.blIsLogin = item.IsLogin;
                        objClsUser.strEmployeeCode = item.EmployeeCode;
                        objClsUser.lgVehicleDistributeId = item.VehicleDistributeId ?? 0;
                        objClsUser.lgAdminUserId = item.AssignBy;
                        objClsUser.intSessionDurationHour = item.SessionDurationHour;
                        objClsUser.intUnsuccessfulLoginAttempt = item.UnsuccessfulLoginAttempt;
                        objClsUser.intPasswordExpirationCount = item.PasswordExpirationCount;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objClsUser;
        }

        public bool IsUserEmailExists(long lgUserId, string userEmail)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.Users.Where(x => x.Id != lgUserId && x.EmailID == userEmail).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return false;
            }
        }

        public bool IsUserExists(long lgUserId, string userName)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.Users.Where(x => x.Id != lgUserId && x.UserName == userName).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveUser(ClsUser objSave)
        {
            try
            {
                long UserId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                    {
                        objSave.strPassword = objSave.strPassword.EncryptString();
                        var result = this.objDataContext.InsertOrUpdateUser(objSave.lgId, 1, objSave.strFirstName, objSave.strSurName, objSave.strMobileNo, objSave.strEmailID, objSave.strUserName, objSave.strPassword, objSave.strAddress, "EMP/12345", objSave.lgRoleId, objSave.blIsActive, objSave.blIsLogin, mySession.Current.UserId, PageMaster.User, objSave.lgAdminUserId, objSave.intSessionDurationHour, objSave.intUnsuccessfulLoginAttempt, objSave.intPasswordExpirationCount).FirstOrDefault();
                        if (result != null)
                        {
                            UserId = result.InsertedId;
                            this.objDataContext.InsertOrUpdateUserTrackerPermissions(0, UserId, objSave.blTripStartDate, objSave.blTripEndDate, objSave.blLocationStart, objSave.blLocationEnd, objSave.blTripReasonName, objSave.blReasonRemarks, objSave.blKmStart, objSave.blKmEnd, objSave.blKmDriven, objSave.blFuelStart, objSave.blFuelEnd, objSave.blUsername, objSave.blEntryDatetime, objSave.blEntryMethod, objSave.blEditable, objSave.blActive, objSave.blCompanyName, objSave.blEditColumn, objSave.blDeleteColumn, mySession.Current.UserId);
                        }
                        scope.Complete();
                    }
                    return UserId;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return 0;
            }
        }
        public string InserActivityLogOfUser(string strUserName, string Password, string TableName, bool IsUserLogin)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    var result = this.objDataContext.ValidateUserAndInsertActivityLog(strUserName, Password, PageMaster.User, TableName, IsUserLogin).FirstOrDefault();
                    if (result != null)
                    {
                        return result.EmailId;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return string.Empty;
            }
        }


        public List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    int SysAdminRoleID = Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode();
                    int AdminRoleID = Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode();
                    return this.objDataContext.SearchUser(inRow, inPage, strSearch, strSort, mySession.Current.UserId, SysAdminRoleID, AdminRoleID).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public ClsUser ValidateLogin(string strUserName, string strPassword)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    User objUser = this.objDataContext.Users.Where(n => n.UserName == strUserName && n.Password == strPassword && n.IsActive == true).FirstOrDefault();
                    if (objUser != null)
                    {
                        ClsUser objClsUser = new ClsUser();
                        objClsUser.lgId = objUser.Id;
                        objClsUser.lgBranchId = objUser.BranchId;
                        objClsUser.strFirstName = objUser.FirstName;
                        objClsUser.strSurName = objUser.SurName;
                        objClsUser.strMobileNo = objUser.MobileNo;
                        objClsUser.strEmailID = objUser.EmailID;
                        objClsUser.strUserName = objUser.UserName;
                        objClsUser.strPassword = objUser.Password.DecryptString();
                        objClsUser.strAddress = objUser.Address;
                        objClsUser.lgRoleId = objUser.RoleId;
                        objClsUser.blIsActive = objUser.IsActive;
                        objClsUser.blIsLogin = objUser.IsLogin;
                        objClsUser.strEmployeeCode = objUser.EmployeeCode;
                        objClsUser.lgAdminUserId = objUser.AssignBy;
                        objClsUser.intSessionDurationHour = objUser.SessionDurationHour;
                        objClsUser.dtPasswordExpireOn = objUser.PasswordExpireOn;
                        objClsUser.intUnsuccessfulLoginAttempt = objUser.UnsuccessfulLoginAttempt;
                        return objClsUser;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public fnCheckIsUserBlockResult IsUserBlock(string UserName)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    fnCheckIsUserBlockResult UserData = new fnCheckIsUserBlockResult();
                    UserData = this.objDataContext.fnCheckIsUserBlock(UserName).FirstOrDefault();
                    return UserData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public List<GetUsersByGroupResult> GetUsersByGroup(long lgGroupId, int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.GetUsersByGroup(inRow, inPage, strSearch, strSort, lgGroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }
    }
}