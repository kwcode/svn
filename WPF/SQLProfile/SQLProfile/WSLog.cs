using SQLProfile.DBCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLProfile
{
    public class WSLog
    {
        public int AddLog(string EventClass, string TextData, string LoginName,
 int SPID, long Duration, string ApplicationName, DateTime  ? StartTime, DateTime ? EndTime, int CPU, int ClientProcessID, string NTUserName, long Reads, long Writes, string DatabaseName, string HostName, long TransactionID)
        {
            return DataConnect.Data.ExecuteSP("p_SP_AddLog", EventClass, TextData, LoginName, SPID, Duration, ApplicationName, StartTime, EndTime, CPU, ClientProcessID, NTUserName,
Reads, Writes, DatabaseName, HostName, TransactionID);
        }
    }
}
