using System.Data;
using System.Web.Services;
using WS_MSO.Services;

namespace WS_MSO
{
	/// <summary>
	/// Summary description for Service_DPS
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class Service_DPS : System.Web.Services.WebService
	{
		private IDPSService _dPSService;
		public Service_DPS()
		{
			_dPSService = new DPSService();
		}

		[WebMethod]
		public DataTable WS_GetProfile(int appId, string programId)
		{
			return _dPSService.GetProfile(appId, programId);
		}

		[WebMethod]
		public DataTable WS_GetJob(string programId)
		{
			return _dPSService.GetJob(programId);
		}

		[WebMethod]
		public DataTable WS_GetAddress(string programId)
		{
			return _dPSService.GetAddress(programId);
		}

		[WebMethod]
		public DataTable WS_GetBankInfo(string programId)
		{
			return _dPSService.GetBankInfo(programId);
		}

		[WebMethod]
		public DataTable WS_GetMedical_Plan(string programId)
		{
			return _dPSService.GetMedicalPlan(programId);
		}

		[WebMethod]
		public DataTable WS_GetPhone(string programId)
		{
			return _dPSService.GetPhone(programId);
		}

		[WebMethod]
		public int WS_InsertCert(int APP_ID, string DOCTOR_ID, string EMP_ID, int PROFILE_ID, int CERT_TYPE_ID, int CERT_COUNTRY_ID, string CERT_COUNTRY, string CERT_FROM, string CERT_NAME, string CERT_START_DATE, string CERT_EXPIRED_DATE, string CERT_END_DATE, string CERT_VERIFY_STATUS, string CERT_STATUS, string CERT_URL)
		{
			return _dPSService.InsertCert(APP_ID, DOCTOR_ID, EMP_ID,
				PROFILE_ID, CERT_TYPE_ID, CERT_COUNTRY_ID, CERT_COUNTRY,
				CERT_FROM, CERT_NAME, CERT_START_DATE, CERT_EXPIRED_DATE,
				CERT_END_DATE, CERT_VERIFY_STATUS, CERT_STATUS, CERT_URL);
		}

		[WebMethod]
		public int WS_InsertResuscitative(int APP_ID, string DOCTOR_ID, int PROFILE_ID, string RESUSCITATIVE_SUBJECT, string RESUSCITATIVE_SUBJECTNAME, string RESUSCITATIVE_END_DATE, string RESUSCITATIVE_INSTITUE_NAME, string RESUSCITATIVE_EXPIRED_DATE, string RESUSCITATIVE_STATUS, string RESUSCITATIVE_URL)
		{
			return _dPSService.InsertResuscitative(APP_ID, DOCTOR_ID, PROFILE_ID,
				RESUSCITATIVE_SUBJECT, RESUSCITATIVE_SUBJECTNAME, RESUSCITATIVE_END_DATE,
				RESUSCITATIVE_INSTITUE_NAME, RESUSCITATIVE_EXPIRED_DATE, RESUSCITATIVE_STATUS,
				RESUSCITATIVE_URL);
		}

		[WebMethod]
		public DataTable WS_Country(string programId)
		{
			return _dPSService.GetCountry(programId);
		}

		[WebMethod]
		public int WS_InsertOrientation(int APP_ID, int PROFILE_ID, string ORIENTATION_DATE, string ORIENTATION_RESULT, string ORIENTATION_STATUS)
		{
			return _dPSService.InsertOrientation(APP_ID, PROFILE_ID, ORIENTATION_DATE,
				ORIENTATION_RESULT, ORIENTATION_STATUS);
		}

		[WebMethod]
		public int WS_InsertCME(int APP_ID, string DOCTOR_ID, string PROFILE_ID, string CME_SUBJECT, string CME_DATE, string CME_TYPE, string CME_INSTITUTE_NAME, string CME_SCORE, string CME_EXPIRATION_DATE, string CME_FILE_PATH, string CME_FILE_TYPE, string CME_STATUS, string CME_URL)
		{
			return _dPSService.InsertCME(APP_ID, DOCTOR_ID, PROFILE_ID,
				CME_SUBJECT, CME_DATE, CME_TYPE, CME_INSTITUTE_NAME,
				CME_SCORE, CME_EXPIRATION_DATE, CME_FILE_PATH,
				CME_FILE_TYPE, CME_STATUS, CME_URL);
		}

		[WebMethod]
		public int WS_InsertMOC(int APP_ID, string DOCTOR_ID, int PROFILE_ID, int MOC_TYPE_ID, string MOC_TOPIC_NAME, int MOC_COUNTRY_ID, string MOC_COUNTRY, string MOC_FROM, string MOC_START_DATE, string MOC_END_DATE, string MOC_VERIFY_STATUS, string MOC_STATUS, float MOC_SCORE, string MOC_URL)
		{
			return _dPSService.InsertMOC(APP_ID, DOCTOR_ID, PROFILE_ID,
				MOC_TYPE_ID, MOC_TOPIC_NAME, MOC_COUNTRY_ID, MOC_COUNTRY,
				MOC_FROM, MOC_START_DATE, MOC_END_DATE, MOC_VERIFY_STATUS,
				MOC_STATUS, MOC_SCORE, MOC_URL);
		}
	}
}
