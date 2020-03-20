using System.Threading.Tasks;
using blazorblog.Context;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace blazorblog.Data
{
    public class EmailSender : IEmailSender
    {
        private readonly blogContext _context;
        private readonly EmailService _EmailService;
        private readonly GlobalSettingService _globalSettingService;

        public EmailSender(blogContext context, EmailService EmailService, GlobalSettingService globalSettingService)
        {
            _context = context;
            _EmailService = EmailService;
            _globalSettingService = globalSettingService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return EmailSendAsync(email, subject, message);
        }

        public async Task EmailSendAsync(string email, string subject, string message)
        {
            var objGeneralSettings = await _globalSettingService.GetGeneralSettingsAsync();

            string strError = await _EmailService.SendMailAsync(
                false,
                email,
                email,
                "", "",
                objGeneralSettings.SMTPFromEmail,
                $"Account Confirmation From: {objGeneralSettings.ApplicationName} {subject}",
                $"This is an account confirmation email from: {objGeneralSettings.ApplicationName}. {message}");

            // if (strError != "")
            // {
            //     BlazorBlogs.Data.Models.Logs objLog = new Data.Models.Logs();
            //     objLog.LogDate = DateTime.Now;
            //     objLog.LogUserName = email;
            //     objLog.LogIpaddress = "127.0.0.1";
            //     objLog.LogAction = $"{Constants.EmailError} - Error: {strError} - To: {email} Subject: Account Confirmation From: {objGeneralSettings.ApplicationName} {subject}";
            //     _context.Logs.Add(objLog);
            //     _context.SaveChanges();
            // }
        }
    }
}