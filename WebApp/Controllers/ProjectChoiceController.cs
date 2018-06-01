using System;
using System.Web;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApp.DAL;
using WebApp.Models;
using WebApp.Formatters;
using WebApp.Infrastructure.AspNet;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProjectChoiceController : Controller
    {
		private readonly IProjectChoiceRepository projectChoiceRepository;

		public ProjectChoiceController(IProjectChoiceRepository projectChoiceRepository)
        {
			this.projectChoiceRepository = projectChoiceRepository;
        }

        public ActionResult RetrieveSubmittedProjectChoices()
        {
			int? studentId = HttpContext.Session.GetInt32("SID") ?? default(int);
            IEnumerable<ProjectChoice> projectChoices;
            try
            {

				projectChoices = null;
               // projectChoices = projectChoiceRepository.setProjec(studentId);
                return Json(new { submittedProjectChoices = projectChoices });
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        public ActionResult SubmitProjectChoiceWindowPeriod(string startDate, string endDate)
        {
            XDocument xDocument = WebApp.Formatters.XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            int courseId = 1;
            int semester = DateTime.Parse(WebApp.Formatters.XmlReader.GetNodeValue(ref xDocument, "/PersistentData/XmlFileCreatedDate")).Month == 2 ? 1 : 2;
	
            //projectChoiceRepository.set(courseId, semester, startDate, endDate);
            return null;

        }

        public ActionResult SubmitProjectChoices(IEnumerable<int> projectIds)
        {
            int courseId = 1;
            XDocument xDocument = WebApp.Formatters.XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            int semester = DateTime.Parse(WebApp.Formatters.XmlReader.GetNodeValue(ref xDocument, "/PersistentData/XmlFileCreatedDate")).Month == 2 ? 1 : 2;

            DateTime currentTime = DateTime.Now;
            int studentId = HttpContext.Session.GetInt32("SID") ?? default(int);
            int iterationCount = 0;
            int numberOfRowsAffected = 0;
            try
            {
               // if (!projectChoiceRepository.CheckProjectChoiceWindowPeriod(courseId, semester)) throw new Exception("The window period to submit project choice has been either over or it has not arrived yet.");

               // projectChoiceRepository.DeleteProjectChoicesForStudent(studentId);
                foreach (int i in projectIds)
                {
                    ++iterationCount;
                    numberOfRowsAffected += projectChoiceRepository.CreateProjectChoice(new ProjectChoice()
                    {
                        student_id = studentId,
                        project_id = i,
                        ranking_precedence = iterationCount,
                        submitted_date = currentTime
                    });
                }
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }
    }
}