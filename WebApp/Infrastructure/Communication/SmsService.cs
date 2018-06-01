using WebApp.Models;
using WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using unirest; 


namespace WebApp.Infrastructure.Communication
{
    public class SmsService
    {
        private static string sender = "";
		//ConfigurationManager.AppSettings["phoneSender"].ToString();
		public void sendSms(string message, string recipients)
		{
			Unirest.post("https://inteltech.p.mashape.com/send.php")
              .header("X-Mashape-Key", "HJZzUnkZZvmshxqoubDF3GbPYPqhp1fouFVjsnFOMh2A5aLuvl")
              .header("Content-Type", "application/x-www-form-urlencoded")
              .header("Accept", "text/plain")
              .field("key", "00494890-649D-C733-6240-182C9247BCA7")
              .field("message", "Test message here.asf")
              .field("method", "mashape")
              .field("return", "")
              .field("schedule", "")
              .field("senderid", "WebApp")
              .field("sms", "+6590055705")
              .field("username", "lesliechen5705").asString();
             
			List<string> mobiles = new List<string>();

			string[] Ids = recipients.Split(',');
			List<int> sanitizeIds = new List<int>();
			int newId;
			foreach (string x in Ids)
			{
				if (int.TryParse(x, out newId))
				{
					sanitizeIds.Add(newId);
				}
			}

			if (!sanitizeIds.Contains(-1000))
			{
				IEnumerable<Student> students = new StudentRepository().GetStudentsFromIds(sanitizeIds);
				foreach (Student s in students)
				{
					mobiles.Add(s.mobile_number);
				}
			}
			else
			{
				IEnumerable<Student> students = new StudentRepository().Get();
				foreach (Student s in students)
				{
					mobiles.Add(s.mobile_number);
				}

			}

			string msg = Uri.EscapeDataString(message);
			string contactsStr = "";
			foreach (string r in mobiles)
			{
				if (!r.Equals(Ids.Last()))
				{
					contactsStr += "65" + r + "%2C";
				}
				else
				{
					contactsStr += "65" + r;
				}
			}
			string encodedUrl = "https://burstsms-burst-sms-singapore.p.mashape.com/send-sms.json/?from=%2B" + sender + "&message=" + msg + "&to=%2B" + contactsStr;
			 Unirest.get(encodedUrl)
			.header("X-Mashape-Key", "HJZzUnkZZvmshxqoubDF3GbPYPqhp1fouFVjsnFOMh2A5aLuvl")
			.header("Accept", "text/plain")
			.asString();
		
        }


    }
}
