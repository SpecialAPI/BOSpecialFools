using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class Zones
    {
        public const string ShoreEasyID     = "ZoneDB_01";
        public const string OrpheumEasyID   = "ZoneDB_02";
        public const string QuarryID        = "ZoneDB_03";

        public static readonly ZoneBGDataBaseSO ShoreEasy       = LoadZoneDBData(ShoreEasyID) as ZoneBGDataBaseSO;
        public static readonly ZoneBGDataBaseSO OrpheumEasy     = LoadZoneDBData(OrpheumEasyID) as ZoneBGDataBaseSO;
        public static readonly ZoneBGDataBaseSO Quarry          = LoadZoneDBData(QuarryID) as ZoneBGDataBaseSO;

        public const string ShoreHardID     = "ZoneDB_Hard_01";
        public const string OrpheumHardID   = "ZoneDB_Hard_02";
        public const string GardenID        = "ZoneDB_Hard_03";

        public static readonly ZoneBGDataBaseSO ShoreHard       = LoadZoneDBData(ShoreHardID) as ZoneBGDataBaseSO;
        public static readonly ZoneBGDataBaseSO OrpheumHard     = LoadZoneDBData(OrpheumHardID) as ZoneBGDataBaseSO;
        public static readonly ZoneBGDataBaseSO Garden          = LoadZoneDBData(GardenID) as ZoneBGDataBaseSO;
    }
}
