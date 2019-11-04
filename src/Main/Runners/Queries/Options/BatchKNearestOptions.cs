using System;
using USC.GISResearchLab.Common.Databases.Runners.Options;

namespace USC.GISResearchLab.Census.Runners.Queries.Options
{
    [Serializable]
    public class BatchKNearestOptions : BatchDatabaseWebOptions
    {
        #region Properties

        public double Version { get; set; }

        public string FieldVersion { get; set; }

        public string FieldNearestId1 { get; set; }
        public string FieldNearestLatitude1 { get; set; }
        public string FieldNearestLongitude1 { get; set; }
        public string FieldNearestDistance1 { get; set; }


        #endregion
    }
}
