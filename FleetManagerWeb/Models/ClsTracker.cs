namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;

    public partial class ClsTracker : IClsTracker
    {
        private TrackerDataContext objDataContext = null;

        public bool hdniFrame { get; set; }

        public int inId { get; set; }
        public int inCarId { get; set; }
        public int inCarIdForRegistration { get; set; }
        public int inCodeId { get; set; }
        public long lgReasonId { get; set; }
        public List<SelectListItem> lstTripReason { get; set; }
        public List<ClsTracker> listTracker { get; set; }
        public List<SelectListItem> lstCar { get; set; }
        public List<SelectListItem> lstRegisteration { get; set; }
        public List<SelectListItem> lstCode { get; set; }

        public string strTripStart { get; set; }

        public string strTripEnd { get; set; }

        public string strLocationStart { get; set; }

        public string strLocationEnd { get; set; }

        public string strReasonRemarks { get; set; }

        public int inKmStart { get; set; }

        public int inKmEnd { get; set; }

        public int inKmDriven { get; set; }

        public int inFuelStart { get; set; }

        public int inFuelEnd { get; set; }

        public long lgUserId { get; set; }

        public string strEntryDatetime { get; set; }

        public string strEntryMethod { get; set; }

        public bool blEditable { get; set; }

        public bool blActive { get; set; }
        public long lgCompanyId { get; set; }
        public long lgGroupId { get; set; }
        public List<SelectListItem> lstCompany { get; set; }
        public List<SelectListItem> lstGroup { get; set; }
        public List<GetTrackerAllResult> GetTrackerAll()
        {
            try
            {
                using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                {
                    List<GetTrackerAllResult> lstTrackerAll = this.objDataContext.GetTrackerAll().ToList();
                    return lstTrackerAll;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return null;
            }
        }

        public ClsTracker GetTrackerByTrackerId(long lgTrackerId)
        {
            ClsTracker objClsTracker = new ClsTracker();
            try
            {
                using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                {
                    GetTrackerByIdResult item = this.objDataContext.GetTrackerById(lgTrackerId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsTracker.inId = item.ID;
                        objClsTracker.inCarId = item.Car_Id ?? 0;
                        objClsTracker.strTripStart = item.Trip_Start.Replace(' ', '/');
                        objClsTracker.strTripEnd = item.Trip_End.Replace(' ', '/');
                        objClsTracker.strLocationStart = item.Location_Start;
                        objClsTracker.strLocationEnd = item.Location_End;
                        objClsTracker.strReasonRemarks = item.Reason_Remarks;
                        objClsTracker.inKmStart = item.Km_Start;
                        objClsTracker.inKmEnd = item.Km_End;
                        objClsTracker.inKmDriven = item.Km_Driven;
                        objClsTracker.inFuelStart = item.Fuel_Start;
                        objClsTracker.inFuelEnd = item.Fuel_End;
                        objClsTracker.lgUserId = item.User_Id;
                        objClsTracker.strEntryDatetime = item.Entry_Datetime.Replace(' ', '/');
                        objClsTracker.strEntryMethod = item.Entry_Method;
                        objClsTracker.blEditable = item.Editable;
                        objClsTracker.blActive = item.Active;
                        objClsTracker.lgReasonId = item.Reason_Id ?? 0;
                        objClsTracker.lgCompanyId = item.CompanyId ?? 0;

                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
            }

            return objClsTracker;
        }

        public int SaveTracker(ClsTracker objSave, ClsCarFleet clsCarFleet)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateTracker(objSave.inId, objSave.strTripStart,
                            objSave.strTripEnd, objSave.strLocationStart, objSave.strLocationEnd, 
                            objSave.strReasonRemarks, objSave.inKmStart, objSave.inKmEnd, objSave.inKmDriven,
                            objSave.inFuelStart, objSave.inFuelEnd, mySession.Current.UserId, objSave.strEntryMethod,
                            true, objSave.blActive, PageMaster.Tracker.ToString().intSafe(), objSave.inCarId, 
                            objSave.lgReasonId, null, null, objSave.lgCompanyId, clsCarFleet.strReg, clsCarFleet.strCode)
                            .FirstOrDefault();

                        if (result != null)
                        {
                            objSave.inId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return objSave.inId;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchTrackerResult> SearchTracker(int inRow, int inPage, string strSearch, string strSort, string strTripStartDate, string strTripEndDate, string strLocationStart, string strLocationEnd)
        {
            try
            {
                using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                {
                    int SysAdminRoleID = Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode();
                    int AdminRoleID = Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode();
                    List<SearchTrackerResult> lstSearchTracker = this.objDataContext.SearchTracker(inRow, inPage, strSort, strTripStartDate, strTripEndDate, strLocationStart, strLocationEnd, strSearch, mySession.Current.UserId, SysAdminRoleID, AdminRoleID).ToList();
                    return lstSearchTracker;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return null;
            }
        }

        public List<GenerateTrackerFormattedReportResult> GenerateTrackerFormattedReport(string strTripStartDate, string strTripEndDate, int carId)
        {
            try
            {
                using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                {
                    List<GenerateTrackerFormattedReportResult> lstSearchTracker = this.objDataContext.GenerateTrackerFormattedReport(strTripStartDate, strTripEndDate, carId).ToList();
                    return lstSearchTracker;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return null;
            }
        }



        public List<SearchChangeLogTrackerResult> SearchChangeLogTracker(int inRow, int inPage, string strSearch, string strSort, int inTrackerId)
        {
            try
            {
                using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                {
                    List<SearchChangeLogTrackerResult> lstLog = this.objDataContext.SearchChangeLogTracker(inRow, inPage, strSearch, strSort, inTrackerId).ToList();
                    return lstLog;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return null;
            }
        }
        public DeleteTrackerResult DeleteTracker(string ids, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new TrackerDataContext(Functions.StrConnection))
                {
                    int SysAdminRoleID = Common.Enum.USER_ADMIN_ROLES.SYSADMIN.GetHashCode();
                    int AdminRoleID = Common.Enum.USER_ADMIN_ROLES.ADMIN.GetHashCode();
                    DeleteTrackerResult result = this.objDataContext.DeleteTracker(ids, lgDeletedBy, PageMaster.User, SysAdminRoleID, AdminRoleID).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteTrackerResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }
    }
}