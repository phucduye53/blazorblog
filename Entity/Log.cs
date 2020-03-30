using System;

namespace blazorblog.Entity
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogAction { get; set; }
        public string LogUserName { get; set; }
        public string LogIpaddress { get; set; }

        public string LogExcuteStatus { get; set; }
        public string LogPerfTime { get; set;}
    }
}