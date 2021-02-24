using System.Net;
using System.Net.Mail;

namespace Intsa.Shared.Utils
{
    public static class Socketlabs
    {
        //const string smtpHost = "smtp.socketlabs.com";
        //const int smtpPort = 2525; // standard is port 25 but that is blocked by many ISP's
        //const string smtpUserName = "server24197";
        //const string smtpPassword = "Jq6b9W5Fey8G2MtLa43T";

        public static void SendMessage(string subject, string body, string filename)
        {
            string mailFrom = "info@intsa.net";
            MailAddress from = new MailAddress(mailFrom);

            var objMail = new SmtpClient("smtp.socketlabs.com", 587);
            objMail.UseDefaultCredentials = false;
            objMail.EnableSsl = true;
            objMail.Credentials = new System.Net.NetworkCredential("server24197", "Jq6b9W5Fey8G2MtLa43T");
            objMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Attachment data = new Attachment("");

            MailAddress to = new MailAddress("bryan.cho@intsa.net");
            MailMessage message = new MailMessage(from, to);
            message.From = from;
            message.Subject = subject;     // 메일 제목
            message.IsBodyHtml = true;
            message.Body = body;
            //message.Attachments.Add(data);
            objMail.Send(message);
        }
    }
}
