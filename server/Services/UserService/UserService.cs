using Azure.Core;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using server.Data.Entities;
using server.Dtos;
using server.Dtos.CheckUser;
using server.Dtos.Register;
using server.Dtos.User.ConfirmEmail;
using server.Dtos.User.OTP;
using server.Dtos.User.ResetPassword;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
        }

        public async Task<bool> DeleteUserByEmail( string email )
        {
            var user = await _userManager.FindByEmailAsync(email);
            var isDeleted = await _userManager.DeleteAsync(user);
            if( isDeleted.Succeeded )
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponse<object>> Authenticate( LoginRequest loginRequest )
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if( user == null ) return new LoginResponse<object>(false, "Email is not exist!!", "");

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, true, true);
            if( !result.Succeeded ) return new LoginResponse<object>(false, "Incorrect Password!!", "");

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(userClaims);
            foreach( var userRole in userRoles )
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if( role != null )
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach( var roleClaim in roleClaims )
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            var accessToken = GenerateAccessToken(claims);

            return new LoginResponse<object>(true, "", new { Token = accessToken, User = new { user.Id, user.UserName, user.Email, user.PhoneNumber, user.EmailConfirmed } });
        }

        private string GenerateAccessToken( List<Claim> claims )
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        public async Task<RegisterResponse> Register( RegisterRequest registerRequest )
        {
            var user = await _userManager.FindByEmailAsync(registerRequest.Email);

            //if( await _userManager.FindByNameAsync(registerRequest.UserName) != null )
            //{
            //    return new RegisterResponse(false, "User Name already exists");
            //}

            if( user != null )
            {
                return new RegisterResponse(false, "User Email already exists");
            }

            user = new AppUser()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                EmailConfirmed = registerRequest.EmailConfirm,
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if( result.Succeeded )
            {
                return new RegisterResponse(true);
            }

            return new RegisterResponse(false, "Cannot register!");
        }

        public async Task<CheckUserResponse> CheckUser( CheckUserRequest checkUserRequest )
        {
            var user = await _userManager.FindByEmailAsync(checkUserRequest.Email);

            if( user != null )
            {
                return new CheckUserResponse(true);
            }

            return new CheckUserResponse(false);
        }

        public async Task<LoginResponse<object>> GoogleLogin( GoogleRequest googleRequest )
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleRequest.Credential);

                var userLoginInfo = new UserLoginInfo("Google", payload.Subject, "Google");
                var user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                if( user == null )
                {
                    user = await _userManager.FindByEmailAsync(payload.Email);

                    if( user == null )
                    {
                        user = new AppUser
                        {
                            FullName = payload.Name,
                            UserName = payload.Email,
                            Email = payload.Email,
                            EmailConfirmed = payload.EmailVerified
                        };

                        var check = await _userManager.CreateAsync(user);

                        if( check.Succeeded )
                        {
                            await _userManager.AddLoginAsync(user, userLoginInfo);
                            var userRoles = await _userManager.GetRolesAsync(user);
                            var userClaims = await _userManager.GetClaimsAsync(user);
                            var claims = new List<Claim>
                            {
                                new Claim("Id", user.Id.ToString()),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Name, user.UserName)
                            };
                            claims.AddRange(userClaims);
                            foreach( var userRole in userRoles )
                            {
                                claims.Add(new Claim(ClaimTypes.Role, userRole));
                                var role = await _roleManager.FindByNameAsync(userRole);
                                if( role != null )
                                {
                                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                                    foreach( var roleClaim in roleClaims )
                                    {
                                        claims.Add(roleClaim);
                                    }
                                }
                            }
                            var accessToken = GenerateAccessToken(claims);

                            return new LoginResponse<object>(true, "Create Success", new { Token = accessToken, User = new { user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.EmailConfirmed } });
                        }
                    }
                    //else
                    //{
                    //    await _userManager.AddLoginAsync(user, userLoginInfo);
                    //    var userRoles = await _userManager.GetRolesAsync(user);
                    //    var userClaims = await _userManager.GetClaimsAsync(user);
                    //    var claims = new List<Claim>
                    //        {
                    //            new Claim("Id", user.Id.ToString()),
                    //            new Claim(ClaimTypes.Email, user.Email),
                    //            new Claim(ClaimTypes.Name, user.UserName)
                    //        };
                    //    claims.AddRange(userClaims);
                    //    foreach( var userRole in userRoles )
                    //    {
                    //        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    //        var role = await _roleManager.FindByNameAsync(userRole);
                    //        if( role != null )
                    //        {
                    //            var roleClaims = await _roleManager.GetClaimsAsync(role);
                    //            foreach( var roleClaim in roleClaims )
                    //            {
                    //                claims.Add(roleClaim);
                    //            }
                    //        }
                    //    }
                    //    var accessToken = GenerateAccessToken(claims);
                    //    return new LoginResponse<object>(true, "SignIn Success", new { Token = accessToken, User = new { user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.EmailConfirmed } });
                    //}
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var claims = new List<Claim>
                            {
                                new Claim("Id", user.Id.ToString()),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Name, user.UserName)
                            };
                    claims.AddRange(userClaims);
                    foreach( var userRole in userRoles )
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                        var role = await _roleManager.FindByNameAsync(userRole);
                        if( role != null )
                        {
                            var roleClaims = await _roleManager.GetClaimsAsync(role);
                            foreach( var roleClaim in roleClaims )
                            {
                                claims.Add(roleClaim);
                            }
                        }
                    }
                    var accessToken = GenerateAccessToken(claims);

                    return new LoginResponse<object>(true, "Signin Success", new { Token = accessToken, User = new { user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.EmailConfirmed } });
                }

                return new LoginResponse<object>(false, "Signin Fail", new { });
            }
            catch( InvalidJwtException ex )
            {
                return new LoginResponse<object>(false, ex.ToString(), new { }); ;
            }
        }

        public async Task<string> GenerateTokenConfirmEmmail( string email )
        {
            var user = await _userManager.FindByEmailAsync(email);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<string> GenerateTokenResetPassword( string email )
        {
            var user = await _userManager.FindByEmailAsync(email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<OTPResponse> GenerateOTP( string token )
        {
            byte[] bytestoken = Encoding.ASCII.GetBytes(token);
            var _totp = new Totp(bytestoken, step: 60, OtpHashMode.Sha512, totpSize: 4);
            var totpCode = _totp.ComputeTotp(DateTime.UtcNow);
            var remainingSeconds = _totp.RemainingSeconds(DateTime.UtcNow);

            return new OTPResponse(OTP: totpCode, Time: remainingSeconds, AccessToken: token);
        }

        public async Task<bool> ConfirmEmail( ConfirmEmailRequest confirmEmailRequest )
        {
            byte[] bytestoken = Encoding.ASCII.GetBytes(confirmEmailRequest.AccessToken);
            var _totp = new Totp(bytestoken, step: 30, OtpHashMode.Sha512, totpSize: 4);
            _totp.VerifyTotp(confirmEmailRequest.OTP, out long timeWindowUsed);

            var user = await _userManager.FindByEmailAsync(confirmEmailRequest.Email);

            await _userManager.ConfirmEmailAsync(user, confirmEmailRequest.AccessToken);

            bool isEmailConfirm = await _userManager.IsEmailConfirmedAsync(user);

            return isEmailConfirm;
        }

        public async Task<bool> ResetPassword( ResetPasswordRequest resetPasswordRequest )
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);

            var reset = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.AccessToken, resetPasswordRequest.Password);

            if( reset.Succeeded ) return true;

            return false;
        }
    }
}