namespace blazorblog.Entity
{
    public class GlobalSetting
    {
        public string SMTPServer { get; set; }
        public string SMTPAuthendication { get; set; }
        public bool SMTPSecure { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPFromEmail { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationLogo { get; set; }
        public string ApplicationHeader { get; set; }
        public string DisqusEnabled { get; set; }
        public string DisqusShortName { get; set; }
    }
}