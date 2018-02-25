using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WS_MSO.DataAccess;

namespace WS_MSO.Services
{
    public class DPSService : IDPSService
    {
        private IDataConnection dataConnection;
        public DPSService(SqlConnector sqlConnector)
        {
            dataConnection = sqlConnector;
        }

        public DataTable GetAddress()
        {
            return dataConnection.GetData(QueryDPSDB.GetAddress(), "DPSAddress");
        }

        public DataTable GetAddress(string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            return dataConnection.GetData(QueryDPSDB.GetAddress(), "DPSAddress");
        }

        public DataTable GetAppInformation(int appId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("WS_APP_ID", appId.ToString());
            return dataConnection.GetData(QueryDPSDB.GetAppInformation(), "DPSAppInformation", dic);
        }

        public DataTable GetBankInfo()
        {
            return dataConnection.GetData(QueryDPSDB.GetBankInfo(), "DPSBankInfo");
        }

        public DataTable GetBankInfo(string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            return dataConnection.GetData(QueryDPSDB.GetBankInfo(), "DPSBankInfo");
        }

        public DataTable GetCountry()
        {
            return dataConnection.GetData(QueryDPSDB.GetCountry(), "DPSCountry");
        }

        public DataTable GetCountry(string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            return dataConnection.GetData(QueryDPSDB.GetCountry(), "DPSCountry");

        }

        public DataTable GetJob()
        {
            return dataConnection.GetData(QueryDPSDB.GetJob(), "DPSJob");
        }

        public DataTable GetJob(string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            return dataConnection.GetData(QueryDPSDB.GetJob(), "DPSJob");
        }

        public DataTable GetMedicalPlan()
        {
            return dataConnection.GetData(QueryDPSDB.GetMedicalPlan(), "DPSMedicalPlan");
        }

        public DataTable GetMedicalPlan(string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            return dataConnection.GetData(QueryDPSDB.GetMedicalPlan(), "DPSMedicalPlan");
        }

        public DataTable GetPhone()
        {
            return dataConnection.GetData(QueryDPSDB.GetPhone(), "DPSPhone");
        }

        public DataTable GetPhone(string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            return dataConnection.GetData(QueryDPSDB.GetPhone(), "DPSPhone");
        }

        public DataTable GetProfile(int appId)
        {
            if (appId.IsCheckedAppId())
            {
                return dataConnection.GetData(QueryDPSDB.GetProfile(), "DPSDoctor");
            }
            else
            {
                return Helper.EmptyDataTable();
            }
        }

        public DataTable GetProfile(int appId, string programId)
        {
            if (!programId.IsMatchProgramId())
            {
                return Helper.EmptyDataTable();
            }

            if (appId.IsCheckedAppId())
            {
                return dataConnection.GetData(QueryDPSDB.GetProfile(), "DPSDoctor");
            }
            else
            {
                return Helper.EmptyDataTable();
            }
        }

        public DataTable GetTrainingOrientation()
        {
            return dataConnection.GetData(QueryDPSDB.GetTrainingOrientation(), "DPSTrainingOrientation");
        }

