namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;
    public interface IClsGroup
    {
        ClsGroup GetGroupByGroupId(long lgGroupId);
        bool IsGroupExists(long lgGroupId, string GroupName);
        long SaveGroup(ClsGroup objSave);
        List<SearchGroupSubGroupResult> SearchGroup(int inRow, int inPage, string strSearch, string strSort, long ParentGroupId);
        DeleteGroupResult DeleteGroup(string strGroupId, long lgDeletedBy);
        List<SelectListItem> GetParentGroupForDropDown();
        List<SelectListItem> GetGroupByCompanyId(long? CompanyId);
    }

}
