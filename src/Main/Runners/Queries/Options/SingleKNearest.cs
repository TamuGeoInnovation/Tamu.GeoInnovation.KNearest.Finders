namespace USC.GISResearchLab.Census.Runners.Queries.Options
{
    public class SingleKNearest
    {
        #region Properties

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double K { get; set; }
        public double Table { get; set; }
        public string APIKey { get; set; }
        public string WebServiceURL { get; set; }

        #endregion
    }
}
