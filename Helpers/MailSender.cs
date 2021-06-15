using MimeKit;
using MailKit.Net.Smtp;

namespace Courses.Helpers
{
    public static class MailSender
    {
        public static void Send(string to, string subject, string body)
        {
            if (to == null)
                return;

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("fit.rs1.seminarski.rad@gmail.com", "Jasmina1331.");

                var mailMessage = new MimeMessage
                {
                    Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = body
                    },
                    From =
                            {
                                new MailboxAddress("Courses", "fit.rs1.seminarski.rad@gmail.com")
                            },
                    To =
                            {
                                MailboxAddress.Parse(to)
                            },
                    Subject = subject
                };

                client.Send(mailMessage);

                client.Disconnect(true);
            }
        }
    }
}
