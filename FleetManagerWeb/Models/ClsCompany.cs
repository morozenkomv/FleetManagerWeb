namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;
    public partial class ClsCompany : IClsCompany
    {
        /// <summary>   Context for the object data. </summary>
        private CompanyDataContext objDataContext = null;
        public bool hdniFrame { get; set; }
        public long lgId { get; set; }
        public string strShortName { get; set; }
        public string strFullName { get; set; }
        public string strAddress1 { get; set; }
        public string strAddress2 { get; set; }
        public string strAddress3 { get; set; }
        public int intVat { get; set; }
        public string strEmail { get; set; }
        public string strPerson { get; set; }
        public string strContact { get; set; }
        public string strPhone { get; set; }
        public string strAdminUserIds { get; set; }
        public string hdnstrAdminUserIds { get; set; }

        public List<SelectListItem> lstAdminuser { get; set; }
        public ClsCompany GetCompanyByCompanyId(long lgCompanyId)
        {
            ClsCompany objClsCompany = new ClsCompany();
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    GetCompanyByIdResult item = this.objDataContext.GetCompanyById(lgCompanyId).FirstOrDefault();
                    if (item != null)
                    {
                        objClsCompany.lgId = item.Id;
                        objClsCompany.strShortName = item.ShortName;
                        objClsCompany.strFullName = item.FullName;
                        objClsCompany.strAddress1 = item.Address1;
                        objClsCompany.strAddress2 = item.Address2;
                        objClsCompany.strAddress3 = item.Address3;
                        objClsCompany.intVat = item.Vat;
                        objClsCompany.strEmail = item.Email;
                        objClsCompany.strPerson = item.Person;
                        objClsCompany.strContact = item.Contact;
                        objClsCompany.strPhone = item.Phone;
                        objClsCompany.hdnstrAdminUserIds = item.AdminUserIds;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
            }

            return objClsCompany;
        }
        public List<GetCompanyByUserResult> GetCompanyByUser(long lgUserId, int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.GetCompanyByUser(inRow, inPage, strSearch, strSort,lgUserId).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return null;
            }
        }
        public bool IsCompanyExists(long lgCompanyId, string ShortName)
        {
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.Companies.Where(x => x.Id != lgCompanyId && x.ShortName == ShortName && x.IsDeleted.Trim() != "1").Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveCompany(ClsCompany objSave)
        {
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    var result = this.objDataContext.InsertOrUpdateCompany(objSave.lgId, objSave.strShortName, objSave.strFullName, objSave.strAddress1, objSave.strAddress2, objSave.strAddress3, objSave.intVat, objSave.strEmail, objSave.strPerson, objSave.strContact, objSave.strPhone, objSave.strAdminUserIds, mySession.Current.UserId, PageMaster.Company).FirstOrDefault();
                    if (result != null)
                    {
                        return result.InsertedId;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchCompanyResult> SearchCompany(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    int SysAdminRoleID = Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode();
                    int AdminRoleID = Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode();
                    return this.objDataContext.SearchCompany(inRow, inPage, strSearch, strSort, mySession.Current.UserId, mySession.Current.RoleId, SysAdminRoleID, AdminRoleID).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return null;
            }
        }

        public DeleteCompanyResult DeleteCompany(string strCompanyId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    DeleteCompanyResult result = this.objDataContext.DeleteCompany(strCompanyId, lgDeletedBy, PageMaster.Company,PageMaster.User).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteCompanyResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Company, mySession.Current.UserId);
                return null;
            }
        }

        public List<SelectListItem> GetCompanyForDropDown(long? CompanyId)
        {
            List<SelectListItem> lstRole = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    lstRole.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetCompanyAllResult> lstCompanyResult = this.objDataContext.GetCompanyAll(mySession.Current.UserId,Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode()).ToList();
                    if (lstCompanyResult != null && lstCompanyResult.Count > 0)
                    {
                        if (CompanyId == 0 || CompanyId == null)
                        {
                            lstCompanyResult = lstCompanyResult.ToList();
                        }
                        else
                        {
                            lstCompanyResult = lstCompanyResult.Where(x => x.Id == CompanyId).ToList();
                        }

                        foreach (var item in lstCompanyResult)
                        {
                            lstRole.Add(new SelectListItem { Text = item.ShortName + " (" + item.FullName + ")", Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
            }

            return lstRole;
        }
        public List<SelectListItem> GetCompanyForTrackerDropDown()
        {
            List<SelectListItem> lstRole = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    List<GetCompanyListForTrackerResult> lstCompanyResult = this.objDataContext.GetCompanyListForTracker(mySession.Current.UserId,Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode(), Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode()).ToList();
                    if (lstCompanyResult != null && lstCompanyResult.Count > 0)
                    {
                        foreach (var item in lstCompanyResult)
                        {
                            lstRole.Add(new SelectListItem { Text = item.CompanyName, Value = item.CompanyId.ToString() });
                        }
                    }
                    else
                        lstRole.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
            }

            return lstRole;
        }
        public List<SelectListItem> GetCompanyForGroupDropDown()
        {
            List<SelectListItem> lstRole = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new CompanyDataContext(Functions.StrConnection))
                {
                    lstRole.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });

                    List<GetCompanyListForGroupResult> lstCompanyResult = this.objDataContext.GetCompanyListForGroup(mySession.Current.UserId, Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode(), Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode()).ToList();
                    if (lstCompanyResult != null && lstCompanyResult.Count > 0)
                    {
                        foreach (var item in lstCompanyResult)
                        {
                            lstRole.Add(new SelectListItem { Text = item.CompanyName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
            }

            return lstRole;
        }
    }
}