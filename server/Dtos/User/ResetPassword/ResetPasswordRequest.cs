namespace server.Dtos.User.ResetPassword
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Password { get; set; }
    }
}
