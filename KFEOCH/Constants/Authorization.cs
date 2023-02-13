namespace KFEOCH.Constants
{
    public class Authorization
    {
        public enum Roles
        {
            SuperUser,
            Administrator,
            Office,
            OfficeManager,
            DictionaryManager,
            SiteManager,
            ReportManager,
            BillingManager,
        }
        public const string default_username = "mhdyoussef";
        public const string default_email = "mohammad.youssef963@gmail.com";
        public const string default_password = "P@2sW0rd";
        public const Roles superuser_role = Roles.SuperUser;
        public const Roles admin_role = Roles.Administrator;
        public const Roles office_role = Roles.Office;
    }
}
