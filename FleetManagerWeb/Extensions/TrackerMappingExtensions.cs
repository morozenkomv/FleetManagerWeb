using FleetManagerWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace FleetManagerWeb.Extensions
{
    public static class TrackerMappingExtensions
    {
        public static List<TrackerFormattedModel> ToReportList(this List<GenerateTrackerFormattedReportResult> tracks)
        {
            GenerateTrackerFormattedReportResult prevTrack = null;
            return tracks.OrderBy(_ => _.Trip_Start).Select(_ =>
            {
                var model = new TrackerFormattedModel
                {
                    Desc = _.Desc,
                    Trip_Start = _.Trip_Start.ToString("dd/MM/yyyy"),
                    Trip_End = _.Trip_End.ToString("dd/MM/yyyy"),
                    Location_Start = _.Location_Start,
                    Location_End = _.Location_End,
                    TripReasonName = _.TripReasonName,
                    Reason_Remarks = _.Reason_Remarks,
                    Km_Start = _.Km_Start,
                    Km_End = _.Km_End,
                    Km_Driven = _.Km_Driven,
                    IsValid = true
                };

                if (prevTrack == null)
                {
                    prevTrack = _;
                    return model;
                }

                if (prevTrack.Trip_End > _.Trip_Start)
                    model.IsValid = false;

                if (prevTrack.Km_End > _.Km_Start)
                    model.IsValid = false;

                prevTrack = _;
                return model;
            }).ToList();
        }
    }
}