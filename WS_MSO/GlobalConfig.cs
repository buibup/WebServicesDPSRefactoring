using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WS_MSO
{
    public static class GlobalConfig
    {
		private const string _dPSLogConnection = "DPSLogConnection";
	   
		public static string DPSLogConnection { get { return _dPSLogConnection; } }

		public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string AppSettings(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
