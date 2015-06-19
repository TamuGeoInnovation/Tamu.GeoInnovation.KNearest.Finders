using System;
using System.Data;
using System.Text;
using System.Xml.Serialization;
//using USC.GISResearchLab.Common.Core.JSON;
using USC.GISResearchLab.Core.WebServices.ResultCodes;
using USC.GISResearchLab.Common.Core.JSON;

namespace USC.GISResearchLab.Common.KNearest.OutputData.WebServices
{
    [Serializable]
    public class WebServiceKNearestResult
    {

        #region Properties

        public DataTable ResultDataTable { get; set; }

        public QueryStatusCodes QueryStatusCodes { get; set; }

        public string QueryStatusCodeName
        {
            get { return QueryResultCodeManager.GetStatusCodeName(QueryStatusCodes); }
            set { ; }
        }

        public int QueryStatusCodeValue
        {
            get { return QueryResultCodeManager.GetStatusCodeValue(QueryStatusCodes); }
            set { ; }
        }

        public double Version { get; set; }
        public double TimeTaken { get; set; }

        public string TransactionId { get; set; }
        public bool ExceptionOccurred { get; set; }
        public string Error { get; set; }

        [XmlIgnore]
        public Exception Exception { get; set; }

        #endregion

        public WebServiceKNearestResult()
        {
            QueryStatusCodes = QueryStatusCodes.Unknown;
            TransactionId = "";
            Error = "";

            ResultDataTable = null;
        }

        public string AsString()
        {
            return AsString(",", "|", Version);
        }

        public string AsString(string separator)
        {
            return AsString(separator, "|", Version);
        }

        public string AsString(string valueSeparator, string tupleSeparator, double version)
        {
            string ret = "";
            //ret += TransactionId;

            if (ResultDataTable != null && ResultDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in ResultDataTable.Rows)
                {
                    if (!String.IsNullOrEmpty(ret))
                    {
                        ret += tupleSeparator;
                    }

                    ret += dataRow[0]; // id
                    ret += valueSeparator;
                    ret += dataRow[1]; // lat
                    ret += valueSeparator;
                    ret += dataRow[2]; // lon
                    ret += valueSeparator;
                    ret += dataRow[3]; // distance
                }
            }
            return ret;
        }

        public string ToJSON()
        {
            StringBuilder sb = new StringBuilder();


            sb.Append("{").AppendLine();
            sb.Append("\t\"TransactionId\" : \"").Append(JSONUtils.CleanText(TransactionId)).Append("\",").AppendLine();
            sb.Append("\t\"Version\"   : \"").Append(JSONUtils.CleanText(Version)).Append("\",").AppendLine();
            sb.Append("\t\"QueryStatusCode\" : \"").Append(JSONUtils.CleanText(QueryStatusCodes.ToString())).Append("\",").AppendLine();
            sb.Append("\t\"TimeTaken\" : \"").Append(JSONUtils.CleanText(TimeTaken)).Append("\",").AppendLine();
            sb.Append("\t\"Exception\" : \"").Append(JSONUtils.CleanText(Exception)).Append("\",").AppendLine();
            sb.Append("\t\"ErrorMessage\" : \"").Append(JSONUtils.CleanText(Error)).Append("\",").AppendLine();
            sb.Append("\t\"NearestFeatures\" : ").AppendLine();
            sb.Append("\t[").AppendLine();
            if (ResultDataTable != null && ResultDataTable.Rows.Count > 0)
            {
                for( int i=0; i<ResultDataTable.Rows.Count; i++)
                {
                    DataRow dataRow = ResultDataTable.Rows[i];

                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append("\t{").AppendLine();
                    sb.Append("\t\t\"TransactionId\" : \"").Append(JSONUtils.CleanText(TransactionId)).Append("\",").AppendLine();
                    sb.Append("\t\t\"Version\"   : \"").Append(JSONUtils.CleanText(Version)).Append("\",").AppendLine();
                    sb.Append("\t\t\"QueryStatusCode\" : \"").Append(JSONUtils.CleanText(QueryStatusCodes.ToString())).Append("\",").AppendLine();
                    sb.Append("\t\t\"TimeTaken\" : \"").Append(JSONUtils.CleanText(TimeTaken)).Append("\",").AppendLine();
                    sb.Append("\t\t\"Exception\" : \"").Append(JSONUtils.CleanText(Exception)).Append("\",").AppendLine();
                    sb.Append("\t\t\"ErrorMessage\" : \"").Append(JSONUtils.CleanText(Error)).Append("\",").AppendLine();
                    sb.Append("\t\t\"FeatureId\" : \"").Append(JSONUtils.CleanText(Convert.ToString(dataRow[0]))).Append("\",").AppendLine();
                    sb.Append("\t\t\"Latitude\" : \"").Append(JSONUtils.CleanText(Convert.ToString(dataRow[1]))).Append("\",").AppendLine();
                    sb.Append("\t\t\"Longitude\" : \"").Append(JSONUtils.CleanText(Convert.ToString(dataRow[2]))).Append("\",").AppendLine();
                    sb.Append("\t\t\"Distance\" : \"").Append(JSONUtils.CleanText(Convert.ToString(dataRow[3]))).Append("\"").AppendLine();
                    sb.Append("\t}").AppendLine();

                }
            }

            sb.Append("\t]").AppendLine();
            sb.Append("}").AppendLine();


            return sb.ToString();
        }

        public string AsStringVerbose(string valueSeparator, string tupleSeparator, string toupleValueSeperator, double version)
        {
            string ret = "";
            ret += TransactionId;
            ret += valueSeparator;
            ret += Version;
            ret += valueSeparator;
            ret += QueryStatusCodeValue;
            ret += valueSeparator;
            ret += TimeTaken;
            ret += valueSeparator;
            ret += Error;
            ret += valueSeparator;

            if (ResultDataTable != null && ResultDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in ResultDataTable.Rows)
                {
                    if (!String.IsNullOrEmpty(ret))
                    {
                        ret += tupleSeparator;
                    }

                    ret += dataRow[0]; // id
                    ret += toupleValueSeperator;
                    ret += dataRow[1]; // lat
                    ret += toupleValueSeperator;
                    ret += dataRow[2]; // lon
                    ret += toupleValueSeperator;
                    ret += dataRow[3]; // distance
                }
            }
            return ret;
        }

        public override string ToString()
        {
            return AsString();
        }

        
    }
}
