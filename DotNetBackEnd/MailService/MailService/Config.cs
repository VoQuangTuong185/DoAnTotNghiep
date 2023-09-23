namespace MailService
{
    public class Config
    {
        public static IConfiguration Configuration { get; set; }

        private static string AppSettings(string key)
        {
            return Configuration.GetSection("AppConfigs")[key];
        }

        public static string Email => AppSettings("Email");
        public static string Password => AppSettings("Password");
        public static string AppClientRootUrl => AppSettings("AppClientRootUrl");
        public static string BackEndUrl => AppSettings("BackEndUrl");
        public static string Token => AppSettings("Token");
    }
}
