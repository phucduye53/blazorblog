using System;
using System.Diagnostics;
using System.Threading.Tasks;
using blazorblog.Data;
using blazorblog.Entity;
using Microsoft.AspNetCore.Http;

namespace blazorblog.Helpers
{
    public class PerfTimerLogger : IDisposable
    {
        
        public PerfTimerLogger(string message,string curUser ,LogService service ,IHttpContextAccessor httpContextAccessor)
        {
            this.service = service;
            this.message = message;
            this.curUser = curUser;
            this.httpContextAccessor = httpContextAccessor;
            this.timer = new Stopwatch();
            this.timer.Start();
        }
        IHttpContextAccessor httpContextAccessor;
        LogService service;
        string message;
        string curUser;
        Stopwatch timer;

        public void Dispose()
        {
            this.timer.Stop();
            var ms = this.timer.ElapsedMilliseconds;
            var log = new Log();
            log.LogAction = message;
            log.LogDate = DateTime.Now;
            log.LogIpaddress =  httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
            log.LogPerfTime = ms.ToString();
            log.LogExcuteStatus = "Success";
            log.LogUserName = curUser;
            Task.Run(async() => await service.CreateLogAsync(log));
            // log the performance timing with the Logging library of your choice
            // Example:
            // Logger.Write(
            //     string.Format("{0} - Elapsed Milliseconds: {1}", this._message, ms)
            // );
        }

        // using (new PerfTimerLogger("name of code being profiled"))
        // {
        //      // do something you want to profile
        // }
    }
}