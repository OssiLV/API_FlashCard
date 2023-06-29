using server.Dtos;
using server.Dtos.CheckUser;
using server.Dtos.Register;
using server.Dtos.User.ConfirmEmail;
using server.Dtos.User.OTP;
using server.Dtos.User.ResetPassword;

namespace server.Services.UserService
{
    public interface IUserService
    {
        Task<bool> DeleteUserByEmail(string email);
        Task<LoginResponse<object>> Authenticate( LoginRequest loginRequest );

        Task<RegisterResponse> Register( RegisterRequest registerRequest );

        Task<CheckUserResponse> CheckUser( CheckUserRequest checkUserRequest );

        //Google Login
        Task<LoginResponse<object>> GoogleLogin( GoogleRequest googleRequest );

        //Generate Token
        Task<string> GenerateTokenConfirmEmmail( string email );

        Task<string> GenerateTokenResetPassword( string email );

        //Generate OTP
        Task<OTPResponse> GenerateOTP( string token );

        //Options
        Task<bool> ConfirmEmail( ConfirmEmailRequest confirmEmailRequest );

        Task<bool> ResetPassword( ResetPasswordRequest resetPasswordRequest );
    }
}