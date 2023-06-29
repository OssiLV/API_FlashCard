namespace server.Dtos.CheckUser
{
    public class CheckUserResponse
    {
        public bool IsExist { get; set; }

        public CheckUserResponse(bool IsExist)
        {
            this.IsExist = IsExist;
        }
    }
    
}
