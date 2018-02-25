using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using DPSWebServices;

namespace WS_MSO
{
    /// <summary>
    /// Summary description for DPSMSO
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DPSMSO : System.Web.Services.WebService
    {
        private OleDbConnection Conn = new OleDbConnection(GlobalConfig.CnnString("MSOConnection"));
        static DataTable dt;

        [WebMethod]
        public DataTable WS_MSOGetProfile(int APP_ID)
        {
            int? appId = APP_ID;
            dt = new DataTable();
            if ((appId != null && appId == 5) || (appId != null && appId == 6))
            {

                using (var cmd = new OleDbCommand(QueryDB.GetProfile(), Conn))
                {
                    Conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = "DPSDoctor";
                        insertLOG(APP_ID, "", 5);
                        return dt;
                    }
                }
            }
            else
            {
                return EmptyDataTable();
            }
        }
        [WebMethod]
        public DataTable WS_MSOGetProfileWithProgramId(int APP_ID, string programId)
        {
            int? appId = APP_ID;
            dt = new DataTable();

            if (!IsMatchProgramId(programId))
            {
                return EmptyDataTable();
            };

            if ((appId != null && appId == 5) || (appId != null && appId == 6))
            {

                using (var cmd = new OleDbCommand(QueryDB.GetProfile(), Conn))
                {
                    Conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = "DPSDoctor";
                        insertLOG(APP_ID, "", 5);
                        return dt;
                    }
                }
            }
            else
            {
                return dt;
            }
        }

        [WebMethod]
        public DataTable WS_MSOGetSpecialty(int APP_ID, int PID)
        {
            int? appId = APP_ID;
            dt = new DataTable();
            if ((appId != null && appId == 5) || (appId != null && appId == 6))
            {
                using (OleDbCommand cmd = new OleDbCommand(QueryDB.GetSpecialty(), Conn))
                {
                    Conn.Open();

                    cmd.Parameters.AddWithValue("PROFILE_ID", PID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = "DPSSpecialty";
                        insertLOG(APP_ID, "", 7);
                    }
                }
            }
            return dt;
        }
        [WebMethod]
        public DataTable WS_MSOGetSpecialtyWithProgramId(int APP_ID, int PID, string programId)
        {
            int? appId = APP_ID;
            if (!IsMatchProgramId(programId))
            {
                return EmptyDataTable();
            }
            dt = new DataTable();
            if ((appId != null && appId == 5) || (appId != null && appId == 6))
            {
                using (OleDbCommand cmd = new OleDbCommand(QueryDB.GetSpecialty(), Conn))
                {
                    Conn.Open();

                    cmd.Parameters.AddWithValue("PROFILE_ID", PID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = "DPSSpecialty";
                        insertLOG(APP_ID, "", 7);
                    }
                }
            }
            return dt;
        }


        [WebMethod]
        public DataTable WS_MSOGetMobil(int APP_ID, int PID)
        {
            int? appId = APP_ID;
            dt = new DataTable();
            if ((appId != null && appId == 5) || (appId != null && appId == 6))
            {
                using (OleDbCommand cmd = new OleDbCommand(QueryDB.GetMobil(), Conn))
                {
                    Conn.Open();

                    cmd.Parameters.AddWithValue("PROFILE_ID", PID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = "DPSMobile";
                        insertLOG(APP_ID, "", 8);
                    }
                }
            }
            return dt;
        }
        [WebMethod]
        public DataTable WS_MSOGetMobilWithProgramId(int APP_ID, int PID, string programId)
        {
            int? appId = APP_ID;
            dt = new DataTable();
            if (!IsMatchProgramId(programId))
            {
                return EmptyDataTable();
            }

            if ((appId != null && appId == 5) || (appId != null && appId == 6))
            {
                using (OleDbCommand cmd = new OleDbCommand(QueryDB.GetMobil(), Conn))
                {
                    Conn.Open();

                    cmd.Parameters.AddWithValue("PROFILE_ID", PID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        dt.TableName = "DPSMobile";
                        insertLOG(APP_ID, "", 8);
                    }
                }
            }
            return dt;
        }

        public void insertLOG(int? WS_APP_ID, string sql, int WS_SERVICES_ID)
        {
            AccessWs ws = new AccessWs();
            DataTable dt = new DataTable();
            string str = "Select * From WS_APP_INFORMATION Where WS_APP_ID = " + WS_APP_ID;
            dt = ws.GetDataTableService(str);
            if (dt.Rows.Count > 0)
            {
                string ins = "insert into WS_TRANSACTION_LOG(WS_APP_ID,WS_CREATE_DATE,WS_USER_NAME,WS_SERVICES_ID,WS_SQL) VALUES(" + dt.Rows[0]["WS_APP_ID"].ToString() + ",GETDATE(),'" + dt.Rows[0]["WS_APP_NAME"].ToString() + "'," + WS_SERVICES_ID + ",'" + sql + "')";
                ws.fn_ExecNonQuery(ins);
            }

        }

        private bool IsMatchProgramId(string programId)
        {
            bool match = false;

            var _programId = ConfigurationManager.AppSettings["ProgramId"];
            var programIdArr = _programId.Split('|');

            foreach (var proId in programIdArr)
            {
                if (proId == programId)
                {
                    return true;
                }
            }

            return match;
        }

        private DataTable EmptyDataTable()
        {
            dt.Clear();
            dt.TableName = "empty";
            return dt;
        }
    }
}
