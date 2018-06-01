using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using WebApp.DAL;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Formatters;
using WebApp.Infrastructure.AspNet;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupRepository groupRepository;

        private readonly IProjectRepository projectRepository;
        private readonly LecturerRepository lecturerRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IProjectChoiceRepository projectChoiceRepository;

        public GroupController()
        {
            groupRepository = new GroupRepository();
            projectRepository = new ProjectRepository();
            lecturerRepository = new LecturerRepository();
            studentRepository = new StudentRepository();
            projectChoiceRepository = new ProjectChoiceRepository();
        }

        public ActionResult DeleteGroup(int groupId)
        {
            int numberOfRowsAffected = 0;
            IGroupJoiningRequestRepository groupJoiningRequestRepository = new GroupJoiningRequestRepository();
            try
            {
                IEnumerable<Student> studentsInGroup = studentRepository.GetStudentsByGroupId(groupId);

                foreach (Student s in studentsInGroup)
                    groupRepository.DeleteGroupMember(s.student_id);

                groupJoiningRequestRepository.DeleteGroupJoiningRequestsForGroup(groupId);

                numberOfRowsAffected = groupRepository.DeleteGroup(groupId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult DeleteGroupMember(int studentId)
        {
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupRepository.DeleteGroupMember(studentId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult DownloadInvalidStudentsFile(int[] invalidStudentIds, string[] reasonsForInvalidity)
        {
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "InvalidStudents_" + DateTime.Now.ToString("G") + ".txt");
            //Response.ContentType = "text/plain";
            //Response.Write("Invalid student(s) who were not added to the group:\r\n");
            //for (int i = 0; i < invalidStudentIds.Length; ++i)
            //{
            //    Student s;
            //    s = studentRepository.GetStudentByStudentId(invalidStudentIds[i]);
            //    Response.Write(s.full_name + " (" + s.admin_number + ") --> " + reasonsForInvalidity[i] + "\r\n");
            //}
            //Response.End();
            return null;
        }

        public ActionResult DownloadProjectSelectionSpreadsheet()
        {
            int courseId = 1;
            uint i = 2;
            string cellReference = "A2";
            string fileName = "ProjectSelectionSpreadsheet_" + DateTime.Now.ToString("G") + ".xlsx";
            List<string> headerRow = new List<string>();
            IProjectRepository projectRepository = new ProjectRepository();
            IEnumerable<Project> allAvailableProjects = new List<Project>();
            MemoryStream memoryStream = new MemoryStream();
            DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();

            List<Font> fonts = new List<Font>();
            fonts.Add(new Font(new Bold()));
            fonts.Add(new Font());

            List<ForegroundColor> foregroundColors = new List<ForegroundColor>();
            foregroundColors.Add(new ForegroundColor { Rgb = HexBinaryValue.FromString("FFD633") });
            foregroundColors.Add(new ForegroundColor { Rgb = HexBinaryValue.FromString("FFFFFF") });

            List<BackgroundColor> backgroundColors = new List<BackgroundColor>();
            backgroundColors.Add(new BackgroundColor { Indexed = 64 });
            backgroundColors.Add(new BackgroundColor { Indexed = 64 });

            List<Alignment> alignments = new List<Alignment>();
            alignments.Add(new Alignment());
            alignments.Add(new Alignment());

            SpreadsheetDocument spreadsheetDocument;
            Stylesheet stylesheet;

            stylesheet = ExcelFormatter.CreateStyles(fonts, foregroundColors, backgroundColors, alignments);
            spreadsheetDocument = ExcelFormatter.CreateExcelSpreadsheet(ref memoryStream);
            ExcelFormatter.InitializeSpreadsheet(ref spreadsheetDocument, stylesheet);
            
            List<List<string>> spreadsheetData = groupRepository.GetGroupProjectSelectionSpreadsheetData(courseId);

            allAvailableProjects = projectRepository.GetAvailableProjects(courseId);
            headerRow.AddRange(new string[] { "Group#", "Student ID", "Student Name", "Mobile", "Personal Email"});
            foreach (Project p in allAvailableProjects)
            {
                headerRow.Add(p.project_title);
            }
            sheetData.Append(ExcelFormatter.CreateRow("A1", 1, headerRow, 1));
            foreach (List<string> l in spreadsheetData)
            {
                sheetData.Append(ExcelFormatter.CreateRow(cellReference, i, l, 2));
                ++i;
                cellReference = "A" + i;
            }
            ExcelFormatter.FinalizeSpreadsheetWriting(ref spreadsheetDocument, ref sheetData);

            memoryStream.Seek(0, SeekOrigin.Begin);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.Cookies.Add(new System.Web.HttpCookie("completedDownloadToken", "downloaded"));
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult AddGroupMembers(int[] studentIds, int groupId)
        {
            int numberOfRowsAffected = 0;
            Queue<int> invalidStudentIds = new Queue<int>();
            Queue<string> reasonsForInvalidity = new Queue<string>();
            try
            {
                foreach (int studentId in studentIds)
                {
                    try
                    {
                        numberOfRowsAffected += groupRepository.AddGroupMember(studentId, groupId);
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.Contains("student")) throw;
                        invalidStudentIds.Enqueue(studentId);
                        reasonsForInvalidity.Enqueue(e.Message);
                    }
                }
                if (invalidStudentIds.Count != 0) throw new Exception("Invalid student(s) detected were not added to the group, please refer to the text file for more information.");
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("student"))
                {
                    return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message, invalidStudents = invalidStudentIds, invalidityReasons = reasonsForInvalidity });
                }
                else
                {
                    return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
                }
            }
        }
        public ActionResult ChangeGroupRoles(int[] studentIds, string[] groupRoles)
        {
            int numberOfRowsAffected = 0;
            try
            {
                for (int i = 0; i < studentIds.Length; ++i)
                {
                    numberOfRowsAffected += groupRepository.ChangeGroupRole(studentIds[i], groupRoles[i]);
                }
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult LecturerViewGroups(GroupLecturerViewGroupsViewModel model)
        {
            int courseId = 1; // Assume that this courseId was attained from the session.

            if (model.groupNumber == null)
            {
                try
                {
                    var projects = projectRepository.GetAvailableProjects(courseId);
                    var supervisors = lecturerRepository.GetLecturers(courseId);
                    var group = groupRepository.GetGroup(1, courseId);
                    var view = new GroupLecturerViewGroupsViewModel() { groupDetails = group };
                    view.init(supervisors, projects);
                    
                    ViewData["totalNumberOfGroups"] = groupRepository.GetNumberOfGroups(courseId);
                    return View("~/Views/Group/LecturerViewGroups.cshtml", view);
                }
                catch (Exception e) // Generic Exception handler.
                {
                    ViewData["error"] = e.Message;
                    return View();
                }
            }
            else
            {
                try
                {
                    var group = groupRepository.GetGroup(int.Parse(model.groupNumber), courseId);
                    return (Json(new { groupDetails = group, numberOfGroups = groupRepository.GetNumberOfGroups(courseId) }));
                }
                catch (Exception e)
                {
                    return (Json(new { error = e.Message }));
                }
            }
        }

        public ActionResult StudentViewGroups(GroupStudentViewGroupsViewModel model)
        {
            int courseId = 1;
            int studentId = HttpContext.Session.GetInt32("SID") ?? default(int);
            List<SelectListItem> projectSelectListItems = new List<SelectListItem>();
            IEnumerable<Project> projects;
            IEnumerable<ProjectChoice> projectChoices;
            if (model.groupNumber == null)
            {
                try
                {
                    projects = projectRepository.GetAvailableProjects(courseId);
                    projectChoices = projectChoiceRepository.GetSortedProjectChoicesByStudentId(studentId);

                    projectSelectListItems.Add(new SelectListItem()
                    {
                        Text = "-- Select --",
                        Value = 0.ToString()
                    });

                    foreach (Project p in projects)
                    {
                        projectSelectListItems.Add(new SelectListItem()
                        {
                            Text = p.project_title,
                            Value = p.project_id.ToString()
                        });
                    }
                    var supervisors = lecturerRepository.GetLecturers(courseId);
                    var group = groupRepository.GetGroup(1, courseId);
                    var view = new GroupStudentViewGroupsViewModel() { groupDetails = group };
                    view.init(supervisors, projects);

                    ViewData["totalNumberOfGroups"] = groupRepository.GetNumberOfGroups(courseId);
                    ViewData["projects"] = projectSelectListItems;
                    ViewData["projectChoices"] = projectChoices;
                    
                    return View("~/Views/Group/StudentViewGroups.cshtml", view);
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
                    var group = groupRepository.GetGroup(int.Parse(model.groupNumber), courseId);
                    return (Json(new { groupDetails = group, numberOfGroups = groupRepository.GetNumberOfGroups(courseId) }));
                }
                catch (Exception e)
                {
                    return (Json(new { error = e.Message }));
                }
            }
        }

        public ActionResult ViewGroups(GroupViewGroupsViewModel model)
        {
            int courseId = HttpContext.Session.GetInt32("CourseId") ?? default(int);

            if (model.groupNumber == null)
            {
                try
                {
                    var projects = projectRepository.GetAvailableProjects(courseId);
                    var supervisors = lecturerRepository.GetLecturers(courseId);
                    var students = studentRepository.GetStudents(courseId);
                    var groups = groupRepository.GetGroup(1, courseId);
                    var view = new GroupViewGroupsViewModel() { groupDetails = groups };
                    view.init(supervisors, projects, students);
                    ViewData["totalNumberOfGroups"] = groupRepository.GetNumberOfGroups(courseId);
                    return View("~/Views/Group/ViewGroups.cshtml",view);
                }
                catch (Exception e)
                {
                    ViewData["error"] = e.Message;
					return View("~/Views/Group/ViewGroups.cshtml");
                }
            }
            else
            {
                try
                {
                    var group = groupRepository.GetGroup(int.Parse(model.groupNumber), courseId);
                    return (Json(new { groupDetails = group, numberOfGroups = groupRepository.GetNumberOfGroups(courseId) }));
                }
                catch (Exception e)
                {
                    return (Json(new { error = e.Message }));
                }
            }
        }

        public ActionResult StudentAddGroup()
        {
            /*
             * In addition to the AddGroup() controller action below, this action adds the student who clicked on the
             * "Create a GROUP" button, to the newly created group. That added student is automatically granted the "Leader" role of
             * that group.
             */
            int numberOfRowsAffected = 0;
            int courseId = 1; // Assume this courseId was attained from the Session.
            object recentlyCreatedGroup = null;
            int recentlyCreatedGroupGroupId = 0;
            int studentId = HttpContext.Session.GetInt32("SID") ?? default(int);
            try
            {
                // First automatically delete all pending response group joining request.
                new GroupJoiningRequestRepository().AutoDeletePendingResponseGroupJoinngRequests(studentId);

                // Next attempt to add the new group.
                string prospectiveGroupNumber = groupRepository.GetProspectiveGroupNumber(courseId);
                numberOfRowsAffected = groupRepository.CreateGroup(new Models.Group()
                {
                    group_id = 0,
                    group_number = prospectiveGroupNumber,
                    valid = true,
                    lecturer_id = null,
                    project_id = null,
                    course_id = courseId
                });

                recentlyCreatedGroup = groupRepository.GetGroup(int.Parse(prospectiveGroupNumber.Substring(1)), courseId);
                recentlyCreatedGroupGroupId = int.Parse(recentlyCreatedGroup.GetType().GetProperty("groupId").GetValue(recentlyCreatedGroup, null).ToString());

                // Finally, add the student to the new group.
                groupRepository.AddGroupMember(studentId, recentlyCreatedGroupGroupId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult AddGroup()
        {
            int numberOfRowsAffected = 0;

            try
            {
                int courseId = HttpContext.Session.GetInt32("CourseId") ?? default(int);
                string prospectiveGroupNumber = groupRepository.GetProspectiveGroupNumber(courseId);
                numberOfRowsAffected = groupRepository.CreateGroup(new Models.Group()
                {
                    group_id = 0,
                    group_number = prospectiveGroupNumber,
                    valid = true,
                    lecturer_id = null,
                    project_id = null,
                    course_id = courseId
                });
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult AssignSupervisor(int? lecturerId, int groupId)
        {
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupRepository.AssignSupervisor(lecturerId, groupId);
                return (Json(new { rowsAffected = numberOfRowsAffected }));
            }
            catch (Exception e)
            {
                return (Json(new { rowsAffected = numberOfRowsAffected, error = e.Message }));
            }
        }

        public ActionResult AssignProject(int? projectId, int groupId)
        {
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupRepository.AssignProject(projectId, groupId);
                return (Json(new { rowsAffected = numberOfRowsAffected }));
            }
            catch (Exception e)
            {
                return (Json(new { rowsAffected = numberOfRowsAffected, error = e.Message }));
            }
        }
    }
    }