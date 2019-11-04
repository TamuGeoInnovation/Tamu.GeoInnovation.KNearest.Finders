using System;
using System.Data;
using System.Data.SqlClient;
//using USC.GISResearchLab.AddressProcessing.Core.AddressNormalization.Implementations;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Databases.QueryManagers;
using USC.GISResearchLab.Common.KNearest.OutputData.WebServices;
using USC.GISResearchLab.Common.Utils.Databases;

namespace USC.GISResearchLab.Common.KNearest.KNearestFinders
{
    [Serializable]
    public class KNearestFinder
    {

        #region Properties

        public DataProviderType DataProviderType { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public double Version
        {
            get
            {
                int major = typeof(KNearestFinder).Assembly.GetName().Version.Major;
                int minor = typeof(KNearestFinder).Assembly.GetName().Version.Minor;
                double minorDecimal = Convert.ToDouble(minor) * 0.1;
                double ret = Convert.ToDouble(major) + minorDecimal;
                return ret;
            }
        }

        public IQueryManager QueryManager
        {
            get { return new QueryManager(DataProviderType, DatabaseType, ConnectionString); }
        }

        #endregion

        public KNearestFinder()
        {
        }

        public KNearestFinder(DataProviderType dataProviderType, DatabaseType databaseType, string connectionString)
        {
            DataProviderType = dataProviderType;
            DatabaseType = databaseType;
            ConnectionString = connectionString;
        }

        public WebServiceKNearestResult GetKNearestRecords(double longitude, double latitude, double k, string table)
        {
            return GetKNearestRecords(longitude, latitude, k, table, 1000);
        }

        public WebServiceKNearestResult GetKNearestRecords(double longitude, double latitude, double k, string table, double strartingThreshold)
        {
            WebServiceKNearestResult ret = new WebServiceKNearestResult();

            try
            {
                if ((latitude <= 90 && latitude >= -90) && (longitude <= 180 && longitude >= -180))
                {
                    DateTime start = DateTime.Now;
                    DataTable dataTable = GetKNearestRecordsAsDataTable(longitude, latitude, k, table, strartingThreshold);
                    DateTime end = DateTime.Now;

                    ret.TimeTaken = end.Subtract(start).TotalMilliseconds;
                    ret.ResultDataTable = dataTable;
                }
            }
            catch (Exception e)
            {
                ret.Exception = e;
                ret.ExceptionOccurred = true;
                ret.Error = e.Message;
            }

            return ret;
        }

        // returns results in meters. Assumes input source database is in EPSG 4269 (NAD83) NOT  EPSG:4326 (WGS 84)
        public DataTable GetKNearestRecordsAsDataTable(double longitude, double latitude, double k, string table, double strartingThreshold)
        {
            DataTable ret = null;

            try
            {
                if ((latitude <= 90 && latitude >= -90) && (longitude <= 180 && longitude >= -180))
                {
                    string sql = "";
                    sql += " USE " + QueryManager.Connection.Database + ";";
                    sql += " SELECT ";
                    sql += "  TOP " + k;
                    sql += "  UniqueId as ID, ";
                    sql += "  X as X, ";
                    sql += "  Y as Y, ";
                    sql += "  Geography::Point(@latitude1, @longitude1, 4269).STDistance(shapeGeog) as distance ";
                    sql += " FROM ";
                    sql += " [" + QueryManager.Connection.Database + "].[dbo]." + table;

                    sql += " WHERE ";

                    sql += "  Geography::Point(@latitude2, @longitude2, 4269).STDistance(shapeGeog) <= @distanceThreshold ";

                    sql += "  ORDER BY ";
                    sql += "  distance ";

                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("latitude1", SqlDbType.Decimal, latitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("longitude1", SqlDbType.Decimal, longitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("latitude2", SqlDbType.Decimal, latitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("longitude2", SqlDbType.Decimal, longitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("distanceThreshold", SqlDbType.Decimal, strartingThreshold));

                    IQueryManager qm = QueryManager;
                    qm.AddParameters(cmd.Parameters);
                    DataTable dataTable = qm.ExecuteDataTable(CommandType.Text, cmd.CommandText, true);

                    if (dataTable != null && dataTable.Rows.Count >= k)
                    {
                        ret = dataTable;
                    }
                    else
                    {
                        double newThreshold = strartingThreshold * strartingThreshold;
                        if (newThreshold < Double.MaxValue)
                        {
                            ret = GetKNearestRecordsAsDataTable(longitude, latitude, k, table, newThreshold);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred GetKNearestRecordsAsDataTable: " + e.Message, e);
            }

            return ret;
        }

        public WebServiceKNearestResult GetKNearestCensusTractRecords(double longitude, double latitude, double k, string table)
        {
            return GetKNearestCensusTractRecords(longitude, latitude, k, table, 1000);
        }

        public WebServiceKNearestResult GetKNearestCensusTractRecords(double longitude, double latitude, double k, string table, double strartingThreshold)
        {
            WebServiceKNearestResult ret = new WebServiceKNearestResult();

            try
            {
                if ((latitude <= 90 && latitude >= -90) && (longitude <= 180 && longitude >= -180))
                {
                    DateTime start = DateTime.Now;
                    DataTable dataTable = GetKNearestCensusTractRecordsAsDataTable(longitude, latitude, k, table, strartingThreshold);
                    DateTime end = DateTime.Now;

                    ret.TimeTaken = end.Subtract(start).TotalMilliseconds;
                    ret.ResultDataTable = dataTable;
                }
            }
            catch (Exception e)
            {
                ret.Exception = e;
                ret.ExceptionOccurred = true;
                ret.Error = e.Message;
            }

            return ret;
        }

        public DataTable GetKNearestCensusTractRecordsAsDataTable(double longitude, double latitude, double k, string table, double strartingThreshold)
        {
            DataTable ret = null;

            try
            {
                if ((latitude <= 90 && latitude >= -90) && (longitude <= 180 && longitude >= -180))
                {
                    string sql = "";
                    sql += " USE " + QueryManager.Connection.Database + ";";
                    sql += " SELECT ";
                    sql += "  TOP " + k;
                    sql += "  TractCe10  as ID, ";
                    sql += "  INTPTLAT10 as Y, ";
                    sql += "  INTPTLON10 as X, ";
                    sql += "  Geography::Point(@latitude1, @longitude1, 4269).STDistance(shapeGeog) as distance, ";
                    sql += "  UniqueId, ";
                    sql += "  stateFp10, ";
                    sql += "  countyFp10, ";
                    sql += "  GEOID10, ";
                    sql += "  Name10, ";
                    sql += "  NameLsad10, ";
                    sql += "  Mtfcc10, ";
                    sql += "  FuncStat10 ";


                    sql += " FROM ";
                    sql += " [" + QueryManager.Connection.Database + "].[dbo]." + table;

                    sql += " WHERE ";

                    sql += "  Geography::Point(@latitude2, @longitude2, 4269).STDistance(shapeGeog) <= @distanceThreshold ";

                    sql += "  ORDER BY ";
                    sql += "  distance ";

                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("latitude1", SqlDbType.Decimal, latitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("longitude1", SqlDbType.Decimal, longitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("latitude2", SqlDbType.Decimal, latitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("longitude2", SqlDbType.Decimal, longitude));
                    cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("distanceThreshold", SqlDbType.Decimal, strartingThreshold));

                    IQueryManager qm = QueryManager;
                    qm.AddParameters(cmd.Parameters);
                    DataTable dataTable = qm.ExecuteDataTable(CommandType.Text, cmd.CommandText, true);

                    if (dataTable != null && dataTable.Rows.Count >= k)
                    {
                        ret = dataTable;
                    }
                    else
                    {
                        double newThreshold = strartingThreshold * strartingThreshold;
                        if (newThreshold < Double.MaxValue)
                        {
                            ret = GetKNearestCensusTractRecordsAsDataTable(longitude, latitude, k, table, newThreshold);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred GetKNearestRecordsAsDataTable: " + e.Message, e);
            }

            return ret;
        }


    }
}
