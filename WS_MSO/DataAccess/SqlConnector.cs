using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WS_MSO.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string conString= "DPSConnection";
        private static DataTable dt;
        private OleDbConnection dbCon;
        public SqlConnector()
        {
            dbCon = new OleDbConnection(GlobalConfig.CnnString(conString));
        }
        public DataTable GetData(string queryString, string tableName)
        {
            dt = new DataTable();
            try
            {
                using (var cmd = new OleDbCommand(QueryDB.GetProfile(), dbCon))
                {
                    dbCon.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = tableName;
                        return dt;
                    }
                }
            }
            catch (Exception)
            {

                return Helper.EmptyDataTable();
            }

        }

        public DataTable GetData(string queryString, string tableName, Dictionary<string, string> paramDic)
        {
            dt = new DataTable();
            try
            {
                using (var cmd = new OleDbCommand(QueryDB.GetProfile(), dbCon))
                {
                    dbCon.Open();

                    foreach(var param in paramDic)
                    {
                        if(param.Key == "WS_APP_ID")
                        {
                            int appId = int.Parse(param.Value);
                            cmd.Parameters.AddWithValue(param.Key, appId);
                        }
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = tableName;
                        return dt;
                    }
                }
            }
            catch (Exception)
            {

                return Helper.EmptyDataTable();
            }
        }

        public Tuple<int, string> InsertOrUpdateData(string queryString, Dictionary<string, dynamic> paramDic)
        {
            int result = -1;
            try
            {
                using(OleDbCommand cmd = new OleDbCommand(queryString, dbCon))
                {
                    dbCon.Open();

                    foreach(var param in paramDic)
                    {
                        //cmd.Parameters.AddWithValue(param.Key, param.Value);
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    result = cmd.ExecuteNonQuery();

                    string tmp = cmd.CommandText.ToString();
                    foreach (OleDbParameter p in cmd.Parameters)
                    {
                        tmp = tmp.Replace('@' + p.ParameterName.ToString(), "'" + p.Value.ToString() + "'");
                        //tmp = tmp.Replace(p.ParameterName.ToString(), "'" + p.Value.ToString() + "'");
                    }

                    return Tuple.Create(result, tmp);
                }
            }
            catch (Exception)
            {
                return Tuple.Create(result, "");
            }
        }
    }
}