namespace KFEOCH.Constants
{
    public class Authorization
    {
        public enum Roles
        {
            SuperUser,
            Manager,
            Administrator,
            User,
            Office
        }
        public const string default_username = "amr";
        public const string default_email = "amrd@kfeoch.com";
        public const string default_password = "Aa@123456";
        public const Roles default_role = Roles.SuperUser;
        public const Roles admin_role = Roles.Administrator;
        public const Roles office_role = Roles.Office;
    }
}
