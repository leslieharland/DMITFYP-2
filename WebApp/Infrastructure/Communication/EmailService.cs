using System.Net;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using WebApp.DAL;
using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Linq;

namespace WebApp.Infrastructure.Communication
{
    public class EmailService
    {
        //static string username = ConfigurationManager.AppSettings.Get("sendgridApiUser");
        //static string mail = ConfigurationManager.AppSettings.Get("mailAccount");
        //static string pswd = ConfigurationManager.AppSettings.Get("mailPassword");

        public async Task sendNewAccountEmail(string email, string passwordLink, string refid)
        {          
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            //myMessage.From = new EmailAddress(mail, "WebApp Web Support Team");
            myMessage.Subject = "Your new account created on DMIT-FYP Web Online System";
            myMessage.HtmlContent = "<h2 style=\"color:brown\">Hi there,</h2>";
            myMessage.HtmlContent += "<p>We are writing to inform you that your new account is ready to be used! However, you would first have to set a new password for it.</p>";
            myMessage.HtmlContent += "<p/>Please copy the following link to your browser or click to view in your browser:<br /><a href=\"http://localhost:1111/Account/Password?passwordLink=" + passwordLink + "&refid=" + refid + "\">http://localhost:1111/Account/Password?passwordLink=" + passwordLink + "&refid=" + refid + "</a></p><br/>";
            myMessage.HtmlContent += "<b>Warmest Regards,</b><br/>";
            myMessage.HtmlContent += "<a href=\"http://localhost:1111\">WebApp Web Support Team</a>";

            await Send(myMessage);

        }

        public async Task sendRecoveryEmail(string email, string passwordLink, string refid)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            //myMessage.From = new EmailAddress(mail, "WebApp Web Support Team");
            myMessage.Subject = "Password reset on WebApp";
            myMessage.HtmlContent = "Please copy the following link to your browser or <a href=\"http://localhost:1111/Account/Password?passwordLink=" + passwordLink + "&refid=" + refid + "\">click to view </a><p style='margin-top: 100px'>WebApp Web Support Team</p>";

            await Send(myMessage);

        }

        public async Task sendEmailToContacts(string recipients, string subject, string content)
        {
            List<EmailAddress> emails = new List<EmailAddress>();

            string[] Ids = recipients.Split(',');
            List<int> sanitizeIds = new List<int>();
            int newId;
            foreach (string x in Ids)
            {
                if (int.TryParse(x, out newId) == false)
                {
                    emails.Add(new EmailAddress(x, ""));   
                }else
                {
                    sanitizeIds.Add(newId);
                }
            }
        
            SendGridMessage myMessage = new SendGridMessage();
            if (!sanitizeIds.Contains(-1000))
            {
                IEnumerable<Student> students = new StudentRepository().GetStudentsFromIds(sanitizeIds);
                foreach (Student s in students)
                {
                    emails.Add(new EmailAddress(s.email_address, ""));
                }
            }else
            {
                IEnumerable<Student> students = new StudentRepository().Get();
                foreach (Student s in students)
                {
                    emails.Add(new EmailAddress(s.email_address, ""));
                }

            }
            
            myMessage.AddTos(emails);
           // myMessage.From = new EmailAddress(mail, "WebApp Web Support Teams");
            myMessage.Subject = subject;
            myMessage.HtmlContent = content;
            await Send(myMessage);
        }

        private async Task Send(SendGridMessage message)
        {
            //var transportWeb = new Web(new NetworkCredential(username, pswd));
            //try {
             // await transportWeb.DeliverAsync(message);
            //}
            //catch (Exception ex)
            //{
            //    var detalle = new StringBuilder();

            //    detalle.Append("ResponseStatusCode: " + ex.ResponseStatusCode + ".   ");
            //    for (int i = 0; i < ex.Errors.Count(); i++)
            //    {
            //        detalle.Append(" -- Error #" + i.ToString() + " : " + ex.Errors[i]);
            //    }

            //    throw new ApplicationException(detalle.ToString(), ex);
            //}
            throw new NotImplementedException();
        }
    }
}