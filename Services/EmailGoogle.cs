using MailKit.Net.Smtp;
using MimeKit;

namespace LacDau.Services
{
    public class EmailGoogle : IEmailGoogle
    {
        private readonly string _name;
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;

        public EmailGoogle(IConfiguration _config)
        {
            _email = _config["SystemGoogleMail:Acc"];
            _name = _config["SystemGoogleMail:Name"];
            _password = _config["SystemGoogleMail:Pass"];
            _host = _config["SystemGoogleMail:EmailHost"];
            _port = int.Parse(_config["SystemGoogleMail:Port"]);
        }

        public string SendMailChangePassWord(string fullName, string email, string newPass)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_name, "noreply@localhost.com"));
                message.To.Add(new MailboxAddress(fullName, email));
                message.Subject = "Thay đổi mật khẩu";
                message.Body = new TextPart("html") { Text = @$"{_name} Xin gửi thông tin mật khẩu mới của bạn là: <strong>{newPass}</strong>" };

                using (var client = new SmtpClient())
                {
                    client.Connect(_host, _port);
                    client.Authenticate(_email, _password);

                    client.Send(message);
                    client.Disconnect(true);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
