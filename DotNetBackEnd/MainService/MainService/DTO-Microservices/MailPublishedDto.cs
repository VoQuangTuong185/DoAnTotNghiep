namespace DoAnTotNghiep.DTOM
{
    public class MailPublishedDto
    {
        public MailPublishedDto(string business, string userName, string email, string subject, string title, string content, string @event)
        {
            Business = business;
            UserName = userName;
            Email = email;
            Subject = subject;
            Content = content;
            Title = title;
            Event = @event;
        }
        public string Business { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Event { get; set; }
    }
}
