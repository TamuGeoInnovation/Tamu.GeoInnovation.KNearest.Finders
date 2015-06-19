using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using USC.GISResearchLab.Census.Runners.Queries.Options;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Core.Threading.ThreadPoolWaits;
using USC.GISResearchLab.Common.Databases.QueryManagers;
using USC.GISResearchLab.Common.Databases.Runners.AbstractClasses;
using USC.GISResearchLab.Common.Diagnostics.TraceEvents;
using USC.GISResearchLab.Common.KNearest.KNearestFinders;
using USC.GISResearchLab.Common.KNearest.OutputData.WebServices;
using USC.GISResearchLab.Common.Threading.ProgressStates;
using USC.GISResearchLab.Common.Utils.Databases;

namespace USC.GISResearchLab.Census.Runners.Databases
{
    public class RecordDataItem
    {
        #region Properties

        public object RecordId { get; set; }
        public object[] DataItems { get; set; }

        #endregion

        public RecordDataItem() { }

        public RecordDataItem(object recordId, object[] dataItems)
        {
            RecordId = recordId;
            DataItems = dataItems;
        }
    }

    public class KNearestDatabaseRunner : AbstractTraceableBackgroundWorkableWebStatusReportableDatabaseRunner
    {
        #region Properties

        public KNearestFinder KNearestFinder
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public KNearestDatabaseRunner()
            : base()
        {
            RunnerName = "USCKNearest";
        }

        public KNearestDatabaseRunner(TraceSource traceSource) : base(traceSource)
        {
            RunnerName = "USCKNearest";
        }

        public KNearestDatabaseRunner(BackgroundWorker backgroundWorker) : base(backgroundWorker)
        {
            RunnerName = "USCKNearest";
        }

        public KNearestDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource)
            : base(backgroundWorker, traceSource)
        {
            RunnerName = "USCKNearest";
        }


        #endregion

        #region GetWorkFunctions

        public override IDataReader GetWorkAsDataReader(bool shouldOpenClose)
        {
            IDataReader ret = null;
            try
            {
                if (shouldOpenClose)
                {
                    DBManagerInputData.Open();
                }


                BatchKNearest args = (BatchKNearest)BatchDatabaseOptions;

                string sql = "SELECT ";
                sql += " " + DatabaseUtils.AsDbColumnName(args.FieldId) + " " + " AS [Id], ";
                sql += " " + DatabaseUtils.AsDbColumnName(args.FieldLatitude) + " " + " AS [Latitude], ";
                sql += " " + DatabaseUtils.AsDbColumnName(args.FieldLongitude) + " " + " AS [Longitude] ";
                sql += " FROM " + DatabaseUtils.AsDbTableName(args.Table, true) + " ";

                if (BatchDatabaseOptions.NonProcessedOnly || BatchDatabaseOptions.ShouldFilterInputData)
                {
                    sql += " WHERE ";

                    if (args.NonProcessedOnly)
                    {
                        sql += " (" + DatabaseUtils.AsDbColumnName(args.FieldProcessed) + " <> 1 or  " + DatabaseUtils.AsDbColumnName(args.FieldProcessed) + " is null)";
                    }

                    if (BatchDatabaseOptions.ShouldFilterInputData)
                    {

                        if (BatchDatabaseOptions.NonProcessedOnly)
                        {
                            sql += " AND ";
                        }

                        sql += " (" + DatabaseUtils.AsDbColumnName(BatchDatabaseOptions.FieldFilterField) + " " + BatchDatabaseOptions.FieldFilterValue + " )";
                    }
                }

                if (args.ShouldOrderWorkByIdField)
                {
                    sql += " ORDER BY " + DatabaseUtils.AsDbColumnName(args.FieldId) + " ASC";
                }

                ret = DBManagerInputData.ExecuteReader(CommandType.Text, sql, false);
            }
            catch (ThreadAbortException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                if (DBManagerInputData != null)
                {
                    if (DBManagerInputData.Connection != null)
                    {
                        if (DBManagerInputData.Connection.State != ConnectionState.Closed)
                        {
                            if (!DBManagerInputDataClosed)
                            {
                                DBManagerInputData.Connection.Close();
                                DBManagerInputDataClosed = true;
                            }
                        }
                    }

                    DBManagerInputData.Dispose();
                    DBManagerInputData = null;
                }

                throw new Exception("Error occured getting work: " + e.Message, e);
            }
            finally
            {
                if (shouldOpenClose)
                {
                    if (DBManagerInputData != null)
                    {
                        if (DBManagerInputData.Connection != null)
                        {
                            if (DBManagerInputData.Connection.State != ConnectionState.Closed)
                            {
                                if (!DBManagerInputDataClosed)
                                {
                                    DBManagerInputData.Connection.Close();
                                    DBManagerInputDataClosed = true;
                                }
                            }
                        }

                        DBManagerInputData.Dispose();
                        DBManagerInputData = null;
                    }
                }
            }
            return ret;
        }