        public int InsertCert(int appId, string doctorId, string empId,
            string profileId, int certTypeId, int certCountryId,
            string certCountry, string certfrom, string certName,
            string certStartDate, string certExpireDate, string certEndDate,
            string certVerifyStatus, string certStatus, string certURL)
        {
            string certType = "";
            if (appId.IsCheckedAppId(5))
            {
                if (certTypeId == (int)CertType.External)
                {
                    certType = CertType.External.ToString();
                }
                else if (certTypeId == (int)CertType.Internal)
                {
                    certType = CertType.Internal.ToString();
                }
            }

            var dic = new Dictionary<string, dynamic>
            {
                { "@DOCTOR_ID", doctorId },
                { "@EMP_ID", empId },
                { "@PROFILE_ID", profileId },
                { "@CERT_TYPE_ID", certTypeId },
                { "@CERT_TYPE", certType },
                { "@CERT_COUNTRY_ID", certCountryId },
                { "@CERT_COUNTRY", certCountry },
                { "@CERT_FROM", certfrom },
                { "@CERT_NAME", certName },
                { "@CERT_START_DATE", certStartDate },
                { "@CERT_EXPIRED_DATE", certExpireDate },
                { "@CERT_END_DATE", certExpireDate },
                { "@CERT_VERIFY_STATUS", certVerifyStatus },
                { "@CERT_STATUS", certStatus },
                { "@CERT_URL", certURL }
            };

            var sqlCmd = QueryDPSDB.InsertCert();
            var tupResultData = dataConnection.InsertOrUpdateData(sqlCmd, dic);
            var tupCmd = Tuple.Create(sqlCmd, dic);

            InsertLog(5, tupResultData.Item2, 6);

            return tupResultData.Item1;
        }

        public int InsertCME(int appId, string doctorId, string profileId, string cmeSubject, string cmeDate, string cmeType, string cmeInstituteName, string cmeScore, string cmeExpirationDate, string cmeFilePath, string cmeFileType, string cmeStatus, string cmeURL)
        {
            int result = 0;

            if (appId.IsCheckedAppId(5))
            {
                var dic = new Dictionary<string, dynamic>()
                {
                    { "PROFILE_ID", profileId },
                    { "CME_SUBJECT", cmeSubject },
                    { "CME_DATE", cmeDate },
                    { "CME_TYPE", cmeType },
                    { "CME_INSTITUTE_NAME", cmeInstituteName },
                    { "CME_SCORE", cmeScore },
                    { "CME_EXPIRATION_DATE", cmeExpirationDate },
                    { "CME_FILE_PATH", cmeFilePath },
                    { "CME_FILE_TYPE", cmeFileType },
                    { "CME_STATUS", cmeStatus },
                    { "CREATE_USER_ID", "MSO_USER" },
                    { "CREATE_DATE", "GETDATE()" },
                    { "UPD_USER_ID", "MSO_USER" },
                    { "UPD_DATE", "GETDATE()" },
                    { "CME_URL", cmeURL }
                };
                var data = dataConnection.InsertOrUpdateData(QueryDPSDB.InsertCME(), dic);
                InsertLog(5, data.Item2, 11);
                result = data.Item1;
            }

            return result;
        }

        public void InsertLog(int appId, string sqlCmd, int serviceId)
        {
            var appInfo = GetAppInformation(appId);
            if (appInfo.Rows.Count > 0)
            {


                var dic = new Dictionary<string, dynamic>
                {
                    { "@WS_APP_ID", appInfo.Rows[0]["WS_APP_ID"].ToString() },
                    { "@WS_USER_NAME", appInfo.Rows[0]["WS_USER_NAME"].ToString() },
                    { "@WS_SERVICES_ID", serviceId.ToString() },
                    { "@WS_SQL", sqlCmd }
                };

                dataConnection.InsertOrUpdateData(QueryDPSDB.InsertTransactionLog(), dic);
            }
        }

        public int InsertMOC(int appId, string doctorId, string profileId, int mocTypeId, string mocTopicName, int mocCountryId, string mocCountry, string mocFrom, string mocStartDate, string mocEndDate, string mocVerifyStatus, string mocStatus, float mocScore, string mocURL)
        {
            int result = -1;
            if (appId.IsCheckedAppId(5))
            {
                var dic = new Dictionary<string, dynamic>()
                {
                    { "DOCTOR_ID", doctorId  },
                    { "PROFILE_ID", profileId  },
                    { "MOC_TYPE_ID", mocTypeId  },
                    { "MOC_TOPIC_NAME", mocTopicName  },
                    { "MOC_COUNTRY_ID", mocCountryId  },
                    { "MOC_COUNTRY", mocCountry  },
                    { "MOC_FROM", mocFrom  },
                    { "MOC_START_DATE", mocStartDate },
                    { "MOC_END_DATE", mocEndDate  },
                    { "MOC_VERIFY_STATUS", mocVerifyStatus  },
                    { "MOC_STATUS", mocStatus  },
                    { "MOC_SCORE", mocScore  },
                    { "MOC_CREATE_USER_ID", "MSO_USER"  },
                    { "MOC_CREATE_DATE", "GETDATE()"  },
                    { "MOC_UPDATE_USER_ID", "MSO_USER"  },
                    { "MOC_UPDATE_DATE", "GETDATE()"  },
                    { "MOC_URL", mocURL  }
                };
                var data = dataConnection.InsertOrUpdateData(QueryDPSDB.InsertMOC(), dic);
                InsertLog(5, data.Item2, 11);
                result = data.Item1;
            }

            return result;
        }

