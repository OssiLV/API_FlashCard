namespace server.Dtos
{
    public class LoginResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public LoginResponse( bool IsSuccess, string Message )
        {
            this.Message = Message;
            this.IsSuccess = IsSuccess;
        }
        public LoginResponse( bool IsSuccess, string Message, T Result )
        {
            this.IsSuccess = IsSuccess;
            this.Message = Message;
            this.Result = Result;
        }
    }
}
