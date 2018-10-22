namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsRole
    {
        DeleteRoleResult DeleteRole(string strRoleIdList, long lgDeletedBy);

        List<SelectListItem> GetAllRoleForDropDown(long? CompanyId);

        ClsRole GetRoleByRoleId(long lgRoleId);

        bool IsRoleExists(long lgRoleId, string strRoleName,long lgCompnayId);
        bool IsRoleDeleted(long lgRoleId);
        long SaveRole(ClsRole objSave);

        List<SearchRoleResult> SearchRole(int inRow, int inPage, string strSearch, string strSort);
    }
}