        public int InsertOrientation(int appId, string profileId, string orientationDate, string orientationResult, string orientationStatus)
        {
            int result = 0;
            if (appId.IsCheckedAppId(5))
            {
                // Insert
                var dt = GetTrainingOrientation();
                if (dt.Rows.Count == 0)
                {
                    var dic = new Dictionary<string, dynamic>()
                    {
                        { "@PROFILE_ID", profileId },
                        { "@ORIENTATION_DATE", orientationDate },
                        { "@ORIENTATION_RESULT", orientationResult },
                        { "@ORIENTATION_STATUS", orientationStatus },
                        { "@CREATE_USER_ID", "MSO_USER" },
                        { "@CREATE_DATE", "GETDATE()" },
                        { "@UPD_USER_ID", "MSO_USER" },
                        { "@UPD_DATE", "GETDATE()" },
                        { "@ORIENTATION_URL", "" }

                    };
                    var data = dataConnection.InsertOrUpdateData(QueryDPSDB.InsertTraininOrientation(), dic);
                    InsertLog(5, data.Item2, 10);
                    result = data.Item1;
                }
                else
                {
                    // Update
                    var dic = new Dictionary<string, dynamic>()
                    {
                        { "ORIENTATION_RESULT", orientationResult },
                        { "PROFILE_ID", profileId }
                    };
                    var data = dataConnection.InsertOrUpdateData(QueryDPSDB.UpdateTraininOrientation(), dic);
                    InsertLog(5, data.Item2, 10);
                    result = data.Item1;
                }
            }
            return result;
        }

        public int InsertResuscitative(int appId, string doctorId, string profileId, string resuscitativeSubject, string resuscitativeSubjectName, string resuscitativeEndDate, string resuscitativeInstitueName, string resuscitativeExpiredDate, string resuscitativeStatus, string resuscitativeURL)
        {
            int result = -1;

            if (appId.IsCheckedAppId(5))
            {
                var dic = new Dictionary<string, dynamic>()
                {
                    { "DOCTOR_ID", doctorId },
                    { "PROFILE_ID", profileId },
                    { "RESUSCITATIVE_SUBJECT", resuscitativeSubject },
                    { "RESUSCITATIVE_SUBJECTNAME", resuscitativeSubjectName },
                    { "RESUSCITATIVE_END_DATE", resuscitativeEndDate },
                    { "RESUSCITATIVE_INSTITUE_NAME", resuscitativeInstitueName },
                    { "RESUSCITATIVE_EXPIRED_DATE", resuscitativeExpiredDate },
                    { "RESUSCITATIVE_STATUS", resuscitativeStatus },
                    { "CREATE_USER_ID", "MSO_USER" },
                    { "CREATE_DATE", "GETDATE()" },
                    { "UPD_USER_ID", "MSO_USER" },
                    { "UPD_DATE", "GETDATE()" },
                    { "RESUSCITATIVE_URL", resuscitativeURL }
                };

                var data = dataConnection.InsertOrUpdateData(QueryDPSDB.InsertResuscitative(), dic);
                InsertLog(5, data.Item2, 9);
                result = data.Item1;
            }

            return result;
        }
    }
}