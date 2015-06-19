namespace USC.GISResearchLab.Common.KNearest.Queries
{
    public class KNearestFinderQuery
    {
        #region Properties

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double K { get; set; }
        public string Table { get; set; }
        public double Version { get; set; }
        public string UserAPIKey { get; set; }
        public bool ShouldNotStore { get; set; }

        #endregion


        public KNearestFinderQuery()
        {
        }

        public KNearestFinderQuery(double latitude, double longitude, double k, string table, double version, string userAPIKey, bool shouldNotStore)
        {
            Latitude = latitude;
            Longitude = longitude;
            K = k;
            Table = table;
            Version = version;
            UserAPIKey = userAPIKey;
            ShouldNotStore = shouldNotStore;
        }
    }
}
