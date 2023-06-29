namespace server.Dtos.User.ConfirmEmail
{
    public class ConfirmEmailRequest
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string OTP { get; set; }

    }
}
