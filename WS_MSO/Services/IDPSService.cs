using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_MSO.Services
{
    public interface IDPSService
    {
        DataTable GetProfile(int appId);
        DataTable GetProfile(int appId, string programId);
        DataTable GetJob();
        DataTable GetJob(string programId);
        DataTable GetAddress();
        DataTable GetAddress(string programId);
        DataTable GetBankInfo();
        DataTable GetBankInfo(string programId);
        DataTable GetMedicalPlan();
        DataTable GetMedicalPlan(string programId);
        DataTable GetPhone();
        DataTable GetPhone(string programId);
        DataTable GetAppInformation(int appId);
        void InsertLog(int appId, string sqlCmd, int serviceId);
        int InsertCert(int appId, string doctorId, string empId, 
            string profileId, int certTypeId, int certCountryId,
            string certCountry, string certfrom, string certName, 
            string certStartDate, string certExpireDate, string certEndDate, 
            string certVerifyStatus, string certStatus, string certURL);
        int InsertResuscitative(int appId, string doctorId, string profileId,
            string resuscitativeSubject, string resuscitativeSubjectName, 
            string resuscitativeEndDate, string resuscitativeInstitueName,
            string resuscitativeExpiredDate, string resuscitativeStatus,
            string resuscitativeURL);
        DataTable GetCountry();
        DataTable GetCountry(string programId);
        DataTable GetTrainingOrientation();
        int InsertOrientation(int appId, string profileId, string orientationDate,
            string orientationResult, string orientationStatus);
        int InsertCME(int appId, string doctorId, string profileId, string cmeSubject,
            string cmeDate, string cmeType, string cmeInstituteName,
            string cmeScore, string cmeExpirationDate, string cmeFilePath,
            string cmeFileType, string cmeStatus, string cmeURL);
        int InsertMOC(int appId, string doctorId, string profileId,
            int mocTypeId, string mocTopicName, int mocCountryId,
            string mocCountry, string mocFrom, string mocStartDate,
            string mocEndDate, string mocVerifyStatus, string mocStatus,
            float mocScore, string mocURL);
    }
}
