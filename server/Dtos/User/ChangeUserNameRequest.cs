namespace server.Dtos.User
{
    public class ChangeUserNameRequest
    {
        public string Email { get; set; }
        public string NewUserName { get; set; }
    }
}