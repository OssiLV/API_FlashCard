using server.Dtos.SendMail;
using server.Dtos.User.ConfirmEmail;
using server.Dtos.User.OTP;

namespace server.Services.SendMailService
{
    public interface ISendMailService
    {
        string SendOTPConfirmEmail( string Email, string Subject, OTPResponse oTPResponse);
        string SendOTPResetPassWord( string Email, string Subject);
    }
}
