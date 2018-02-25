using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// Summary description for AccessWs
/// </summary>
/// 

namespace DPSWebServices
{
    public class AccessWs
    {
        OleDbConnection Con = new OleDbConnection(ConfigurationManager.AppSettings["ConStr"]);
        OleDbConnection ConLog = new OleDbConnection(ConfigurationManager.AppSettings["ConStrLog"]);

        public DataSet GetDataSet(string pstrSql, string pTableName)
        {
            if (Con.State == ConnectionState.Closed)
            {
                Con.Open();
            }
            DataSet functionReturnValue = default(DataSet);
            DataSet dtsTmp = new DataSet();

            OleDbDataAdapter datTmp = new OleDbDataAdapter(pstrSql, Con);
            //int x = Con.ConnectionTimeout;

            dtsTmp.Clear();
            dtsTmp.EnforceConstraints = false;
            datTmp.Fill(dtsTmp, pTableName);
            functionReturnValue = dtsTmp;
            datTmp.Dispose();
            dtsTmp.Dispose();
            Con.Close();
            return functionReturnValue;
        }
        public int fn_ExecNonQuery(string pstrSql)
        {
            try
            {
                if (ConLog.State == ConnectionState.Closed)
                {
                    ConLog.Open();
                }
                OleDbCommand Cmd = new OleDbCommand(pstrSql, ConLog);
                int result = Cmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public DataTable GetDataTable(string pstrSql)
        {
            DataTable functionReturnValue = default(DataTable);
            functionReturnValue = GetDataTable(pstrSql, "Table0");
            return functionReturnValue;
        }
        private DataTable GetDataTable(string pstrSql, string pTableName)
        {
            if (Con.State == ConnectionState.Closed)
            {
                Con.Open();
            }
            DataTable functionReturnValue = default(DataTable);
            DataTable dtTmp = new DataTable();
            OleDbDataAdapter datTmp = new OleDbDataAdapter(pstrSql, Con);
            dtTmp.Clear();
            datTmp.Fill(dtTmp);
            dtTmp.TableName = pTableName;
            functionReturnValue = dtTmp;
            datTmp.Dispose();
            dtTmp.Dispose();
            Con.Close();
            return functionReturnValue;
        }


        public DataTable GetDataTableService(string pstrSql)
        {
            DataTable functionReturnValue = default(DataTable);
            functionReturnValue = GetDataTableService(pstrSql, "Table0");
            return functionReturnValue;
        }
        private DataTable GetDataTableService(string pstrSql, string pTableName)
        {
            if (ConLog.State == ConnectionState.Closed)
            {
                ConLog.Open();
            }
            DataTable functionReturnValue = default(DataTable);
            DataTable dtTmp = new DataTable();
            OleDbDataAdapter datTmp = new OleDbDataAdapter(pstrSql, ConLog);
            dtTmp.Clear();
            datTmp.Fill(dtTmp);
            dtTmp.TableName = pTableName;
            functionReturnValue = dtTmp;
            datTmp.Dispose();
            dtTmp.Dispose();
            ConLog.Close();
            return functionReturnValue;
        }
    }
}