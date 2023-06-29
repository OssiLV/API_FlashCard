using Microsoft.Extensions.Options;
using server.Dtos.User.ConfirmEmail;
using server.Dtos.User.OTP;
using System.Net;
using System.Net.Mail;

namespace server.Services.SendMailService
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class SendMailService : ISendMailService
    {
        private readonly MailSettings _mailsettings;

        public SendMailService( IOptions<MailSettings> mailSettings )
        {
            _mailsettings = mailSettings.Value;
        }

        public string SendOTPConfirmEmail( string Email, string Subject, OTPResponse oTPResponse )
        {
            try
            {
                string from = _mailsettings.Mail;
                string frompass = _mailsettings.Password;
                string to = Email;

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(from),
                    Subject = Subject,
                    Body = $"<html><body> <p>Your OTP: </p> <h2>{oTPResponse.OTP}</h2> <br> <a href='http://localhost:3000/OTPverifyemail'>Click here to verify your Email</a> </body></html>",
                    IsBodyHtml = true,
                };
                message.To.Add(new MailAddress(to));
                SmtpClient client = new SmtpClient()
                {
                    Port = _mailsettings.Port,
                    Host = _mailsettings.Host,
                    Credentials = new NetworkCredential(from, frompass),

                    EnableSsl = true,
                };
                client.Send(message);
                return "Success";
            }
            catch( Exception ex )
            {
                return ("Fail: " + ex);
            }
        }

        public string SendOTPResetPassWord( string Email, string Subject )
        {
            try
            {
                string from = _mailsettings.Mail;
                string frompass = _mailsettings.Password;
                string to = Email;

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(from),
                    Subject = Subject,
                    Body = $"<html><body> <br> <a href='http://localhost:3000/resetpassword'>Click here to reset your Password</a> </body></html>",
                    IsBodyHtml = true,
                };
                message.To.Add(new MailAddress(to));
                SmtpClient client = new SmtpClient()
                {
                    Port = _mailsettings.Port,
                    Host = _mailsettings.Host,
                    Credentials = new NetworkCredential(from, frompass),

                    EnableSsl = true,
                };
                client.Send(message);
                return "Success";
            }
            catch( Exception ex )
            {
                return ("Fail: " + ex);
            }
        }
    }
}