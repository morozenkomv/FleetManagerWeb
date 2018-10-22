namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;
    using System.Transactions;

    public partial class ClsGroup : IClsGroup
    {
        /// <summary>   Context for the object data. </summary>
        private GroupsDataContext objDataContext = null;
        public bool hdniFrame { get; set; }
        public long lgId { get; set; }
        public string strGroupName { get; set; }
        public long lgCompanyId { get; set; }
        public long lgParentGroupId { get; set; }
        public string strUserId { get; set; }
        public string hdnstrUserIds { get; set; }

        public List<SelectListItem> lstGroups { get; set; }
        public List<SelectListItem> lstCompany { get; set; }
        public List<SelectListItem> lstUsers { get; set; }
        public ClsGroup GetGroupByGroupId(long lgGroupId)
        {
            ClsGroup objClsGroup = new ClsGroup();
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    GetGroupByIdResult item = this.objDataContext.GetGroupById(lgGroupId).FirstOrDefault();
                    if (item != null)
                    {
                        objClsGroup.lgId = item.Id;
                        objClsGroup.strGroupName = item.GroupName;
                        objClsGroup.lgCompanyId = item.CompanyId;
                        objClsGroup.lgParentGroupId = item.ParentGroupId;
                        objClsGroup.hdnstrUserIds = item.UserIds;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
            }

            return objClsGroup;
        }
        public bool IsGroupExists(long lgGroupId, string GroupName)
        {
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.Groups.Where(x => x.Id != lgGroupId && x.GroupName == GroupName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveGroup(ClsGroup objSave)
        {
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    var result = this.objDataContext.InsertOrUpdateGroup(objSave.lgId, objSave.strGroupName, objSave.lgCompanyId, objSave.lgParentGroupId, objSave.hdnstrUserIds, mySession.Current.UserId, PageMaster.Group).FirstOrDefault();
                    if (result != null)
                    {
                        return result.InsertedId;
                    }
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return 0;
            }
        }
        public List<SearchGroupSubGroupResult> SearchGroup(int inRow, int inPage, string strSearch, string strSort, long ParentGroupId)
        {
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    int SysAdminRoleID = Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode();
                    int AdminRoleID = Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode();
                    return this.objDataContext.SearchGroupSubGroup(inRow, inPage, strSearch, strSort, mySession.Current.UserId, ParentGroupId, mySession.Current.RoleId, SysAdminRoleID, AdminRoleID).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return null;
            }
        }

        public DeleteGroupResult DeleteGroup(string strGroupId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    DeleteGroupResult result = this.objDataContext.DeleteGroup(strGroupId, lgDeletedBy, PageMaster.Group).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteGroupResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
                return null;
            }
        }

        public List<SelectListItem> GetParentGroupForDropDown()
        {
            List<SelectListItem> lstgroup = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    lstgroup.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetParentGroupListForGroupResult> lstGroupResult = this.objDataContext.GetParentGroupListForGroup(mySession.Current.UserId, Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode(), Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode()).ToList();
                    if (lstGroupResult != null && lstGroupResult.Count > 0)
                    {
                        foreach (var item in lstGroupResult)
                        {
                            lstgroup.Add(new SelectListItem { Text = item.GroupName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
            }

            return lstgroup;
        }

        public List<SelectListItem> GetGroupByCompanyId(long? CompanyId)
        {
            List<SelectListItem> lstGroup = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new GroupsDataContext(Functions.StrConnection))
                {
                    lstGroup.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetGroupByCompanyIdResult> lstGroupResult = this.objDataContext.GetGroupByCompanyId(CompanyId).ToList();
                    if (lstGroupResult.Count > 0)
                    {
                        foreach (var item in lstGroupResult)
                        {
                            lstGroup.Add(new SelectListItem { Text = item.GroupName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Group, mySession.Current.UserId);
            }

            return lstGroup;
        }
    }
}