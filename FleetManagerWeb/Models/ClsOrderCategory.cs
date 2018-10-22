namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;
    using System.Transactions;

    public partial class ClsOrderCategory : IClsOrderCategory
    {
        /// <summary>   Context for the object data. </summary>
        private OrderCategoryDataContext objDataContext = null;

        public bool hdniFrame { get; set; }
        public long lgId { get; set; }
        public string strName { get; set; }
        public bool blIsActive { get; set; }
            
        public DeleteOrderCategoryResult DeleteOrderCategory(string strOrderCategoryId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new OrderCategoryDataContext(Functions.StrConnection))
                {
                    DeleteOrderCategoryResult result = this.objDataContext.DeleteOrderCategory(strOrderCategoryId, lgDeletedBy, PageMaster.OrderCategory).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteOrderCategoryResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return null;
            }
        }
        public long SaveOrderCategory(ClsOrderCategory objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new OrderCategoryDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateOrderCategory(objSave.lgId, objSave.strName, mySession.Current.UserId, objSave.blIsActive, PageMaster.OrderCategory.ToString().intSafe()).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchOrderCategoryResult> SearchOrderCategory(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new OrderCategoryDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.SearchOrderCategory(inRow, inPage, strSort, strSearch).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return null;
            }
        }

        public bool IsOrderCategoryExists(long lgOrderCategoryId, string strName)
        {
            try
            {
                using (this.objDataContext = new OrderCategoryDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.OrderCategories.Where(x => x.Id != lgOrderCategoryId && x.Name == strName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
                return false;
            }
        }

        public ClsOrderCategory GetOrderCategoryById(long lgId)
        {
            ClsOrderCategory objClsOrderCategory = new ClsOrderCategory();
            try
            {
                using (this.objDataContext = new OrderCategoryDataContext(Functions.StrConnection))
                {
                    GetOrderCategoryByIdResult item = this.objDataContext.GetOrderCategoryById(lgId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsOrderCategory.lgId = item.Id;
                        objClsOrderCategory.strName = item.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
            }

            return objClsOrderCategory;
        }

        public List<SelectListItem> GetAllOrderCategoryForDropDown()
        {
            List<SelectListItem> lstOrderCategory = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new OrderCategoryDataContext(Functions.StrConnection))
                {
                    lstOrderCategory.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetOrderCategoryAllResult> lstOrderCategoryResult = this.objDataContext.GetOrderCategoryAll().ToList();
                    if (lstOrderCategoryResult != null && lstOrderCategoryResult.Count > 0)
                    {
                        foreach (var item in lstOrderCategoryResult)
                        {
                            lstOrderCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.OrderCategory, mySession.Current.UserId);
            }

            return lstOrderCategory;
        }

    }
}