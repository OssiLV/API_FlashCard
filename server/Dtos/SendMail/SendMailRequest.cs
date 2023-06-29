namespace server.Dtos.SendMail
{
    public class SendMailRequest
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string HtmlMessage { get; set; }
    }
}
