using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Xceed;
using WebApp.DAL;
using WebApp.Models;
using WebApp.Formatters;
using WebApp.ViewModels.Proposal;
using WebApp.Infrastructure.AspNet;
using WebApp.ViewModels.InitiatedProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Xceed.Words.NET;
using System.Diagnostics;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Manager, Admin")]
    public class ProposalController : Controller
	{
		private readonly IHostingEnvironment environment;
        private readonly IProposalRepository _proposalctx;
        private readonly ILecturerRepository _lecturerctx;
        private readonly IProjectRepository _projectx;


        public ProposalController(IProposalRepository proposal,
		                          ILecturerRepository lecturer, 
		                          IProjectRepository project,
		                          IHostingEnvironment environment)
        {
            _proposalctx = proposal;
            _lecturerctx = lecturer;
            _projectx = project;
			this.environment = environment;
        }

        public ActionResult Index()
        {
            HttpContext.Session.SetBoolean("IndexStudent", false);
            HttpContext.Session.SetBoolean("IndexUser", false);
			Lecturer lecturer = _lecturerctx.GetLecturer(User.Identity.Name.ToString().Substring(1, User.Identity.Name.ToString().Length - 1));
            int courseId = HttpContext.Session.GetInt32("CourseId") ?? default(int);

            if (User.IsInRole("Admin"))
            {
                IEnumerable<ProposalViewModel> proposals = _proposalctx.Get(lecturer);
                HttpContext.Session.SetBoolean("IndexExternal", true);
                return View("~/Views/_Admin/Proposal/Index.cshtml", proposals);
            }
            else
            {
				Debug.WriteLine(lecturer.lecturer_id);
                IEnumerable<ProposalViewModel> proposals = _proposalctx.GetForLecturer(lecturer);
			
                return View("~/Views/_Lecturer/Proposal/Index.cshtml", proposals);
            }
        }

        [Route("proposal/student")]
        public ActionResult IndexStudent()
        {
            int courseId = HttpContext.Session.GetInt32("CourseId") ?? default(int);
            IEnumerable<dynamic> proposals = _projectx.GetSelfInitiatedProjects(courseId);
            HttpContext.Session.SetBoolean("IndexStudent", true);
            ViewBag.Type = "Student";
            return View("~/Views/_Lecturer/Proposal/Index.cshtml", proposals);
        }
        [Route("proposal/user")]
        public ActionResult IndexUser()
        {
            Lecturer lecturer = _lecturerctx.GetLecturer(User.Identity.Name.ToString());
            int courseId = HttpContext.Session.GetInt32("CourseId") ?? default(int);
            IEnumerable<ProposalViewModel> proposals = _proposalctx.GetForLecturer(lecturer);
            HttpContext.Session.SetBoolean("IndexUser", true);
            return View("~/Views/_Admin/Proposal/Index.cshtml", proposals);
        }

        public ActionResult ViewLatestSubmission(int Id)
        {
            InitiatedProjectViewModel project = _projectx.GetProjectByGroup(Id);
            return Redirect("/proposal/details/" + project.Id);
        }
        public ActionResult Details(int Id)
        {
            dynamic proposal = _proposalctx.GetProposalById(Id);

            proposal.ProjectHistory = _projectx.IsProjectHistory(Id);

            if (User.IsInRole("Admin"))
            {
				if (HttpContext.Session.GetBoolean("IndexExternal") ?? default(bool))
                {
                    ViewBag.Type = "External";
                    HttpContext.Session.SetBoolean("IndexExternal", false);
                }

				if (HttpContext.Session.GetBoolean("IndexStudent") ?? default(bool))
                {
                    ViewBag.Type = "Student";
                    HttpContext.Session.SetBoolean("IndexStudent", false);
                }

                return View("~/Views/_Admin/Proposal/Details.cshtml", proposal);
            }
            return View("~/Views/_Lecturer/Proposal/Details.cshtml", proposal);
        }

        [HttpPost]
        public ActionResult Approve(int Id)
        {
            _proposalctx.ApproveProposal(Id);
            return RedirectToAction("Details", new { Id });

        }
        public ActionResult Create()
        {
            return View("~/Views/_Lecturer/Proposal/Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(ProposalViewModel proposal)
        {
            Lecturer lecturer = _lecturerctx.GetLecturer(User.Identity.Name.ToString());
            if (proposal.Submit != "Submit")
            {
                ModelState.Remove("Title");
                ModelState.Remove("Aims");
                ModelState.Remove("Schedule");
                ModelState.Remove("Objectives");
                ModelState.Remove("TargetAudience");
                ModelState.Remove("MainFunction");
                ModelState.Remove("HardwareAndSoftwareConfiguration");
                ModelState.Remove("TargetAudience");
                if (string.IsNullOrEmpty(proposal.Title))
                {
                    ModelState.AddModelError("Title", "The title is required to save");
                }

                if (ModelState.IsValid)
                {
                    int error = _proposalctx.AddProposal(proposal, lecturer.lecturer_id, lecturer.course_id);
                    if (error == 1)
                    {
                        ModelState.AddModelError("Title", "Title being used by another proposal");
                        return View(proposal);
                    }
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    proposal.SubmittedDate = DateTime.Now;
                    int error = _proposalctx.AddProposal(proposal, lecturer.lecturer_id, lecturer.course_id);
                    if (error == 1)
                    {
                        ModelState.AddModelError("Title", "Title being used by another proposal");
                        return View(proposal);
                    }
                    else
                    {
                        _proposalctx.ApproveProposal(proposal.Id);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(proposal);
        }


        public ActionResult Edit(int Id)
        {
            ProposalViewModel proposal = _proposalctx.GetProposalById(Id);
            return View(proposal);
        }

        [HttpPost]
        public ActionResult Edit(ProposalViewModel proposal)
        {
            if (ModelState.IsValid)
            {
                if (proposal.Submit == "Submit")
                {
                    if (User.IsInRole("Admin"))
                    {
                        _proposalctx.ApproveProposal(proposal.Id);
                    }

                    if (User.IsInRole("Manager"))
                    {
                        proposal.SubmittedDate = DateTime.Now;
                        _proposalctx.UpdateProposal(proposal);
                    }

                }
                else
                {
                    _proposalctx.UpdateProposal(proposal);
                    //ViewBag.Success = true;
                }
                return RedirectToAction("Details", new { proposal.Id });
            }
            return View(proposal);
        }

        public ActionResult Delete(List<int> ids)
        {
            _proposalctx.DeleteProposals(ids);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Generate(int Id)
        {
            dynamic proposal = _proposalctx.GetProposalById(Id);
            string templateName = "External_Project_Proposal.docx";
			string path = Path.Combine(environment.ContentRootPath, "/App_Data/Generated", templateName);
            string downloadPath = string.Format(@"{0}{1}.docx", path, "-" + proposal.Title);
            if (proposal.ProjectType == "External")
            {
                DocX document = new DocxFormatter().GetExternalProjectProposalTemplate();
                document.SaveAs(path);
            }

            if (proposal.ProjectType == "Student")
            {
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
            }
            using (DocX doc = DocX.Load(path))
            {
                //if (doc == null)
                //{
                //DocX document = new DocxFormatter().GetExternalProjectProposalTemplate();
                //document.SaveAs(path);
                //}
                doc.AddCustomProperty(new CustomProperty("ProjectTitle", proposal.Title));
                if (proposal.ProjectType == "External")
                {
                    doc.AddCustomProperty(new CustomProperty("CompanyName", proposal.CompanyName));
                    doc.AddCustomProperty(new CustomProperty("Address", proposal.Address));
                    doc.AddCustomProperty(new CustomProperty("Tel", proposal.Tel));
                    doc.AddCustomProperty(new CustomProperty("Fax", proposal.Fax));
                    doc.AddCustomProperty(new CustomProperty("Email", proposal.Email));
                    doc.AddCustomProperty(new CustomProperty("LiaisonOfficer", proposal.LiaisonOfficer));
                    string WillingToSponsor = "No";
                    if (proposal.WillingToSponsor == true)
                    {
                        WillingToSponsor = "Yes";
                    }
                    doc.AddCustomProperty(new CustomProperty("WillingToSponsor", WillingToSponsor));

                    doc.AddCustomProperty(new CustomProperty("ProjectAims", proposal.Aims));
                    doc.AddCustomProperty(new CustomProperty("ProjectObjectives", proposal.Objectives));
                    doc.AddCustomProperty(new CustomProperty("ProjectSchedule", proposal.Schedule));
                    doc.AddCustomProperty(new CustomProperty("ProjectAudience", proposal.TargetAudience));
                    doc.AddCustomProperty(new CustomProperty("ProjectMainFunction", proposal.MainFunction));
                    doc.AddCustomProperty(new CustomProperty("ProjectRequirements", proposal.HardwareAndSoftwareConfiguration));
                }

                if (proposal.ProjectType == "Student")
                {
                    doc.AddCustomProperty(new CustomProperty("ProjectOverview", proposal.ProjectOverview));
                    doc.AddCustomProperty(new CustomProperty("IntroBackground", proposal.IntroBackground));
                    doc.AddCustomProperty(new CustomProperty("Approach", proposal.KeyInnovationAndResearchGoals));
                    doc.AddCustomProperty(new CustomProperty("ComparisonMerits", proposal.ComparisonOfTheMerits));
                    doc.AddCustomProperty(new CustomProperty("TargetAudience", proposal.TargetAudience));
                    doc.AddCustomProperty(new CustomProperty("BusinessModel", proposal.BusinessModelAndMarketPotential));
                    doc.AddCustomProperty(new CustomProperty("MainFunction", proposal.MainFunction));
                    doc.AddCustomProperty(new CustomProperty("ProjectPlan", proposal.ProjectPlan));
                    doc.AddCustomProperty(new CustomProperty("HardwareAndSoftwareRequirements", proposal.HardwareAndSoftwareRequirements));
                    doc.AddCustomProperty(new CustomProperty("ProblemsAndCountermeasures", proposal.ProblemsAndCountermeasures));
                }

                doc.SaveAs(downloadPath);
            }
            return File(downloadPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(downloadPath));
        }
    }
}