        #endregion

        public override bool ProcessRecords(DataTable dataTable)
        {
            return ProcessRecords(dataTable.CreateDataReader());
        }

        public bool ProcessRecordsMultiThreaded(IDataReader dataReader)
        {
            BatchKNearest args = (BatchKNearest)BatchDatabaseOptions;

            bool ret = false;
            try
            {

                if (args.ShouldLeaveDatabaseConnectionOpen)
                {
                    if (DBManagerInputDataUpdate.Connection.State != ConnectionState.Open)
                    {
                        DBManagerInputDataUpdate.Connection.Open();
                    }
                }

                ProgressState = new PercentCompletableProgressState();

                if (dataReader != null)
                {

                    

                    while (dataReader.Read())
                    {
                        if (!ShouldStop)
                        {
                            bool shouldQueue = false;

                            ThreadPoolWait tpw = new ThreadPoolWait();
                            for (int i = 0; i < args.MaxNumberOfThreads; i++)
                            {
                                if (!ShouldStop)
                                {

                                    if (i == 0)
                                    {
                                        shouldQueue = true;
                                    }
                                    else
                                    {
                                        shouldQueue = dataReader.Read();
                                    }

                                    if (shouldQueue)
                                    {
                                        string id = DatabaseUtils.StringIfNull(dataReader["Id"]);
                                        double longitude = DatabaseUtils.DoubleIfNull(dataReader["longitude"]);
                                        double latitude = DatabaseUtils.DoubleIfNull(dataReader["latitude"]);
                                        

                                        if (!String.IsNullOrEmpty(id) && longitude != 0 && latitude != 0)
                                        {
                                            object[] dataItems = new object[3];
                                            dataItems[0] = longitude;
                                            dataItems[1] = latitude;
                                            

                                            RecordDataItem recordDataItem = new RecordDataItem(id, dataItems);

                                            tpw.QueueUserWorkItem(new WaitCallback(ProcessRecordInQueue), recordDataItem);

                                        }
                                    }
                                }
                                else
                                {
                                    tpw.Dispose();
                                    SignalCancelled();
                                    ret = false;
                                    break;
                                }
                            }

                            if (shouldQueue)
                            {
                                tpw.WaitOne();
                            }

                            RecordsCompleted += args.MaxNumberOfThreads;
                            UpdateProcessingStatus(RecordsTotal, RecordsCompleted);

                            tpw.Dispose();

                        }
                        else
                        {
                            SignalCancelled();
                            ret = false;
                            break;
                        }

                    }

                    ret = true;

                }

                if (!ShouldStop)
                {
                    UpdateProcessingStatusFinished();
                }
                else
                {
                    SignalCancelled();
                    ret = false;
                }

            }
            catch (Exception e)
            {
                UpdateProcessingStatusAborted(e);

                if (dataReader != null)
                {
                    if (!dataReader.IsClosed)
                    {
                        dataReader.Close();
                    }
                }

                if (args.ShouldLeaveDatabaseConnectionOpen)
                {
                    try
                    {
                        if (DBManagerInputDataUpdate.Connection != null)
                        {
                            if (DBManagerInputDataUpdate.Connection.State != ConnectionState.Closed)
                            {
                                DBManagerInputDataUpdate.Connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception closing input database: " + ex.Message, ex);
                    }
                }

                throw new Exception("Error occured processing records: " + e.Message, e);
            }
            finally
            {
                if (dataReader != null)
                {
                    if (!dataReader.IsClosed)
                    {
                        dataReader.Close();
                    }
                }

                if (args.ShouldLeaveDatabaseConnectionOpen)
                {
                    try
                    {
                        if (DBManagerInputDataUpdate.Connection != null)
                        {
                            if (DBManagerInputDataUpdate.Connection.State != ConnectionState.Closed)
                            {
                                DBManagerInputDataUpdate.Connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception closing input database: " + ex.Message, ex);
                    }
                }
            }

            return ret;
        }


        public void ProcessRecordInQueue(object o)
        {
            
            try
            {

                RecordDataItem recordDataItem = (RecordDataItem)o;

                if (TraceSource != null)
                {
                    TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Running, "{0}: {1} {2} {3}", new object[] { recordDataItem.RecordId, recordDataItem.DataItems[0], recordDataItem.DataItems[1], recordDataItem.DataItems[2] });
                }

                WebServiceKNearestResult result = (WebServiceKNearestResult)ProcessRecord(recordDataItem.RecordId, recordDataItem.DataItems);

                if (TraceSource != null)
                {
                    TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Running, " - {0}, {1}, {2}",
                     new object[] { 
                        "200", 
                        "Success", 
                        result.ToString()
                        });
                }

                UpdateRecord(recordDataItem.RecordId, result);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception occurred in ProcessRecordInQueue: " + ex.Message, ex);
            }
        }

        public override bool ProcessRecords(IDataReader dataReader)
        {
            BatchKNearest args = (BatchKNearest)BatchDatabaseOptions;

            bool ret = false;
            try
            {
                if (args.ShouldLeaveDatabaseConnectionOpen)
                {
                    if (DBManagerInputDataUpdate.Connection.State != ConnectionState.Open)
                    {
                        DBManagerInputDataUpdate.Connection.Open();
                    }
                }

                ProgressState = new PercentCompletableProgressState();

                if (dataReader != null)
                {
                    while(dataReader.Read())
                    {
                        if (!ShouldStop)
                        {
                            string id = DatabaseUtils.StringIfNull(dataReader["Id"]);
                            double longitude = DatabaseUtils.DoubleIfNull(dataReader["longitude"]);
                            double latitude = DatabaseUtils.DoubleIfNull(dataReader["latitude"]);
                           


                            if (!String.IsNullOrEmpty(id) && longitude != 0 && latitude != 0)
                            {
                                
                                object[] record = new object[3];
                                record[0] = longitude;
                                record[1] = latitude;
                                

                                if (TraceSource != null)
                                {
                                    TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Running, "{0}: {1} {2} ", new object[] { id, longitude, latitude });
                                }

                                WebServiceKNearestResult result = (WebServiceKNearestResult)ProcessRecord(id, record);

                                if (!ShouldStop)
                                {
                                    UpdateRecord(id, result);

                                    if (TraceSource != null)
                                    {
                                        TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Running, " - {0}, {1}, {2}",
                                         new object[] { 
                                            "200", 
                                            "Success", 
                                            result.ToString()
                                            });
                                    }

                                    RecordsCompleted++;
                                    UpdateProcessingStatus(RecordsTotal, RecordsCompleted);
                                }
                                else
                                {
                                    SignalCancelled();
                                    ret = false;
                                    break;
                                }
                            }
                            else
                            {
                                RecordsCompleted++;
                                UpdateProcessingStatus(RecordsTotal, RecordsCompleted);
                            }
                        }
                        else
                        {
                            SignalCancelled();
                            ret = false;
                            break;
                        }
                    }

                    ret = true;

                }

                if (!ShouldStop)
                {
                    UpdateProcessingStatusFinished();
                }
                else
                {
                    SignalCancelled();
                    ret = false;
                }

            }
            catch (Exception e)
            {
                UpdateProcessingStatusAborted(e);

                if (dataReader != null)
                {
                    if (!dataReader.IsClosed)
                    {
                        dataReader.Close();
                    }
                }

                if (args.ShouldLeaveDatabaseConnectionOpen)
                {
                    try
                    {
                        if (DBManagerInputDataUpdate.Connection != null)
                        {
                            if (DBManagerInputDataUpdate.Connection.State != ConnectionState.Closed)
                            {
                                DBManagerInputDataUpdate.Connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception closing input database: " + ex.Message, ex);
                    }
                }

                throw new Exception("Error occured processing records: " + e.Message, e);
            }
            finally
            {
                if (dataReader != null)
                {
                    if (!dataReader.IsClosed)
                    {
                        dataReader.Close();
                    }
                }

                if (args.ShouldLeaveDatabaseConnectionOpen)
                {
                    try
                    {
                        if (DBManagerInputDataUpdate.Connection != null)
                        {
                            if (DBManagerInputDataUpdate.Connection.State != ConnectionState.Closed)
                            {
                                DBManagerInputDataUpdate.Connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception closing input database: " + ex.Message, ex);
                    }
                }
            }

            return ret;
        }

        public override bool UpdateRecord(object recordId, object result)
        {
            bool ret = false;

            WebServiceKNearestResult webServiceKNearestResult = null;
            try
            {
                webServiceKNearestResult = (WebServiceKNearestResult)result;
                BatchKNearest args = (BatchKNearest)BatchDatabaseOptions;

                string sql = "";

                // for SqlServer, first turn off warnings so fields are truncated if neccessary
                if (DBManagerInputDataUpdate.DatabaseType == DatabaseType.SqlServer)
                {
                    sql = " SET ANSI_WARNINGS OFF; ";
                }

                sql += " UPDATE " + DatabaseUtils.AsDbTableName(args.Table, true) + " SET ";

                if (webServiceKNearestResult.ResultDataTable != null && webServiceKNearestResult.ResultDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < webServiceKNearestResult.ResultDataTable.Rows.Count; i++)
                    {
                        string fieldNameNearestDistance = args.FieldNearestDistance1.Substring(1, args.FieldNearestDistance1.Length - 3) + (i + 1);
                        string fieldNameNearestId = args.FieldNearestId1.Substring(1, args.FieldNearestId1.Length - 3) + (i + 1);
                        string fieldNameNearestLatitude = args.FieldNearestLatitude1.Substring(1, args.FieldNearestLatitude1.Length - 3) + (i + 1);
                        string fieldNameNearestLongitude = args.FieldNearestLongitude1.Substring(1, args.FieldNearestLongitude1.Length - 3) + (i + 1);

                        sql += DatabaseUtils.AsDbColumnName(fieldNameNearestDistance) + " =@" + fieldNameNearestDistance + ", ";
                        sql += DatabaseUtils.AsDbColumnName(fieldNameNearestId) + " =@" + fieldNameNearestId + ", ";
                        sql += DatabaseUtils.AsDbColumnName(fieldNameNearestLatitude) + " =@" + fieldNameNearestLatitude + ", ";
                        sql += DatabaseUtils.AsDbColumnName(fieldNameNearestLongitude) + " =@" + fieldNameNearestLongitude + ", ";
                    }
                }


                sql += DatabaseUtils.AsDbColumnName(args.FieldVersion) + " =@FieldVersion, ";
                sql += DatabaseUtils.AsDbColumnName(args.FieldTimeTaken) + " =@FieldTimeTaken, ";
                sql += DatabaseUtils.AsDbColumnName(args.FieldTransactionId) + " =@FieldTransactionId, ";
                sql += DatabaseUtils.AsDbColumnName(args.FieldSource) + " =@FieldSource, ";
                sql += DatabaseUtils.AsDbColumnName(args.FieldErrorMessage) + " =@FieldErrorMessage, ";



                sql += DatabaseUtils.AsDbColumnName(args.FieldProcessed) + " =1 ";
                sql += " WHERE " + DatabaseUtils.AsDbColumnName(args.FieldId) + " =@ID ";

                SqlCommand cmd = new SqlCommand(sql);

                if (webServiceKNearestResult.ResultDataTable != null && webServiceKNearestResult.ResultDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < webServiceKNearestResult.ResultDataTable.Rows.Count; i++)
                    {
                        DataRow dataRow = webServiceKNearestResult.ResultDataTable.Rows[i];
                        string fieldNameNearestDistance = args.FieldNearestDistance1.Substring(1, args.FieldNearestDistance1.Length - 3) + (i + 1);
                        string fieldNameNearestId = args.FieldNearestId1.Substring(1, args.FieldNearestId1.Length - 3) + (i + 1);
                        string fieldNameNearestLatitude = args.FieldNearestLatitude1.Substring(1, args.FieldNearestLatitude1.Length - 3) + (i + 1);
                        string fieldNameNearestLongitude = args.FieldNearestLongitude1.Substring(1, args.FieldNearestLongitude1.Length - 3) + (i + 1);

                        cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter(fieldNameNearestDistance, SqlDbType.Float, dataRow["distance"]));
                        cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter(fieldNameNearestId, SqlDbType.VarChar, dataRow["id"]));
                        cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter(fieldNameNearestLatitude, SqlDbType.Float, dataRow["y"]));
                        cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter(fieldNameNearestLongitude, SqlDbType.Float, dataRow["x"]));
                    }
                }

                cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("FieldVersion", SqlDbType.VarChar, webServiceKNearestResult.Version.ToString()));
                cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("FieldTimeTaken", SqlDbType.Decimal, webServiceKNearestResult.TimeTaken));
                cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("FieldTransactionId", SqlDbType.VarChar, webServiceKNearestResult.TransactionId));
                cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("FieldSource", SqlDbType.VarChar, RunnerName));

                string errorMessage = webServiceKNearestResult.Error;
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    if (errorMessage.Length > 50)
                    {
                        errorMessage = errorMessage.Substring(0, 50);
                    }
                }

                cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("FieldErrorMessage", SqlDbType.VarChar, errorMessage));

                cmd.Parameters.Add(SqlParameterUtils.BuildSqlParameter("ID", SqlDbType.VarChar, recordId));

                QueryManager qm = DBManagerInputDataUpdateNew;
                qm.AddParameters(cmd.Parameters);


                try
                {
                    qm.ExecuteNonQuery(CommandType.Text, cmd.CommandText, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception updating census fields: " + ex.Message, ex);
                }

                ret = true;
            }
            catch (ThreadAbortException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                if (BatchDatabaseOptions.AbortOnError)
                {
                    throw new Exception("Error occured updating record: " + recordId + " : " + e.Message, e);
                }
                else
                {
                    ErrorCount++;
                }
            }

            return ret;
        }

        public override object ProcessRecord(object recordId, object record)
        {
            BatchKNearest args = (BatchKNearest)BatchDatabaseOptions;
            object ret = false;
            string added = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string transactionGuid = Guid.NewGuid().ToString();

            try
            {
                object[] recordFields = (object[])record;
                double lon = (double)recordFields[0];
                double lat = (double)recordFields[1];
                ret = KNearestFinder.GetKNearestRecords(lon, lat, args.K, args.FeatureTable);
            }
            catch (ThreadAbortException te)
            {
                throw te;
            }
            catch (Exception e)
            {

                if (BatchDatabaseOptions.AbortOnError)
                {
                    throw new Exception("Error occured processing record - abort on error: " + recordId + " : " + e.Message, e);
                }
                else
                {

                    if (PrevErrMsg == e.Message)
                    {
                        RepeatedErrorCount++;
                    }
                    else
                    {
                        RepeatedErrorCount = 0;
                    }

                    if (RepeatedErrorCount > BatchDatabaseOptions.MaxRepeatedErrorCountBeforeAbort)
                    {
                        throw new Exception("Error occured processing record - too many repeated errors: " + recordId.ToString() + " : " + e.Message, e);
                    }
                    else
                    {
                        ErrorCount++;

                        if (ErrorCount > BatchDatabaseOptions.MaxErrorCountBeforeAbort)
                        {
                            throw new Exception("Error occured processing record  - too many errors: " + recordId.ToString() + " : " + e.Message, e);
                        }
                    }

                    PrevErrMsg = e.Message;
                }
            }
            return ret;
        }
    }
}
