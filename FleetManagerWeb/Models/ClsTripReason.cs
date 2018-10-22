﻿namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public partial class ClsTripReason : IClsTripReason
    {
        private TripReasonDataContext objDataContext = null;

        public bool hdniFrame { get; set; }

        public long lgId { get; set; }

        public List<ClsTripReason> listTripReason { get; set; }

        public string strTripReasonName { get; set; }

        public DeleteTripReasonResult DeleteTripReason(string strTripReasonIdList, long lgDeletedBy)
        {
            DeleteTripReasonResult result = new DeleteTripReasonResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                    {
                        result = this.objDataContext.DeleteTripReason(strTripReasonIdList, lgDeletedBy, PageMaster.TripReason).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
            }

            return result;
        }

        public List<SelectListItem> GetAllTripReasonForDropDown()
        {
            List<SelectListItem> lstTripReason = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                {
                    lstTripReason.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetTripReasonAllResult> lstTripReasonResult = this.objDataContext.GetTripReasonAll().ToList();
                    if (lstTripReasonResult != null && lstTripReasonResult.Count > 0)
                    {
                        foreach (var item in lstTripReasonResult)
                        {
                            lstTripReason.Add(new SelectListItem { Text = item.TripReasonName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
            }

            return lstTripReason;
        }

        public List<GetTripReasonAllResult> GetTripReasonAll()
        {
            try
            {
                using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                {
                    List<GetTripReasonAllResult> lstTripReasonAll = this.objDataContext.GetTripReasonAll().ToList();
                    return lstTripReasonAll;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return null;
            }
        }

        public ClsTripReason GetTripReasonByTripReasonId(long lgTripReasonId)
        {
            ClsTripReason objClsTripReason = new ClsTripReason();
            try
            {
                using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                {
                    GetTripReasonByIdResult item = this.objDataContext.GetTripReasonById(lgTripReasonId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsTripReason.lgId = item.Id;
                        objClsTripReason.strTripReasonName = item.TripReasonName;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
            }

            return objClsTripReason;
        }

        public bool IsTripReasonExists(long lgTripReasonId, string strTripReasonName)
        {
            try
            {
                using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.TripReason.Where(x => x.Id != lgTripReasonId && x.TripReasonName == strTripReasonName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveTripReason(ClsTripReason objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateTripReason(objSave.lgId, objSave.strTripReasonName, mySession.Current.UserId, PageMaster.TripReason).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.lgId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return objSave.lgId;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchTripReasonResult> SearchTripReason(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new TripReasonDataContext(Functions.StrConnection))
                {
                    List<SearchTripReasonResult> lstSearchTripReason = this.objDataContext.SearchTripReason(inRow, inPage, strSearch, strSort).ToList();
                    return lstSearchTripReason;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return null;
            }
        }
    }
}