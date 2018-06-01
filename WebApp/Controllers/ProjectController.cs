using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using WebApp.DAL;
using WebApp.Models;
using WebApp.Formatters;
using WebApp.Infrastructure.AspNet;
using WebApp.ViewModels.InitiatedProject;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectctx;
        private readonly IProposalRepository _proposalctx;
        private readonly IStudentRepository _studentctx;
        private static ICollection<Project> _projects;


        public ProjectController(IProjectRepository project,
		                         IStudentRepository student, 
		                         IProposalRepository proposal)
        {
            _projectctx = project;
            _studentctx = student;
            _proposalctx = proposal;
        }

        public ActionResult StudentIndex()
        {
            int courseId = 1;
            var projects = _projectctx.GetAvailableProjects(courseId).ToList();
            _projects = projects;
            return View(_projects);
        }
        // GET: Project
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            IProjectChoiceRepository projectChoiceRepository = new ProjectChoiceRepository();
            XDocument xDocument = XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            int courseId = 1;
            int semester = DateTime.Parse(XmlReader.GetNodeValue(ref xDocument, "/PersistentData/XmlFileCreatedDate")).Month == 2 ? 1 : 2;
            List<string> projectChoiceWindowPeriod = projectChoiceRepository.GetProjectChoiceWindowPeriod(courseId, semester);
            var projects = _projectctx.Get().ToList();
            _projects = projects;
            ViewData["ProjectChoiceWindowPeriodStartDate"] = projectChoiceWindowPeriod[0];
            ViewData["ProjectChoiceWindowPeriodEndDate"] = projectChoiceWindowPeriod[1];
            return View("~/Views/Project/Index.cshtml",_projects);
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult SelfInitiated()
        {
			Student student = _studentctx.GetStudent(User.Identity.Name);

            if (student.group_id == null)
            {
                int? groupId = student.group_id;
                bool notingroup = groupId.GetValueOrDefault(0) == 0;
                if (notingroup)
                {
                    ViewBag.NotInGroup = true;
                }
                return View("_Create", new InitiatedProjectViewModel());
            }
            else
            {
                InitiatedProjectViewModel project = _projectctx.GetProjectByGroup(int.Parse(student.group_id.ToString()));
                return View("SelfInitiated", project);
            }

        }

        [HttpPost]
        public ActionResult SelfInitiated(string resubmit = "", InitiatedProjectViewModel project = null)
        {

            if (resubmit == "" || resubmit == "resubmit")
            {
                return ValidateSubmission(project, resubmit);
            }
            else
            {
                ModelState.Clear();
                return View("_Create");
            }
        }

        public ActionResult ValidateSubmission(InitiatedProjectViewModel project, string resubmit)
        {
             Student student = _studentctx.GetStudent(User.Identity.Name);
                int? groupId = student.group_id;
            if (project.Submit != "Submit")
            {
                ModelState.Clear();
                if (string.IsNullOrEmpty(project.Title))
                {
                    ModelState.AddModelError("", "Enter the proposal title to save");
                }
                else
                {
                    if (project.displayFields != null)
                    {

                        var model = JsonConvert.DeserializeObject<List<ViewModels.InitiatedProject.Field>>(project.displayFields);
                        foreach (ViewModels.InitiatedProject.Field f in model)
                        {
                            if (f.value.Length > 500)
                            {
                                ModelState.AddModelError("", "One of the fields '" + f.label + "' has exceeded maximum of 500 characters");
                                project.displayFieldsModel = model;
                                return View("_Create", project);
                            }
                        }

                    }
                    project.SavedDate = DateTime.Now;
                    _projectctx.AddSelfInitiatedProject(project, (int)groupId, student.course_id);
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                if (project.displayFields != null)
                {
                    var model = JsonConvert.DeserializeObject<List<ViewModels.InitiatedProject.Field>>(project.displayFields);
                    foreach (ViewModels.InitiatedProject.Field f in model)
                    {
                        if (f.value.Length > 500)
                        {
                            ModelState.AddModelError("", "One of the fields '" + f.label + "' has exceeded maximum of 500 characters");
                            return View("_Create", project);
                        }
                    }
                }
                switch (resubmit) {
                    case "":
                        if (ModelState.IsValid)
                        {
                            if (_projectctx.GetProjectByGroup((int)student.group_id) == null)
                            {
                                project.SubmittedDate = DateTime.Now;
                                _projectctx.AddSelfInitiatedProject(project, (int)groupId, student.course_id);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        break;
                    case "resubmit":
                            project.SubmittedDate = DateTime.Now;
                            _projectctx.AddProjectHistory(_projectctx.GetProjectByGroup((int)student.group_id).Id);
                            _projectctx.AddSelfInitiatedProject(project, (int)groupId, student.course_id);                        
                            return RedirectToAction("Index", "Home");
                }
                   
                
            }
            return View("_Create");
        }
        // GET: Project/Edit/5
        public ActionResult Edit(int Id)
        {
			InitiatedProjectViewModel project = _projectctx.GetProject(Id, HttpContext.Session.GetInt32("GroupId") ?? default(int));
            if (project == null)
            {
				return View();
                //return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(InitiatedProjectViewModel project)
        {
            _projectctx.DeleteProjectFields(project);
            if (project.Submit != "Submit")
            {
                if (string.IsNullOrEmpty(project.Title))
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Enter the proposal title to save");
                }
                else
                {
                    if (project.displayFields != null)
                    {

                        var model = JsonConvert.DeserializeObject<List<ViewModels.InitiatedProject.Field>>(project.displayFields);
                        foreach (ViewModels.InitiatedProject.Field f in model)
                        {
                            if (f.value.Length > 500)
                            {
                                ModelState.AddModelError("", "One of the fields '" + f.label + "' has exceeded maximum of 500 characters");
                                project.displayFieldsModel = model;
                                return View("Edit", project);
                            }
                        }

                    }
                    project.SavedDate = DateTime.Now;
                    _projectctx.UpdateProject(project);
                    return RedirectToAction("SelfInitiated");
                }

            }
            else
            {

                if (ModelState.IsValid)
                {
                    project.SubmittedDate = DateTime.Now;
                    _projectctx.UpdateProject(project);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            _projectctx.DeleteProject(id);
            return RedirectToAction("SelfInitiated");
        }

        public ActionResult DownloadCombinedProposalsSpreadsheet()
        {
            int courseId = 1;
            uint i = 2;
            string cellReference = "A2";
            string fileName = "CombinedProposalsSpreadsheet_" + DateTime.Now.ToString("G") + ".xlsx";
            List<string> headerRow = new List<string>();
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

            List<DocumentFormat.OpenXml.Spreadsheet.Alignment> alignments = new List<DocumentFormat.OpenXml.Spreadsheet.Alignment>();
            alignments.Add(new DocumentFormat.OpenXml.Spreadsheet.Alignment());
            alignments.Add(new DocumentFormat.OpenXml.Spreadsheet.Alignment { WrapText = true, Vertical = VerticalAlignmentValues.Top });

            SpreadsheetDocument spreadsheetDocument;
            Stylesheet stylesheet;

            stylesheet = ExcelFormatter.CreateStyles(fonts, foregroundColors, backgroundColors, alignments);
            spreadsheetDocument = ExcelFormatter.CreateExcelSpreadsheet(ref memoryStream);
            ExcelFormatter.InitializeSpreadsheet(ref spreadsheetDocument, stylesheet);
            List<List<string>> spreadsheetData = _projectctx.GetCombinedProjectsSpreadsheetData(courseId);
            headerRow.AddRange(new string[] { "S/N", "Company", "Project Background", "Project Title", "Project Brief", "Technology Specification" });
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

        [AllowAnonymous]
        public ActionResult Download(int id)
        {
            InitiatedProjectViewModel proposal = _proposalctx.GetProposalById(id);
            string templateName = "External_Project_Proposal.docx";
            string path = Path.Combine(Server.MapPath("~/App_Data/Generated"), templateName);
            string downloadPath = string.Format(@"{0}{1}.docx", path, "-" + proposal.Title);


            DateTime currentDt = DateTime.Now;
            DateTime Sem1Start = new DateTime(currentDt.Year, 4, 20);
            DateTime Sem2Start = new DateTime(currentDt.Year, 10, 19);
            string semester = "";
            if (DateHelper.Between(currentDt, Sem1Start, Sem2Start))
            {
                semester = "1";
            }
            else
            {
                semester = "2";
            }
            DocX document = new DocxFormatter().GetInitiatedProjectProposal(currentDt, semester);
            document.SaveAs(path);

            using (DocX doc = DocX.Load(path))
            {
                doc.AddCustomProperty(new Novacode.CustomProperty("ProjectTitle", proposal.Title));
                doc.AddCustomProperty(new Novacode.CustomProperty("ProjectOverview", proposal.ProjectOverview));
                doc.AddCustomProperty(new Novacode.CustomProperty("IntroBackground", proposal.IntroBackground));
                doc.AddCustomProperty(new Novacode.CustomProperty("Approach", proposal.KeyInnovationAndResearchGoals));
                doc.AddCustomProperty(new Novacode.CustomProperty("ComparisonMerits", proposal.ComparisonOfTheMerits));
                doc.AddCustomProperty(new Novacode.CustomProperty("TargetAudience", proposal.TargetAudience));
                doc.AddCustomProperty(new Novacode.CustomProperty("BusinessModel", proposal.BusinessModelAndMarketPotential));
                doc.AddCustomProperty(new Novacode.CustomProperty("MainFunction", proposal.MainFunction));
                doc.AddCustomProperty(new Novacode.CustomProperty("ProjectPlan", proposal.ProjectPlan));
                doc.AddCustomProperty(new Novacode.CustomProperty("HardwareAndSoftwareRequirements", proposal.HardwareAndSoftwareRequirements));
                doc.AddCustomProperty(new Novacode.CustomProperty("ProblemsAndCountermeasures", proposal.ProblemsAndCountermeasures));
                doc.SaveAs(downloadPath);
            }
            return File(downloadPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(downloadPath));
        }

        public ActionResult DownloadIndustryLecturerSubmittedProposalsSpreadsheet()
        {
            int courseId = 1;
            uint i = 2;
            string cellReference = "A2";
            string fileName = "Industry-LecturerProposalsSpreadsheet_" + DateTime.Now.ToString("G") + ".xlsx";
            List<string> headerRow = new List<string>();
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

            List<DocumentFormat.OpenXml.Spreadsheet.Alignment> alignments = new List<DocumentFormat.OpenXml.Spreadsheet.Alignment>();
            alignments.Add(new DocumentFormat.OpenXml.Spreadsheet.Alignment());
            alignments.Add(new DocumentFormat.OpenXml.Spreadsheet.Alignment { WrapText = true, Vertical = VerticalAlignmentValues.Top });
            SpreadsheetDocument spreadsheetDocument;
            Stylesheet stylesheet;

            stylesheet = ExcelFormatter.CreateStyles(fonts, foregroundColors, backgroundColors, alignments);
            spreadsheetDocument = ExcelFormatter.CreateExcelSpreadsheet(ref memoryStream);
            ExcelFormatter.InitializeSpreadsheet(ref spreadsheetDocument, stylesheet);

            List<List<string>> spreadsheetData = _projectctx.GetIndustryLecturerProjectsSpreadsheetData(courseId);
            headerRow.AddRange(new string[] { "S/N", "Company", "Project Title", "Project Brief", "Technology Specification" });
            sheetData.Append(ExcelFormatter.CreateRow("A1", 1, headerRow, 1));
            foreach (List<string> l in spreadsheetData)
            {
                sheetData.Append(ExcelFormatter.CreateRow(cellReference, i, l, 2));
                ++i;
                cellReference = "A" + i;
            }
            ExcelFormatter.FinalizeSpreadsheetWriting(ref spreadsheetDocument, ref sheetData);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Response.AddHeader("Content-Disposition", "attachement; filename=" + fileName);
            Response.Cookies.Add(new System.Web.HttpCookie("completedDownloadToken", "downloaded"));
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult DownloadStudentSubmittedProposalsSpreadsheet()
        {
            int courseId = 1;
            uint i = 2;
            string cellReference = "A2";
            string fileName = "StudentProposalsSpreadsheet_" + DateTime.Now.ToString("G") + ".xlsx";
            List<string> headerRow = new List<string>();
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

            List<DocumentFormat.OpenXml.Spreadsheet.Alignment> alignments = new List<DocumentFormat.OpenXml.Spreadsheet.Alignment>();
            alignments.Add(new DocumentFormat.OpenXml.Spreadsheet.Alignment());
            alignments.Add(new DocumentFormat.OpenXml.Spreadsheet.Alignment { WrapText = true, Vertical = VerticalAlignmentValues.Top });

            SpreadsheetDocument spreadsheetDocument;
            Stylesheet stylesheet;

            stylesheet = ExcelFormatter.CreateStyles(fonts, foregroundColors, backgroundColors, alignments);
            spreadsheetDocument = ExcelFormatter.CreateExcelSpreadsheet(ref memoryStream);
            ExcelFormatter.InitializeSpreadsheet(ref spreadsheetDocument, stylesheet);

            List<List<string>> spreadsheetData = _projectctx.GetSelfInitiatedProjectsSpreadsheetData(courseId);
            headerRow.AddRange(new string[] { "S/N", "Project Background", "Project Title", "Project Brief", "Technology Specification" });
            sheetData.Append(ExcelFormatter.CreateRow("A1", 1, headerRow, 1));
            foreach (List<string> l in spreadsheetData)
            {
                sheetData.Append(ExcelFormatter.CreateRow(cellReference, i, l, 2));
                ++i;
                cellReference = "A" + i;
            }
            ExcelFormatter.FinalizeSpreadsheetWriting(ref spreadsheetDocument, ref sheetData);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Response.AddHeader("Content-Disposition", "attachement; filename=" + fileName);
            Response.Cookies.Add(new System.Web.HttpCookie("completedDownloadToken", "downloaded"));
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
