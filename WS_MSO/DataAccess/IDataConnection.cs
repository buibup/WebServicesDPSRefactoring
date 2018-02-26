using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_MSO.DataAccess
{
    public interface IDataConnection
    {
        DataTable GetData(string queryString, string tableName);
        DataTable GetData(string queryString, string tableName, Dictionary<string, dynamic> paramDic);
        Tuple<int, string> InsertOrUpdateData(string queryString, Dictionary<string, dynamic> paramDic);
    }
}
