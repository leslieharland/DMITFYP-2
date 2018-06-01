using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApp.DAL;
using WebApp.Models;
using WebApp.Infrastructure.Communication;
using WebApp.ViewModels.Contact;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IStudentRepository _studentctx;

   

		public ContactController(IStudentRepository student)
        {
            _studentctx = student;
        }
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Compose()
        {
            ComposeViewModel compose = new ComposeViewModel();
            compose.StudentContacts = _studentctx.GetContacts();
            return View("~/Views/_Lecturer/Contact/Compose.cshtml",compose);
        }

        public ActionResult SendSms()
        {
            return PartialView("_Sms");
        }
        public ActionResult GetAllStudentsFullName()
        {
			return Json(_studentctx.GetAllStudentsFullName());
        }

        public ActionResult GetStudentContacts()
        {
            ComposeViewModel compose = new ComposeViewModel();
            compose.StudentContacts = _studentctx.GetContacts();
            return PartialView("~/Views/_Lecturer/Contact/_ContactItem.cshtml", compose);
        }
        [HttpPost]
        public async Task<ActionResult> Send(ComposeViewModel model, string messageText)
        {
            switch (model.typetosend) {
                case "1":
                    await new EmailService().sendEmailToContacts(model.recipients, model.subject, model.content);
                    break;
                case "2":
                    new SmsService().sendSms(messageText, model.recipients);
                    break;
            }
    
            return Redirect("/#contact");
        }
    }
}