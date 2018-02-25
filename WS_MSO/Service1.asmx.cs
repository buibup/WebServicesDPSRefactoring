using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using DPSWebServices;

namespace WS_MSO
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        OleDbConnection Con = new OleDbConnection(ConfigurationManager.AppSettings["ConStr"]);
        //static DataTable dt = new DataTable();
        static DataTable dt;
        static int InsRecord = 0;
        AccessWs ad = new AccessWs();
        [WebMethod]
        public DataTable WS_GetProfile(int APP_ID)
        {
            dt = new DataTable();
            if ((APP_ID != null && APP_ID == 5) || (APP_ID != null && APP_ID == 6))
            {
                OleDbCommand oCmd = new OleDbCommand();
                DataSet ds = new DataSet();

                oCmd.Connection = Con;
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = @"Select P.PROFILE_ID,P.DOCTOR_ID,P.EMP_ID,(Select T.TITLE_NAME From DPS_MT_TITLE T Where P.PROFILE_TITLE_THA_ID = T.TITLE_ID) As [TITLE_TH],P.PROFILE_FIRST_NAME_THA + ' ' + P.PROFILE_LAST_NAME_THA As [Thai Name],
	                            (Select T.TITLE_NAME From DPS_MT_TITLE T Where P.PROFILE_TITLE_ENG_ID = T.TITLE_ID) As [TITLE_EN],
                                P.PROFILE_FIRST_NAME_ENG + ' ' + P.PROFILE_LAST_NAME_ENG As [English Name],
                                CASE P.MARITAL WHEN 1 THEN 'Single' WHEN 2 THEN 'Married' WHEN 3 THEN 'Divorced' WHEN 4 THEN 'Widowed' END As MARITAL,
                                CASE P.RACE WHEN 1 THEN 'Thai' WHEN 2 THEN 'Other' End As RACE,
                                P.RACEOTHER,
                                CASE P.NATIONALITY WHEN 1 THEN 'Thai' WHEN 2 THEN 'Other' End As NATIONALITY,
                                P.NATIONALITYOTHER,
                                (Select MR.RELIGION_NAME From DPS_MT_RELIGION MR Where MR.RELIGION_ID = P.RELIGION_ID) As REGLIGION_NAME,
								P.PROFILE_CARD_ID,
								P.PROFILE_CARD_EXPIRATION,
	                            S.SPECIALTY_NAME,S.SUBSPECIALTY_NAME,
	                            (Select top (1) W.WORK_START_DATE From DPS_JOB_WORK W Where W.PROFILE_ID = P.PROFILE_ID order by W.WORK_ID desc) As [Start Date],
	                            (Select top (1) WP.WORK_POSITION_NAME From DPS_JOB_WORK W LEFT JOIN DPS_MT_WORK_POSITION WP ON W.WORK_POSITION = WP.WORK_POSITION_ID Where W.PROFILE_ID = P.PROFILE_ID order by W.WORK_ID desc) As [Work Position],
	                            (Select top (1) MG.WORK_GROUP_NAME From DPS_JOB_WORK W LEFT JOIN DPS_MT_WORK_GROUP MG on W.WORK_GROUP = MG.WORK_GROUP_ID Where W.PROFILE_ID = P.PROFILE_ID order by W.WORK_ID desc) As [Work Status],
	                            (Select H.HOSPITAL_SHORTNAME From DPS_HOSPITAL_INFO H Where P.HOSPITAL_ID = H.HOSPITAL_ID) As [Hospital Name],
	                            (Select D.DEPARTMENT_NAME From DPS_MT_DEPARTMENT D Where D.DEPARTMENT_ID = P.DEPARTMENT_ID) As [Department],
	                            P.PROFILE_BIRTH_DATE As [Date of Birth],
	                            P.PROFILE_GENDER As [Gender],
	                            (Select E.EMAIL_ADDRESS From DPS_PERSONAL_EMAIL E Where E.PROFILE_ID = P.PROFILE_ID And E.EMAIL_TYPE = 'P') As [Primary E-Mail],
	                            (Select E.EMAIL_ADDRESS From DPS_PERSONAL_EMAIL E Where E.PROFILE_ID = P.PROFILE_ID And E.EMAIL_TYPE = 'S') As [Secondary E-Mail],
	                            --PH.PHONE_NUMBER As [Mobile Number],
	                            P.ACCOUNT_ID As [Username],
	                            P.PROFILE_MEDICAL_LICENSE As [Medical License ID],P.PCU_ID,P.PROFILE_PHOTO,P.PCG,P.PROFILE_FLAG
                            From DPS_PERSONAL_PROFILE P LEFT JOIN DPS_TRAINING_SPECIALTY S on P.PROFILE_ID = S.PROFILE_ID
	                            --LEFT JOIN DPS_PERSONAL_PHONE PH on P.PROFILE_ID = PH.PROFILE_ID
                            Where 1=1 
                                AND P.HOSPITAL_ID in (1,10,22)";
                OleDbDataAdapter da = new OleDbDataAdapter(oCmd);
                da.Fill(ds, "DPSDoctor");
                dt = ds.Tables["DPSDoctor"];
                insertLOG(APP_ID,"",5);
                return dt;
            }
            else
            {
                return dt;
            }
        }
        [WebMethod]
        public DataTable WS_GetJob()
        {
            dt = new DataTable();
            string sql = @"Select J.WORK_ID,J.PROFILE_ID,
	                        (Select WP.WORK_POSITION_NAME From DPS_MT_WORK_POSITION WP Where WP.WORK_POSITION_ID = J.WORK_POSITION) As [WORK_POSITION],
	                        (Select WG.WORK_GROUP_NAME From DPS_MT_WORK_GROUP WG Where WG.WORK_GROUP_ID = J.WORK_GROUP) As [WORK_STATUS],
	                        J.WORK_START_DATE,
	                        J.WORK_APPROVE,J.WORK_EXPIRATION_DATE,J.WORK_PROBATION_END_DATE 
                        From DPS_JOB_WORK J
                        Where J.WORK_STATUS = 'A'";
            dt = ad.GetDataTable(sql);
            return dt;
        }
        [WebMethod]
        public DataTable WS_GetAddress()
        {
            dt = new DataTable();
            string sql = @"Select A.PROFILE_ID,CASE A.ADDRESS_TYPE WHEN 'P' THEN 'ID_CARD_ADDRESS' WHEN 'S' THEN 'CURRENT_ADDRESS' END As [ADDRESS] ,A.ADDRESS_NO,MSUB.SUBDISTRICT_NAME,MD.DISTRICT_NAME,MP.PROVINCE_NAME,A.ADDRESS_POST_CODE
                            From DPS_PERSONAL_ADDRESS A LEFT JOIN DPS_MT_SUBDISTRICT MSUB ON A.ADDRESS_SUB_DISTRICT = MSUB.SUBDISTRICT_CODE
	                            LEFT JOIN DPS_MT_DISTRICT MD ON A.ADDRESS_DISTRICT = MD.DISTRICT_CODE
	                            LEFT JOIN DPS_MT_PROVINCE MP ON A.ADDRESS_PROVINCE = MP.PROVINCE_CODE
                            WHERE 1=1";
            dt = ad.GetDataTable(sql);
            return dt;
        }
        [WebMethod]
        public DataTable WS_GetBankInfo()
        {
            dt = new DataTable();
            string sql = @"Select C.PROFILE_ID,C.PROVIDER_CODE,C.PROVIDER_HOSPITAL_CODE,C.PROVIDER_INDIVIDUAL,
	                            C.PROVIDER_BANK,C.PROVIDER_ACCOUNT_NO,C.PROVIDER_ACCOUNT_NAME,C.PROVIDER_ACCOUNT_BRANCH
                            From DPS_CARE_PROVIDER C
                            Where C.PROVIDER_STATUS = 'A'";
            dt = ad.GetDataTable(sql);
            return dt;
        }
        [WebMethod]
        public DataTable WS_GetMedical_Plan()
        {
            dt = new DataTable();
            string sql = @"Select F.PROFILE_ID,F.FAMILY_TYPE,F.FAMILY_FIRST_NAME_THA,F.FAMILY_FIRST_NAME_THA,F.FAMILY_FIRST_NAME_ENG,F.FAMILY_LAST_NAME_ENG,
	                        F.FAMILY_GENDER,F.FAMILY_CARD_ID,
	                        (Select MP.MEDICAL_PLAN_CODE From DPS_MT_MIDICAL_PLAN MP Where MP.MEDICAL_PLAN_ID = F.MEDICAL_PLAN_ID) As [MEDICAL_PLAN_CODE],
	                        (Select MP.MEDICAL_PLAN_DESC From DPS_MT_MIDICAL_PLAN MP Where MP.MEDICAL_PLAN_ID = F.MEDICAL_PLAN_ID) As [MEDICAL_PLAN_DESC]
                        From DPS_PERSONAL_FAMILY F
                        Where F.FAMILY_FLAG = 'A'";
            dt = ad.GetDataTable(sql);
            return dt;
        }
        [WebMethod]
        public DataTable WS_GetPhone()
        {
            dt = new DataTable();
            string sql = @"Select PH.PROFILE_ID,CASE PH.PHONE_TYPE WHEN 'M' THEN 'MOBILE' WHEN 'H' THEN 'HOME' WHEN 'F' THEN 'FAX' WHEN 'O' THEN 'OFFICE' WHEN 'D' THEN '6 DIGI CODE' END AS PHONE_TYPE,
                             PH.PHONE_NUMBER
                            From DPS_PERSONAL_PHONE PH
                            Where PH.PHONE_FLAG ='A'";
            dt = ad.GetDataTable(sql);
            return dt;
        }
        public void insertLOG(int WS_APP_ID,string sql,int WS_SERVICES_ID)
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
        [WebMethod]
        public int WS_InsertCert(int APP_ID,string DOCTOR_ID,string EMP_ID,string PROFILE_ID,int CERT_TYPE_ID,int CERT_COUNTRY_ID,string CERT_COUNTRY,string CERT_FROM,string CERT_NAME,string CERT_START_DATE,string CERT_EXPIRED_DATE,string CERT_END_DATE,string CERT_VERIFY_STATUS,string CERT_STATUS, string CERT_URL)
        {
            if (APP_ID != null && APP_ID == 5)
            {
                string CERT_TYPE = "";
                if (CERT_TYPE_ID == 1)
                {
                    CERT_TYPE = "External";
                }
                else if (CERT_TYPE_ID == 2)
                {
                    CERT_TYPE = "Internal";
                }
                else
                {
                    CERT_TYPE = "";
                }
                string strIns = @"Insert Into DPS_TRAINING_CERTIFICATE(DOCTOR_ID,EMP_ID,PROFILE_ID,CERT_TYPE_ID,CERT_TYPE,CERT_COUNTRY_ID,CERT_COUNTRY,CERT_FROM,CERT_NAME,CERT_START_DATE,CERT_EXPIRED_DATE,CERT_END_DATE,CERT_VERIFY_STATUS,CERT_STATUS,CERT_URL) 
                            VALUES('" + DOCTOR_ID + "','" + EMP_ID + "'," + PROFILE_ID + "," + CERT_TYPE_ID + ",'" + CERT_TYPE + "'," + CERT_COUNTRY_ID + ",'" + CERT_COUNTRY + "','" + CERT_FROM + "','" + CERT_NAME + "','" + CERT_START_DATE + "','" + CERT_EXPIRED_DATE + "','" + CERT_END_DATE + "','" + CERT_VERIFY_STATUS + "','" + CERT_STATUS + "','" + CERT_URL + "')";
                InsRecord = fn_ExecNonQuery(strIns);
                strIns = strIns.Replace("'", "");
                insertLOG(5, strIns, 6);
                return InsRecord;
            }
            else
            {
                return InsRecord;
            }
        }
        [WebMethod]
        public int WS_InsertResuscitative(int APP_ID, string DOCTOR_ID, string PROFILE_ID, string RESUSCITATIVE_SUBJECT, string RESUSCITATIVE_SUBJECTNAME, string RESUSCITATIVE_END_DATE, string RESUSCITATIVE_INSTITUE_NAME, string RESUSCITATIVE_EXPIRED_DATE, string RESUSCITATIVE_STATUS, string RESUSCITATIVE_URL)
        {
            if (APP_ID != null && APP_ID == 5)
            {
                string strInsRe = @"Insert Into DPS_TRAINING_RESUSCITATIVE(DOCTOR_ID,PROFILE_ID,RESUSCITATIVE_SUBJECT,RESUSCITATIVE_SUBJECTNAME,RESUSCITATIVE_END_DATE,RESUSCITATIVE_INSTITUE_NAME,RESUSCITATIVE_EXPIRED_DATE,RESUSCITATIVE_STATUS,CREATE_USER_ID,CREATE_DATE,UPD_USER_ID,UPD_DATE,RESUSCITATIVE_URL)
                                    VALUES('" + DOCTOR_ID + "'," + PROFILE_ID + ",'" + RESUSCITATIVE_SUBJECT + "','" + RESUSCITATIVE_SUBJECTNAME + "','" + RESUSCITATIVE_END_DATE + "','" + RESUSCITATIVE_INSTITUE_NAME + "','" + RESUSCITATIVE_EXPIRED_DATE + "','" + RESUSCITATIVE_STATUS + "','" + "MSO_USER" + "'," + "GETDATE()" + ",'" + "MSO_USER" + "'," + "GETDATE(),'" + RESUSCITATIVE_URL + "')";
                InsRecord = fn_ExecNonQuery(strInsRe);
                strInsRe = strInsRe.Replace("'", "");
                insertLOG(5, strInsRe, 9);
                return InsRecord;
            }
            return InsRecord;
        }
        [WebMethod]
        public DataTable WS_Country()
        {
            string strCountry = "Select COUNTRY_ID,COUNTRY_NAME From DPS_MT_COUNTRY";
            AccessWs ws = new AccessWs();
            dt = ws.GetDataTable(strCountry);
            return dt;
        }
        [WebMethod]
        public int WS_InsertOrientation(int APP_ID, string PROFILE_ID, string ORIENTATION_DATE, string ORIENTATION_RESULT, string ORIENTATION_STATUS)
        {
            if (APP_ID != null && APP_ID == 5)
            {
                string ckhOr = "Select * From DPS_TRAINING_ORIENTATION O Where O.ORIENTATION_STATUS = 'Y' AND O.PROFILE_ID = '" + PROFILE_ID + "'";
                DataTable dt = ad.GetDataTable(ckhOr);
                if (dt.Rows.Count == 0)
                {
                    // Insert
                    string strInsOr = @"Insert Into DPS_TRAINING_ORIENTATION(PROFILE_ID,ORIENTATION_DATE,ORIENTATION_RESULT,ORIENTATION_STATUS,CREATE_USER_ID,CREATE_DATE,UPD_USER_ID,UPD_DATE,ORIENTATION_URL) 
                    VALUES('" + PROFILE_ID + "','" + ORIENTATION_DATE + "','" + ORIENTATION_RESULT + "','" + ORIENTATION_STATUS + "','" + "MSO_USER" + "'," + "GETDATE()" + ",'" + "MSO_USER" + "'," + "GETDATE())";
                    InsRecord = fn_ExecNonQuery(strInsOr);
                    strInsOr = strInsOr.Replace("'", "");
                    insertLOG(5, strInsOr, 10);
                }
                else
                { 
                    // Update
                    string strUpdOr = @"Update DPS_TRAINING_ORIENTATION Set ORIENTATION_RESULT = '" + ORIENTATION_RESULT + "',UPD_USER_ID='MSO_USER',UPD_DATE=GETDATE() Where PROFILE_ID = '" + PROFILE_ID + "'";
                    InsRecord = fn_ExecNonQuery(strUpdOr);
                    strUpdOr = strUpdOr.Replace("'", "");
                    insertLOG(5, strUpdOr, 10);
                }
                return InsRecord;
            }
            return InsRecord;
        }
        [WebMethod]
        public int WS_InsertCME(int APP_ID, string DOCTOR_ID, string PROFILE_ID, string CME_SUBJECT, string CME_DATE, string CME_TYPE, string CME_INSTITUTE_NAME, string CME_SCORE, string CME_EXPIRATION_DATE, string CME_FILE_PATH, string CME_FILE_TYPE, string CME_STATUS, string CME_URL)
        {
            if (APP_ID != null && APP_ID == 5)
            {
                string strInsCME = @"INSERT INTO [dbo].[DPS_TRAINING_CME]([PROFILE_ID],[CME_SUBJECT],[CME_DATE],[CME_TYPE],[CME_INSTITUTE_NAME],[CME_SCORE],[CME_EXPIRATION_DATE],[CME_FILE_PATH],[CME_FILE_TYPE],[CME_STATUS],[CREATE_USER_ID],[CREATE_DATE],[UPD_USER_ID],[UPD_DATE],[CME_URL])
                                VALUES('" + PROFILE_ID + "','" + CME_SUBJECT + "','" + CME_DATE + "','" + CME_TYPE + "','" + CME_INSTITUTE_NAME + "'," + CME_SCORE + ",'" + CME_EXPIRATION_DATE + "','" + CME_FILE_PATH + "','" + CME_FILE_TYPE + "','" + CME_STATUS + "','" + "MSO_USER" + "'," + "GETDATE()" + ",'" + "MSO_USER" + "'," + "GETDATE(),'" + CME_URL + "')";
                InsRecord = fn_ExecNonQuery(strInsCME);
                strInsCME = strInsCME.Replace("'", "");
                insertLOG(5, strInsCME, 11);
                return InsRecord;
            }
            return InsRecord;
        }
        [WebMethod]
        public int WS_InsertMOC(int APP_ID, string DOCTOR_ID, string PROFILE_ID, int MOC_TYPE_ID, string MOC_TOPIC_NAME, int MOC_COUNTRY_ID, string MOC_COUNTRY, string MOC_FROM, string MOC_START_DATE, string MOC_END_DATE, string MOC_VERIFY_STATUS, string MOC_STATUS, float MOC_SCORE, string MOC_URL)
        {
            if (APP_ID != null && APP_ID == 5)
            {
                string strInsMOC = @"Insert Into DPS_TRAINING_MOC(DOCTOR_ID,PROFILE_ID,MOC_TYPE_ID,MOC_TOPIC_NAME,MOC_COUNTRY_ID,MOC_COUNTRY,MOC_FROM,MOC_START_DATE,MOC_END_DATE,MOC_VERIFY_STATUS,MOC_STATUS,MOC_SCORE,MOC_CREATE_USER_ID,MOC_CREATE_DATE,MOC_UPDATE_USER_ID,MOC_UPDATE_DATE,MOC_URL)
                                VALUES('" + DOCTOR_ID + "','" + PROFILE_ID + "'," + MOC_TYPE_ID + ",'" + MOC_TOPIC_NAME + "'," + MOC_COUNTRY_ID + ",'" + MOC_COUNTRY + "','" + MOC_FROM + "','" + MOC_START_DATE + "','" + MOC_END_DATE + "','" + MOC_VERIFY_STATUS + "','" + MOC_STATUS + "'," + MOC_SCORE + ",'" + "MSO_USER" + "'," + "GETDATE()" + ",'" + "MSO_USER" + "'," + "GETDATE(),'" + MOC_URL + "')";
                InsRecord = fn_ExecNonQuery(strInsMOC);
                strInsMOC = strInsMOC.Replace("'", "");
                insertLOG(5, strInsMOC, 11);
                return InsRecord;
            }
            return InsRecord;
        }        
        public int fn_ExecNonQuery(string pstrSql)
        {
            try
            {
                if (Con.State == ConnectionState.Closed)
                {
                    Con.Open();
                }
                OleDbCommand Cmd = new OleDbCommand(pstrSql, Con);
                int result = Cmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}