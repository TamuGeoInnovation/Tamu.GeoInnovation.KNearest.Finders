using System.Collections.Generic;
using USC.GISResearchLab.Common.Databases.FieldMappings;
using USC.GISResearchLab.Common.Databases.TypeConverters;

namespace USC.GISResearchLab.Common.KNearest.Metadata.OutputFields
{
    public class OutputCensusFields
    {

        public static List<DatabaseFieldMapping> GetOutputFields()
        {
            return GetOutputFields(null);
        }

        public static List<DatabaseFieldMapping> GetOutputFields(string prefix)
        {
            List<DatabaseFieldMapping> ret = new List<DatabaseFieldMapping>();

            ret.Add(new DatabaseFieldMapping(prefix + "Block", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "BlockGroup", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "Tract", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "CountyFips", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "PlaceFips", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "MSAFips", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "MCDFips", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "CBSAFips", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "CBSAMicro", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "MetDivFips", DatabaseSuperDataType.String, 20));
            ret.Add(new DatabaseFieldMapping(prefix + "StateFips", DatabaseSuperDataType.String, 20));

            return ret;
        }
    }
}
