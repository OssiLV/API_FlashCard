namespace server.Dtos.User.OTP
{
    public class OTPResponse
    {
        public string OTP { get; set; }
        public int Time { get; set; }
        public string AccessToken { get; set; }

        public OTPResponse( string OTP, int Time, string AccessToken )
        {
            this.OTP = OTP;
            this.Time = Time;
            this.AccessToken = AccessToken;
        }
    }
}
