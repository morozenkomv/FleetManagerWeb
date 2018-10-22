namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;
    using System.Transactions;

    public partial class ClsOrder : IClsOrder
    {
        /// <summary>   Context for the object data. </summary>
        private OrderDataContext objDataContext = null;

        public bool hdniFrame { get; set; }
        public long lgId { get; set; }
        public string strName { get; set; }
        public string strUserId { get; set; }
        public string hdnstrUserIds { get; set; }
        public long lgOrderCategoryId { get; set; }
        public List<SelectListItem> lstOrderCategory { get; set; }
        public List<SelectListItem> lstUsers { get; set; }

        public DeleteOrderResult DeleteOrder(string strOrderId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new OrderDataContext(Functions.StrConnection))
                {
                    DeleteOrderResult result = this.objDataContext.DeleteOrder(strOrderId, lgDeletedBy, PageMaster.OrderCategory).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteOrderResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return null;
            }
        }
        public long SaveOrder(ClsOrder objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new OrderDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateOrder(objSave.lgId, objSave.strName, mySession.Current.UserId, objSave.lgOrderCategoryId, objSave.hdnstrUserIds, PageMaster.Order.ToString().intSafe()).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.lgId = result.InsertedId;
                        }
                        scope.Complete();
                    }
                    return objSave.lgId;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchOrderResult> SearchOrder(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new OrderDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.SearchOrder(inRow, inPage, strSort, strSearch).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
                return null;
            }
        }

        public ClsOrder GetOrderById(long lgId)
        {
            ClsOrder objClsOrder = new ClsOrder();
            try
            {
                using (this.objDataContext = new OrderDataContext(Functions.StrConnection))
                {
                    GetOrderByIdResult item = this.objDataContext.GetOrderById(lgId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsOrder.lgId = item.Id;
                        objClsOrder.strName = item.Name;
                        objClsOrder.lgOrderCategoryId = item.OrderCategoryId ?? 0;
                        objClsOrder.hdnstrUserIds = item.UserIds;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Order, mySession.Current.UserId);
            }

            return objClsOrder;
        }
    }
}