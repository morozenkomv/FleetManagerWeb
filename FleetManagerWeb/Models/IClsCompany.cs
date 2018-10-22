namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;
    public interface IClsCompany
    {
        ClsCompany GetCompanyByCompanyId(long lgCompanyId);
        bool IsCompanyExists(long lgCompanyId, string ShortName);
        long SaveCompany(ClsCompany objSave);
        List<SearchCompanyResult> SearchCompany(int inRow, int inPage, string strSearch, string strSort);
        DeleteCompanyResult DeleteCompany(string strCompanyId, long lgDeletedBy);
        List<SelectListItem> GetCompanyForDropDown(long? CompanyId);
        List<SelectListItem> GetCompanyForTrackerDropDown();
        List<GetCompanyByUserResult> GetCompanyByUser(long lgUserId, int inRow, int inPage, string strSearch, string strSort);
        List<SelectListItem> GetCompanyForGroupDropDown();
    }
}
