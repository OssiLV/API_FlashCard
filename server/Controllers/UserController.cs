using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Dtos.CheckUser;
using server.Dtos.Register;
using server.Dtos.SendMail;
using server.Dtos.User.ConfirmEmail;
using server.Dtos.User.ResetPassword;
using server.Services.SendMailService;
using server.Services.UserService;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISendMailService _sendMailService;

        public UserController( IUserService userService, ISendMailService sendMailService )
        {
            _userService = userService;
            _sendMailService = sendMailService;
        }

        [Authorize]
        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteUser(string email )
        {
            var check = await _userService.DeleteUserByEmail(email);
            if( check ) return Ok(check);
            return BadRequest(check);
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Login( LoginRequest loginRequest )
        {
            var data = await _userService.Authenticate(loginRequest);

            if( data.IsSuccess )
            {
                return Ok(data.Result);
            }

            return BadRequest(data.Message);
        }
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Register( RegisterRequest registerRequest )
        {
            var data = await _userService.Register(registerRequest);

            if( data.IsSuccess )
            {
                return Ok("Success");
            }
            return BadRequest(data.Message);
        }

        [HttpPost("checkuser")]
        public async Task<IActionResult> CheckUserByEmail( CheckUserRequest checkUserRequest )
        {
            var check = await _userService.CheckUser(checkUserRequest);

            if( check.IsExist )
            {
                return Ok(check.IsExist);
            }

            return Ok(check.IsExist);
        }

        [HttpGet("otp/{email}")]
        public async Task<IActionResult> GenerateOTP( string email )
        {
            var token = await _userService.GenerateTokenConfirmEmmail(email);
            var OTP = await _userService.GenerateOTP(token);

            _sendMailService.SendOTPConfirmEmail(email, "FlashCard", OTP);

            return Ok(OTP);
        }

        [HttpGet("token-resetpassword/{email}")]
        public async Task<IActionResult> GenerateTokenResetPassword( string email )
        {
            var token = await _userService.GenerateTokenResetPassword(email);
            _sendMailService.SendOTPResetPassWord(email, "FlashCard");
            return Ok(token);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword( ResetPasswordRequest resetPasswordRequest )
        {
            var check = await _userService.ResetPassword(resetPasswordRequest);
            if( check ) return Ok(check);
            return BadRequest(check);
        }

        [HttpPost("verifyemail")]
        public async Task<IActionResult> ConfirmEmail( ConfirmEmailRequest confirmEmailRequest )
        {
            var check = await _userService.ConfirmEmail(confirmEmailRequest);
            return Ok(check);
        }

        [AllowAnonymous]
        [HttpPost("auth/google")]
        public async Task<IActionResult> GoogleAuth( [FromBody] GoogleRequest googleRequest )
        {
            var check = await _userService.GoogleLogin(googleRequest);

            return Ok(check);
        }
    }
}