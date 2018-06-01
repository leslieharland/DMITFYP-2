using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using WebApp.DAL;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Formatters;
using Microsoft.AspNetCore.Mvc;
using WebApp.Infrastructure.AspNet;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
		private readonly IGroupJoiningRequestRepository groupJoiningRequestRepository;
		private readonly IProjectChoiceRepository projectChoiceRepository;
        
        
		public StudentController(IStudentRepository studentRepository, 
		                         IGroupJoiningRequestRepository groupJoiningRequestRepository,
		                         IProjectChoiceRepository projectChoiceRepository)
        {
			this.studentRepository = studentRepository;
			this.groupJoiningRequestRepository = groupJoiningRequestRepository;
			this.projectChoiceRepository = projectChoiceRepository;
        }

        public ActionResult ViewStudents(StudentViewStudentsViewModel model)
        {
            int totalNumberOfStudents;
            int totalNumberOfPages;
            int courseId = 1;
            if ((model.partialFullName == null) && (model.sortingOrder == null) && (model.startIndex == null) && (model.databaseColumnToSortBy == null))
            {
                try
                {
                    var allStudents = studentRepository.GetStudentsWithPartialFullNameFilter("", new Pagination(1, Constants.PaginationPageSize, Student.AdminNumberDatabaseColumnName, Constants.AscendingOrderSql), courseId);
                    var view = new StudentViewStudentsViewModel() { students = allStudents };
                    totalNumberOfStudents = studentRepository.GetNumberOfStudentsAfterPartialFullNameFilter("", courseId);
                    totalNumberOfPages = (totalNumberOfStudents / Constants.PaginationPageSize) + (totalNumberOfStudents % Constants.PaginationPageSize != 0 ? 1 : 0);
                    ViewData["totalNumberOfPages"] = totalNumberOfPages;
                    return View(view);
                }
                catch (Exception e)
                {
                    ViewData["error"] = e.Message;
                    return View();
                }
            }
            else
            {
                try
                {
                    var allStudents = studentRepository.GetStudentsWithPartialFullNameFilter(model.partialFullName, new Pagination(int.Parse(model.startIndex), Constants.PaginationPageSize, model.databaseColumnToSortBy, model.sortingOrder), courseId);
                    totalNumberOfStudents = studentRepository.GetNumberOfStudentsAfterPartialFullNameFilter(model.partialFullName, courseId);
                    totalNumberOfPages = (totalNumberOfStudents / Constants.PaginationPageSize) + (totalNumberOfStudents % Constants.PaginationPageSize != 0 ? 1 : 0);
                    return Json(new { students = allStudents, numberOfPages = totalNumberOfPages });
                }
                catch (Exception e)
                {
                    return Json(new { error = e.Message });
                }
            }
        }

        public ActionResult DeleteStudent(int studentId)
        {
            /*
             * Deleting a student is quite an impactful action to the system's data because a student record
             * has its primary key (student_id) as a foriegn key in other entities, thus prompting the associated
             * records in those entities to also be deleted.
             * 
             * The worst case scenario for this, is when the deletion of a student affects n number of entities (prompting their records to be removed also), where n is the maximum number of entities that
             * can be affected.
             * 
             * This scenario occurs when the FYP time-period has commenced and the student being in a group and
             * also having appeared to make project choices OR send group joining request(s) before
             * (the commenement of the FYP time-period), has decided to leave SP (or change join another course).
             * In this worst case scenario, n turns out to be 1 (Given that the group entity does not have
             * student's primary key as a foreign key and is not associated to a student, thus n = 3 - 1 = 2.
             * Additionally, as mentioned it is as such that a student can only make project choies OR send
             * group joining request(s), he/ she cannot do both because it does not make sense for a leader who
             * has chosen project choices to send group joining request(s) nor does it make sense for a group
             * member who has sent group joining request(s) to assume the role of the leader and make project
             * choice(s). As such, n = 2 - 1 = 1).
             * 
             * Hence as dicussed above, in the worst case scenario, only at most n entities can be affected, where
             * n is revealed to be 1. That affected entity is either Group_Joining_Request or Project_Choice.
             * 
             * Thus, with the above discussion, the code below deletes associated records from the
             * Project_Choice and Group_Joining_Request entity.
             */

            int numberOfRowsAffected = 0;
           
          

            try
            {
                groupJoiningRequestRepository.DeleteGroupJoiningRequestsForStudent(studentId);
                projectChoiceRepository.DeleteProjectChoicesForStudent(studentId);
                numberOfRowsAffected = studentRepository.DeleteStudent(studentId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }

        }

        public ActionResult DownloadInvalidRowsFile(int[] invalidRowNumbers, string[] reasonsForInvalidity)
        {
            
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "InvalidRows_" + DateTime.Now.ToString("G") + ".txt");
            //Response.ContentType = "text/plain";
            //Response.Write("Invalid student records (rows) in the excel spreadsheet were not added to the system:\r\n");
            //for (int i = 0; i < invalidRowNumbers.Length; ++i)
            //{
            //    Response.Write("Row " + invalidRowNumbers[i] + " --> " + reasonsForInvalidity[i] + "\r\n");
            //}
            //Response.End();
            return null;
        }
        public ActionResult BulkAddStudents()
        {
            int numberOfRowsAffected = 0;
            object spreadSheetStudentData;
                int year = System.DateTime.Now.Year;
                int semester = (System.DateTime.Now.Month >= 2 && System.DateTime.Now.Month <= 8) ? 1 : 2;
                int courseId = 1;
                Queue<string> listOfRowNumbersForInvalidStudents = new Queue<string>();
                List<Student> listOfStudentsToBeAdded = new List<Student>();
                Queue<string> listOfRowNumbersForValidStudents = new Queue<string>();
                List<string> reasonsForInvalidity = new List<string>();

                //if (Request.Files.Count != 1) throw new Exception("The number of files in the HTTP Request Stream is invalid.");

                //spreadSheetStudentData = ExcelReader.readStudentData(Request.Files[0].InputStream);

                //listOfRowNumbersForValidStudents = (Queue<string>)spreadSheetStudentData.GetType().GetProperty("validRowNumbers").GetValue(spreadSheetStudentData, null);
                //listOfStudentsToBeAdded = (List<Student>)spreadSheetStudentData.GetType().GetProperty("validStudents").GetValue(spreadSheetStudentData, null);
                //listOfRowNumbersForInvalidStudents = (Queue<string>)spreadSheetStudentData.GetType().GetProperty("invalidRowNumbers").GetValue(spreadSheetStudentData, null);
                //reasonsForInvalidity = (List<string>)spreadSheetStudentData.GetType().GetProperty("invalidityReasons").GetValue(spreadSheetStudentData, null);
            try
            {
                foreach (Student s in listOfStudentsToBeAdded)
                {
                    try
                    {                     
						s.student_id = 0;             
                        s.group_role = null;                 
                        s.year = year;
                        s.semester = semester;
                        s.completed_module = false;
                        s.group_id = null;                    
                        s.course_id = courseId;
                        numberOfRowsAffected += studentRepository.CreateStudent(s);
                        //new EmailService().sendNewAccountEmail(s.email_address, urlEmbeddedAccountActivationToken, "1240126338");
                        listOfRowNumbersForValidStudents.Dequeue();
                    }
                    catch (Exception e)
                    {
                        if (!(e.Message.Contains("admin") || (e.Message.Contains("email")))) throw;
                        listOfRowNumbersForInvalidStudents.Enqueue(listOfRowNumbersForValidStudents.Dequeue());
                        reasonsForInvalidity.Add(e.Message);
                    }
                }

                if (listOfRowNumbersForInvalidStudents.Count != 0 && reasonsForInvalidity.Count != 0)
                {
                    throw new Exception("Invalid student(s) were not added to the system, please refer to the text file for more information.");
                }
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("student"))
                {
                    return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message, invalidRows = listOfRowNumbersForInvalidStudents, invalidityReasons = reasonsForInvalidity });
                }
                else
                {
                    return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
                }
            }
        }

        public ActionResult AddStudent()
        {
            return View("~/Views/Student/AddStudent.cshtml");
        }

        [HttpPost]
        public ActionResult AddStudent(StudentAddStudentViewModel model)
        {
            int numberOfRowsAffected = 0;
            int courseId = 1; // Assume this courseId was acquired from the current HTTP Session.
            int year = DateTime.Now.Year;
            try
            {
                XDocument xDocument = WebApp.Formatters.XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
                int semester = DateTime.Parse(WebApp.Formatters.XmlReader.GetNodeValue(ref xDocument, "/PersistentData/XmlFileCreatedDate")).Month == 2 ? 1 : 2;
                string urlEmbeddedAccountActivationToken = studentRepository.GenerateRandomUrlEmbeddedAccountActivationToken();

                numberOfRowsAffected = studentRepository.CreateStudent(new Student()
                {
                    admin_number = model.adminNumber,
                    full_name = model.fullName,
                    mobile_number = model.mobileNumber,
                    email_address = model.emailAddress,
                    group_role = null,
                    year = year,
                    semester = semester,
                    completed_module = false,
                    group_id = null,
                    course_id = courseId               
                });
               // new EmailService().sendNewAccountEmail(model.emailAddress, urlEmbeddedAccountActivationToken, "1240126338");
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }
	}
}