using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_MSO
{
    public static class QueryDB
    {
        public static string GetProfile()
        {
            const string query = @"Select P.PROFILE_ID,P.DOCTOR_ID,P.EMP_ID,(Select T.TITLE_NAME From DPS_MT_TITLE T Where P.PROFILE_TITLE_THA_ID = T.TITLE_ID) As [TITLE_TH],P.PROFILE_FIRST_NAME_THA + ' ' + P.PROFILE_LAST_NAME_THA As [Thai Name],
	                            (Select T.TITLE_NAME From DPS_MT_TITLE T Where P.PROFILE_TITLE_ENG_ID = T.TITLE_ID) As [TITLE_EN],
                                P.PROFILE_FIRST_NAME_ENG + ' ' + P.PROFILE_LAST_NAME_ENG As [English Name],
	                            (Select top (1) W.WORK_START_DATE From DPS_JOB_WORK W Where W.PROFILE_ID = P.PROFILE_ID order by W.WORK_ID desc) As [Start Date],
	                            (Select top (1) WP.WORK_POSITION_NAME From DPS_JOB_WORK W LEFT JOIN DPS_MT_WORK_POSITION WP ON W.WORK_POSITION = WP.WORK_POSITION_ID Where W.PROFILE_ID = P.PROFILE_ID order by W.WORK_ID desc) As [Work Position],
	                            (Select top (1) MG.WORK_GROUP_NAME From DPS_JOB_WORK W LEFT JOIN DPS_MT_WORK_GROUP MG on W.WORK_GROUP = MG.WORK_GROUP_ID Where W.PROFILE_ID = P.PROFILE_ID order by W.WORK_ID desc) As [Work Status],
	                            (Select H.HOSPITAL_SHORTNAME From DPS_HOSPITAL_INFO H Where P.HOSPITAL_ID = H.HOSPITAL_ID) As [Hospital Name],
	                            (Select D.DEPARTMENT_NAME From DPS_MT_DEPARTMENT D Where D.DEPARTMENT_ID = P.DEPARTMENT_ID) As [Department],
	                            P.PROFILE_BIRTH_DATE As [Date of Birth],
	                            P.PROFILE_GENDER As [Gender],
	                            (Select E.EMAIL_ADDRESS From DPS_PERSONAL_EMAIL E Where E.PROFILE_ID = P.PROFILE_ID And E.EMAIL_TYPE = 'P') As [Primary E-Mail],
	                            (Select E.EMAIL_ADDRESS From DPS_PERSONAL_EMAIL E Where E.PROFILE_ID = P.PROFILE_ID And E.EMAIL_TYPE = 'S') As [Secondary E-Mail],
	                            P.ACCOUNT_ID As [Username],
	                            P.PROFILE_MEDICAL_LICENSE As [Medical License ID],P.PCU_ID,P.PROFILE_PHOTO,P.PCG
                            From DPS_PERSONAL_PROFILE P
                            Where 1=1 And P.PROFILE_FLAG = 'A' AND P.HOSPITAL_ID in (1,10,22)";

            return query;
        }

        public static string GetSpecialty()
        {
            const string query = @"Select S.PROFILE_ID,S.SPECIALTY_NAME,S.SUBSPECIALTY_NAME,S.SPECIALTY_PRIMARY,
                                    S.SPECIALTY_STATUS From DPS_TRAINING_SPECIALTY S 
                                 Where S.SPECIALTY_STATUS = 'A' AND S.PROFILE_ID = ?";

            return query;
        }

        public static string GetMobil()
        {
            const string query = @"Select PH.PROFILE_ID,PH.PHONE_NUMBER 
                                 From DPS_PERSONAL_PHONE PH 
                                 Where PH.PROFILE_ID = ? AND PH.PHONE_TYPE = 'M' AND PH.PHONE_FLAG = 'A'";

            return query;
        }
    }
}