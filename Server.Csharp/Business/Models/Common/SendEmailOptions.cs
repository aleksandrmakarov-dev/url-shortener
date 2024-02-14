namespace Server.Csharp.Business.Models.Common
{
    public class SendEmailOptions
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
