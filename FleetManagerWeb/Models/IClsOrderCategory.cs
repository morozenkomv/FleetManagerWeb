namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsOrderCategory
    {
        DeleteOrderCategoryResult DeleteOrderCategory(string strOrderCategoryId, long lgDeletedBy);
        long SaveOrderCategory(ClsOrderCategory objSave);
        List<SearchOrderCategoryResult> SearchOrderCategory(int inRow, int inPage, string strSearch, string strSort);
        bool IsOrderCategoryExists(long lgOrderCategoryId, string strName);
        ClsOrderCategory GetOrderCategoryById(long lgId);
        List<SelectListItem> GetAllOrderCategoryForDropDown();
    }

}