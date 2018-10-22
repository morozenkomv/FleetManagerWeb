namespace FleetManagerWeb.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsTracker
    {
        List<GetTrackerAllResult> GetTrackerAll();

        ClsTracker GetTrackerByTrackerId(long lgTrackerId);

        int SaveTracker(ClsTracker objSave);

        List<SearchTrackerResult> SearchTracker(int inRow, int inPage, string strSearch, string strSort, string strTripStartDate, string strTripEndDate, string strLocationStart, string strLocationEnd);

        List<GenerateTrackerFormattedReportResult> GenerateTrackerFormattedReport(string strTripStartDate, string strTripEndDate, int carId);

        List<SearchChangeLogTrackerResult> SearchChangeLogTracker(int inRow, int inPage, string strSearch, string strSort, int inTrackerId);

        DeleteTrackerResult DeleteTracker(string ids, long lgDeletedBy);
    }
}