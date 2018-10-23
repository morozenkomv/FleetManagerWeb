namespace FleetManagerWeb.Common
{
    using System;
    using System.Linq;

    public static class PageMaster
    {
        public static readonly long LgCommon = 1;
        public static readonly long Role = 8;
        public static readonly long User = 11;
        public static readonly long FleetMakes = 101;
        public static readonly long FleetModels = 102;
        public static readonly long FleetColors = 103;
        public static readonly long TripReason = 104;
        public static readonly long Tracker= 105;
        public static readonly long CarFleet = 106;
        public static readonly long Company = 107;
        public static readonly long Group = 108;
        public static readonly long Compose = 109;
        public static readonly long Inbox = 110;
        public static readonly long OrderCategory = 111;
        public static readonly long Order = 112;
        public static readonly long AdditionalCost = 105;
    }

    public enum Page
    {
        Unset = 0,
        AdditionalCost = 105
    }
}