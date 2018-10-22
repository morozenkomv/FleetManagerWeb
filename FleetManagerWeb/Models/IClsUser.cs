namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsUser
    {
        ClsUser ChangePassword(long lgUserId, string strUserPwd);
        DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy);
        List<SelectListItem> GetAllUserByBranchForDropDown(long lgBranchId);
        List<SelectListItem> GetAllUserForDropDown();
        List<SelectListItem> GetAllAdminUserForDropDown();
        List<SelectListItem> GetUsersOfUserForDropDown();
        List<GetBranchManagerByBranchIdResult> GetBranchManagerByBranchId(long lgBranchId);
        List<GetUserAllResult> GetUserAll();
        List<SelectListItem> GetAllAdminUserForDropDownByRole(long RoleId);
        ClsUser GetUserByEmailId(string strEmailId);
        ClsUser GetUserByUserId(long lgUserId);
        bool IsUserEmailExists(long lgUserId, string userEmail);
        bool IsUserExists(long lgUserId, string userName);
        long SaveUser(ClsUser objSave);
        List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort);
        ClsUser ValidateLogin(string strUserName, string strPassword);
        List<SelectListItem> GetAllUsersByCompany(long? CompanyId, long? ParentGroupId);
        List<GetUsersByGroupResult> GetUsersByGroup(long lgGroupId, int inRow, int inPage, string strSearch, string strSort);
        string InserActivityLogOfUser(string strUserName, string Password, string TableName, bool IsUserLogin);
        fnCheckIsUserBlockResult IsUserBlock(string UserName);
    }
}