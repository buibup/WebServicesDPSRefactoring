using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WS_MSO
{
    public static class Helper
    {
        public static bool IsCheckedAppId(this int appId)
        {
            int? _appId = appId;
            var isAppIdNull = _appId == null ? false : true;

            if (isAppIdNull) return false;
            if (!appId.IsMatchedAppId()) return false;

            return true;
        }

        public static bool IsCheckedAppId(this int appId, int compareAppId)
        {
            int? _appId = appId;
            int? _compareAppId = compareAppId;

            var isAppIdNull = _appId == null ? true : false;
            var isCompareAppIdNull = _compareAppId == null ? true : false;

            if (isAppIdNull || isCompareAppIdNull) return false;
            if (!(_appId == compareAppId)) return false;

            return true;
        }

        public static bool IsMatchProgramId(this string programId)
        {
            var _programId = programId;

            var _programIdString = GlobalConfig.AppSettings("ProgramId");
            var programIdArr = _programIdString.Split('|');

            foreach (var proId in programIdArr)
            {
                if (proId == _programId)
                {
                    return true;
                }
            }

            return false;
        }

        public static DataTable EmptyDataTable()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.TableName = "empty";
            return dt;
        }

        private static bool IsMatchedAppId(this int appId)
        {
            int castAppId = 0;

            var appIdStr = GlobalConfig.AppSettings("AppId");
            var appIdArr = appIdStr.Split('|');


            foreach (var _appId in appIdArr)
            {
                castAppId = int.Parse(_appId);
                if (castAppId == appId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
