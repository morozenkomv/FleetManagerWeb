namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsOrder
    {
        DeleteOrderResult DeleteOrder(string strOrderId, long lgDeletedBy);
        long SaveOrder(ClsOrder objSave);
        List<SearchOrderResult> SearchOrder(int inRow, int inPage, string strSearch, string strSort);
        ClsOrder GetOrderById(long lgId);
    }

}