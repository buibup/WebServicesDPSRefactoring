using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_MSO.DataAccess
{
    public static class QueryDPSDB
    {
        public static string GetProfile()
        {
            const string query = @"Select P.PROFILE_ID,P.DOCTOR_ID,P.EMP_ID,(Select T.TITLE_NAME From DPS_MT_TITLE T Where P.PROFILE_TITLE_THA_ID = T.TITLE_ID) As [TITLE_TH],P.PROFILE_FIRST_NAME_THA + ' ' + P.PROFILE_LAST_NAME_THA As [Thai Name],
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

            return query;
        }
        public static string GetJob()
        {
            const string query = @"Select J.WORK_ID,J.PROFILE_ID,
	                        (Select WP.WORK_POSITION_NAME From DPS_MT_WORK_POSITION WP Where WP.WORK_POSITION_ID = J.WORK_POSITION) As [WORK_POSITION],
	                        (Select WG.WORK_GROUP_NAME From DPS_MT_WORK_GROUP WG Where WG.WORK_GROUP_ID = J.WORK_GROUP) As [WORK_STATUS],
	                        J.WORK_START_DATE,
	                        J.WORK_APPROVE,J.WORK_EXPIRATION_DATE,J.WORK_PROBATION_END_DATE 
                        From DPS_JOB_WORK J
                        Where J.WORK_STATUS = 'A'";

            return query;
        }
        public static string GetAddress()
        {
            const string query = @"Select A.PROFILE_ID,CASE A.ADDRESS_TYPE WHEN 'P' THEN 'ID_CARD_ADDRESS' WHEN 'S' THEN 'CURRENT_ADDRESS' END As [ADDRESS] ,A.ADDRESS_NO,MSUB.SUBDISTRICT_NAME,MD.DISTRICT_NAME,MP.PROVINCE_NAME,A.ADDRESS_POST_CODE
                            From DPS_PERSONAL_ADDRESS A LEFT JOIN DPS_MT_SUBDISTRICT MSUB ON A.ADDRESS_SUB_DISTRICT = MSUB.SUBDISTRICT_CODE
	                            LEFT JOIN DPS_MT_DISTRICT MD ON A.ADDRESS_DISTRICT = MD.DISTRICT_CODE
	                            LEFT JOIN DPS_MT_PROVINCE MP ON A.ADDRESS_PROVINCE = MP.PROVINCE_CODE
                            WHERE 1=1";

            return query;
        }
        public static string GetBankInfo()
        {
            const string query = @"Select C.PROFILE_ID,C.PROVIDER_CODE,C.PROVIDER_HOSPITAL_CODE,C.PROVIDER_INDIVIDUAL,
	                            C.PROVIDER_BANK,C.PROVIDER_ACCOUNT_NO,C.PROVIDER_ACCOUNT_NAME,C.PROVIDER_ACCOUNT_BRANCH
                            From DPS_CARE_PROVIDER C
                            Where C.PROVIDER_STATUS = 'A'";

            return query;
        }
        public static string GetMedicalPlan()
        {
            const string query = @"Select F.PROFILE_ID,F.FAMILY_TYPE,F.FAMILY_FIRST_NAME_THA,F.FAMILY_FIRST_NAME_THA,F.FAMILY_FIRST_NAME_ENG,F.FAMILY_LAST_NAME_ENG,
	                        F.FAMILY_GENDER,F.FAMILY_CARD_ID,
	                        (Select MP.MEDICAL_PLAN_CODE From DPS_MT_MIDICAL_PLAN MP Where MP.MEDICAL_PLAN_ID = F.MEDICAL_PLAN_ID) As [MEDICAL_PLAN_CODE],
	                        (Select MP.MEDICAL_PLAN_DESC From DPS_MT_MIDICAL_PLAN MP Where MP.MEDICAL_PLAN_ID = F.MEDICAL_PLAN_ID) As [MEDICAL_PLAN_DESC]
                        From DPS_PERSONAL_FAMILY F
                        Where F.FAMILY_FLAG = 'A'";

            return query;
        }
        public static string GetPhone()
        {
            const string query = @"Select PH.PROFILE_ID,CASE PH.PHONE_TYPE WHEN 'M' THEN 'MOBILE' WHEN 'H' THEN 'HOME' WHEN 'F' THEN 'FAX' WHEN 'O' THEN 'OFFICE' WHEN 'D' THEN '6 DIGI CODE' END AS PHONE_TYPE,
                             PH.PHONE_NUMBER
                            From DPS_PERSONAL_PHONE PH
                            Where PH.PHONE_FLAG ='A'";

            return query;
        }
        public static string GetAppInformation()
        {
            const string query = "Select * From WS_APP_INFORMATION Where WS_APP_ID = ?";

            return query;
        }
        public static string InsertTransactionLog()
        {
            const string query = @"insert into WS_TRANSACTION_LOG(WS_APP_ID,WS_CREATE_DATE,WS_USER_NAME,WS_SERVICES_ID,WS_SQL) 
                                VALUES(?, GETDATE(), ?, ?, ?)";

            return query;
        }
        public static string InsertCert()
        {
            const string query = @"Insert Into DPS_TRAINING_CERTIFICATE
                                (DOCTOR_ID,EMP_ID,PROFILE_ID,CERT_TYPE_ID,CERT_TYPE,CERT_COUNTRY_ID,
                                CERT_COUNTRY,CERT_FROM,CERT_NAME,CERT_START_DATE,CERT_EXPIRED_DATE,
                                CERT_END_DATE,CERT_VERIFY_STATUS,CERT_STATUS,CERT_URL) 
                                VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            return query;
        }
        public static string InsertResuscitative()
        {
            const string query = @"Insert Into DPS_TRAINING_RESUSCITATIVE(DOCTOR_ID,PROFILE_ID,RESUSCITATIVE_SUBJECT,
                                RESUSCITATIVE_SUBJECTNAME,RESUSCITATIVE_END_DATE,RESUSCITATIVE_INSTITUE_NAME,
                                RESUSCITATIVE_EXPIRED_DATE,RESUSCITATIVE_STATUS,CREATE_USER_ID,CREATE_DATE,
                                UPD_USER_ID,UPD_DATE,RESUSCITATIVE_URL)
                                    VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), ?, GETDATE(), ?)";

            return query;
        }
        public static string GetCountry()
        {
            const string query = "Select COUNTRY_ID,COUNTRY_NAME From DPS_MT_COUNTRY";

            return query;
        }
        public static string GetTrainingOrientation()
        {
            const string query = "Select * From DPS_TRAINING_ORIENTATION O Where O.ORIENTATION_STATUS = 'Y' AND O.PROFILE_ID = ?";

            return query;
        }
        public static string InsertTraininOrientation()
        {
            const string query = @"Insert Into DPS_TRAINING_ORIENTATION(PROFILE_ID,ORIENTATION_DATE,ORIENTATION_RESULT,
                                ORIENTATION_STATUS,CREATE_USER_ID,CREATE_DATE,UPD_USER_ID,UPD_DATE,ORIENTATION_URL) 
                                VALUES(?, ?, ?,?, ?, GETDATE(), ?, GETDATE(), ?)";

            return query;
        }
        public static string UpdateTraininOrientation()
        {
            const string query = @"Update DPS_TRAINING_ORIENTATION Set ORIENTATION_RESULT = ?, UPD_USER_ID='MSO_USER', UPD_DATE=GETDATE() Where PROFILE_ID = ?";

            return query;
        }
        public static string InsertCME()
        {
			const string query = @"INSERT INTO [dbo].[DPS_TRAINING_CME]([PROFILE_ID],[CME_SUBJECT],[CME_DATE],
			                    [CME_TYPE],[CME_INSTITUTE_NAME],[CME_SCORE],[CME_EXPIRATION_DATE],[CME_FILE_PATH],
			                    [CME_FILE_TYPE],[CME_STATUS],[CREATE_USER_ID],[CREATE_DATE],[UPD_USER_ID],[UPD_DATE],[CME_URL])
			                     VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), ?, GETDATE(), ?)";

			return query;
        }
        public static string InsertMOC()
        {
            const string query = @"Insert Into DPS_TRAINING_MOC(DOCTOR_ID,PROFILE_ID,MOC_TYPE_ID,
                                MOC_TOPIC_NAME,MOC_COUNTRY_ID,MOC_COUNTRY,MOC_FROM,MOC_START_DATE,
                                MOC_END_DATE,MOC_VERIFY_STATUS,MOC_STATUS,MOC_SCORE,MOC_CREATE_USER_ID,
                                MOC_CREATE_DATE,MOC_UPDATE_USER_ID,MOC_UPDATE_DATE,MOC_URL)
                                VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), ?, GETDATE(), ?)";

            return query;
        }
    }
}
