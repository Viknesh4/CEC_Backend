using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CEC_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("CEC", "nooneee1517@gmail.com")); // Replace with your Gmail
                email.To.Add(new MailboxAddress("", request.To));
                email.Subject = request.Subject;

                var bodyBuilder = new BodyBuilder { TextBody = request.Body };
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate("nooneee1517@gmail.com", "tfxpqavrqqiryphu"); // Replace with your credentials
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return Ok(new { message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending email", error = ex.Message });
            }
        }
    }

    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

