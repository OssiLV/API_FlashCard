namespace server.Dtos.Register
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EmailConfirm { get; set; }
    }
}
