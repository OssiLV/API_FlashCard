namespace server.Dtos.Register
{
    public class RegisterResponse
    {

        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public RegisterResponse( bool IsSuccess )
        {
            this.IsSuccess = IsSuccess;
        }
        public RegisterResponse(bool IsSuccess, string Message)
        {
            this.IsSuccess = IsSuccess;
            this.Message = Message;
        }
    }
}
