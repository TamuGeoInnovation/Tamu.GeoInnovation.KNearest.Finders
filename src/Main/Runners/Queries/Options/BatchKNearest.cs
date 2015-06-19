using System;

namespace USC.GISResearchLab.Census.Runners.Queries.Options
{
    [Serializable]
    public class BatchKNearest : BatchKNearestOptions
    {
        #region Properties


        public string FieldLatitude { get; set; }
        public string FieldLongitude { get; set; }

        public double K { get; set; }
        public string FeatureTable { get; set; }
        public string APIKey { get; set; }
        public string WebServiceURL { get; set; }


        #endregion

    }
}
