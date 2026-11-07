using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsDataAccessLogs
    {
        public static void CreateExceptionLog(string Message)
        {
            string SourceName = "DVLD";
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Error);
            }
        }
    }
}
