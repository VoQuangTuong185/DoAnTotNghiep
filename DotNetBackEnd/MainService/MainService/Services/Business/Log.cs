using System.Text;
using WebAppAPI.Services.Contracts;
namespace WebAppAPI.Services.Business
{
    public sealed class Log : ILog
    {
        private Log()
        {
        }
        private static readonly Lazy<Log> instance = new Lazy<Log>(() => new Log());

        public static Log GetInstance
        {
            get
            {
                return instance.Value;
            }
        }

        public void LogException(string message)
        {
            string fileName = string.Format("{0}_{1}.log", "Exception", DateTime.Now.ToString("dd MM yyyy"));
            string folderName = Path.Combine("Resources", "Logs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string logFilePath = string.Format(@"{0}\{1}", pathToSave, fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.Write(sb.ToString());
                writer.Flush();
            }
        }
    }
}
