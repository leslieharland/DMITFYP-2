using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Dapper;
using WebApp.Infrastructure.AspNet;
using WebApp.ViewModels.Proposal;
using Microsoft.Extensions.Configuration;
using WebApp.Models;
using WebApp.ViewModels.InitiatedProject;

namespace WebApp.DAL
{
    public class ProposalRepository : IProposalRepository
    {
		private readonly IConfiguration configuration;
        public String connectionString;
		public ProposalRepository(IConfiguration config)
        {
            configuration = config;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        private IDbConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        //dynamic p = new ExpandoObject();
        public IEnumerable<ProposalViewModel> Get(int? lecturerId)
        {
            using (var conn = GetConnection())
            {
               
                //var projects = connection.Query("Select * from Industry_Lecturer_Project Inner Join Project on Industry_Lecturer_Project.project_id = Project.project_id where lecturer_id = @lecturerId", new { lecturerId });
                var projects = conn.Query("Select * from Industry_Lecturer_Project Inner Join Project on Industry_Lecturer_Project.project_id = Project.project_id");
                List<ProposalViewModel> proposalItems = new List<ProposalViewModel>();
                foreach (var i in projects)
                {
                    ProposalViewModel p = new ProposalViewModel();
                    p.Id = i.project_id;
                    p.Title = i.project_title;
                    p.Aims = i.project_aims;
                    p.Objectives = i.project_objectives;
                    p.Deadline = i.deadline.ToString("dd/MM/yyyy");
                    p.TargetAudience = i.target_audience;
                    p.Schedule = i.project_milestones;
                    p.MainFunction = i.main_functions_and_deliverables;
                    p.HardwareAndSoftwareConfiguration = i.hardware_and_software_requirements;
                    p.CompanyName = i.company_name;
                    p.Address = i.company_postal_address;
                    p.Tel = i.company_telephone_number;
                    p.Fax = i.company_fax_number;
                    p.Email = i.company_email_address;
                    p.LiaisonOfficer = i.company_liaison_officer_name;
                    p.WillingToSponsor = i.company_sponsorship_willingness;
                    p.Availability = i.project_availability;

                    proposalItems.Add(p);
                }
                return proposalItems;
            }
        }

        

        public void AddProposal(ProposalViewModel proposal)
        {
            using (var conn = GetConnection())
            {
				conn.Execute("insert into Industry_Lecturer_Project(company_name, company_postal_address, company_telephone_number, company_email_address) values(@CompanyName, @Address, @Tel, @Email)", proposal);
            }
        }

        public void UpdateProposal(ProposalViewModel proposal)
        {
            using (var conn = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inProjectId", proposal.Id, dbType: DbType.Int32);
                p.Add("@inCompanyName", proposal.CompanyName, dbType: DbType.String, size: 50);
                p.Add("@inAddress", proposal.Address, dbType: DbType.String, size: 100);
                p.Add("@inTel", proposal.Tel, dbType: DbType.StringFixedLength, size: 8);
                p.Add("@inFax", proposal.Fax, dbType: DbType.StringFixedLength, size: 8);
                p.Add("@inEmailAddress", proposal.Email, dbType: DbType.String, size: 50);
                p.Add("@inLiaisonOfficer", proposal.LiaisonOfficer, dbType: DbType.String, size: 50);
                p.Add("@inWillingToSponsor", proposal.WillingToSponsor, dbType: DbType.Boolean);
                p.Add("@inTitle", proposal.Title, dbType: DbType.String, size: 50);
                p.Add("@inAims", proposal.Aims, dbType: DbType.String, size: 500);
                p.Add("@inObjectives", proposal.Objectives, dbType: DbType.String, size: 500);
                p.Add("@inAudience", proposal.TargetAudience, dbType: DbType.String, size: 500);
                p.Add("@inMainFunction", proposal.MainFunction, dbType: DbType.String, size: 500);
                p.Add("@inRequirements", proposal.HardwareAndSoftwareConfiguration, dbType: DbType.String, size: 500);
                p.Add("@inSchedule", proposal.Schedule, dbType: DbType.String, size: 500);

				conn.Execute("usp_updateExternalProject", p, commandType: CommandType.StoredProcedure);
            }
        }


        public IEnumerable<ProposalViewModel> GetForLecturer(Lecturer lecturer)
        {
            int lecturerId = lecturer.lecturer_id;
            var projects = new object();
            using (var connection = GetConnection())
            {
                projects = connection.Query("Select * from Industry_Lecturer_Project Inner Join Project on Industry_Lecturer_Project.project_id = Project.project_id where course_id = @courseId and lecturer_id = @lecturerId", new { courseId = lecturer.course_id, lecturerId });
                List<ProposalViewModel> proposalItems = new List<ProposalViewModel>();
                foreach (var i in projects as IEnumerable<dynamic>)
                {
                    ProposalViewModel p = new ProposalViewModel();
                    p.Id = i.project_id;
                    p.Title = i.project_title;
                    p.Aims = i.project_aims;
                    p.Objectives = i.project_objectives;
                    p.Deadline = i.deadline.ToString("dd/MM/yyyy");
                    p.TargetAudience = i.target_audience;
                    p.Schedule = i.project_milestones;
                    p.MainFunction = i.main_functions_and_deliverables;
                    p.HardwareAndSoftwareConfiguration = i.hardware_and_software_requirements;
                    p.CompanyName = i.company_name;
                    p.Address = i.company_postal_address;
                    p.Tel = i.company_telephone_number;
                    p.Fax = i.company_fax_number;
                    p.Email = i.company_email_address;
                    p.LiaisonOfficer = i.company_liaison_officer_name;
                    p.WillingToSponsor = i.company_sponsorship_willingness;
                    p.SubmittedDate = i.submitted_on;
                    p.Availability = i.project_availability;

                    proposalItems.Add(p);
                }
                return proposalItems;
            }
        }

        public IEnumerable<ProposalViewModel> Get(Lecturer lecturer, int saved = 0)
        {

            int lecturerId = lecturer.lecturer_id;
            var projects = new object();
            using (var connection = GetConnection())
            {
                projects = connection.Query("Select * from Industry_Lecturer_Project Inner Join Project on Industry_Lecturer_Project.project_id = Project.project_id where course_id = @courseId", new { courseId = lecturer.course_id });

                List<ProposalViewModel> proposalItems = new List<ProposalViewModel>();
                foreach (var i in projects as IEnumerable<dynamic>)
                {
                    ProposalViewModel p = new ProposalViewModel();
                    p.Id = i.project_id;
                    p.Title = i.project_title;
                    p.Aims = i.project_aims;
                    p.Objectives = i.project_objectives;
                    p.Deadline = i.deadline.ToString("dd/MM/yyyy");
                    p.TargetAudience = i.target_audience;
                    p.Schedule = i.project_milestones;
                    p.MainFunction = i.main_functions_and_deliverables;
                    p.HardwareAndSoftwareConfiguration = i.hardware_and_software_requirements;
                    p.CompanyName = i.company_name;
                    p.Address = i.company_postal_address;
                    p.Tel = i.company_telephone_number;
                    p.Fax = i.company_fax_number;
                    p.Email = i.company_email_address;
                    p.LiaisonOfficer = i.company_liaison_officer_name;
                    p.WillingToSponsor = i.company_sponsorship_willingness;
                    p.SubmittedDate = i.submitted_on;
                    p.Availability = i.project_availability;

                    proposalItems.Add(p);
                }

                return proposalItems;
            }
        }

        public dynamic GetProposalById(int Id)
        {
            using (var connection = GetConnection())
            {

                var i = connection.Query("Select * from Industry_Lecturer_Project Inner Join Project on Industry_Lecturer_Project.project_id = Project.project_id where Project.project_id = @Id", new { Id }).SingleOrDefault();
                if (i != null)
                {
                    ProposalViewModel p = new ProposalViewModel();
                    p.Id = i.project_id;
                    p.LecturerId = i.lecturer_id;
                    p.Title = i.project_title;
                    p.Aims = i.project_aims;
                    p.Objectives = i.project_objectives;
                    p.Deadline = i.deadline.ToString("dd/MM/yyyy");
                    p.TargetAudience = i.target_audience;
                    p.MainFunction = i.main_functions_and_deliverables;
                    p.HardwareAndSoftwareConfiguration = i.hardware_and_software_requirements;
                    p.Schedule = i.project_milestones;
                    p.CompanyName = i.company_name;
                    p.Address = i.company_postal_address;
                    p.Tel = i.company_telephone_number;
                    p.Fax = i.company_fax_number;
                    p.Email = i.company_email_address;
                    p.LiaisonOfficer = i.company_liaison_officer_name;
                    p.WillingToSponsor = i.company_sponsorship_willingness;
                    p.SubmittedDate = i.submitted_on;
                    p.Availability = i.project_availability;
                    p.ProjectType = "External";
                    return p;
                }
                else
                {
                    ProjectRepository projectRepository = new ProjectRepository();
                    InitiatedProjectViewModel p = projectRepository.GetProject(Id);
                    p.ProjectType = "Student";
                    return p;
                }
            }
        }
        
        public void AddProposal(ProposalViewModel proposal, int lecturerId, int courseId)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inLecturerId", lecturerId, dbType: DbType.Int32);
                p.Add("@inCourseId", courseId, dbType: DbType.Int32);
                p.Add("@inCompanyName", proposal.CompanyName, dbType: DbType.String, size: 50);
                p.Add("@inEmail", proposal.Email, dbType: DbType.String, size: 50);
                p.Add("@inAddress", proposal.Address, dbType: DbType.String, size: 100);
                p.Add("@inTel", proposal.Tel, dbType: DbType.StringFixedLength, size: 8);
                p.Add("@inFax", proposal.Fax, dbType: DbType.StringFixedLength, size: 8);
                p.Add("@inLiaisonOfficer", proposal.LiaisonOfficer, dbType: DbType.String, size: 50);
                p.Add("@inWillingToSponsor", proposal.WillingToSponsor, dbType: DbType.Boolean);
                p.Add("@inProjectTitle", proposal.Title, dbType: DbType.String, size: 50);
                p.Add("@inProjectAims", proposal.Aims, dbType: DbType.String, size: 500);
                p.Add("@inProjectObjectives", proposal.Objectives, dbType: DbType.String, size: 500);
                p.Add("@inTargetAudience", proposal.TargetAudience, dbType: DbType.String, size: 500);
                p.Add("@inMainFunction", proposal.MainFunction, dbType: DbType.String, size: 500);
                p.Add("@inHardwareAndSoftwareRequirements", proposal.HardwareAndSoftwareConfiguration, dbType: DbType.String, size: 500);
				p.Add("@inDeadline", DateTime.Parse(proposal.Schedule).AddMonths(3).ToString(), dbType: DbType.String, size: 500);
                
                p.Add("@inSchedule", proposal.Schedule, dbType: DbType.String, size: 500);

                DateTime dt = DateTime.Now;
                string r = proposal.ReadyForApproval.ToSelectList().SelectedValue.ToString();
                switch (r)
                {
                    case "0":
                        dt = dt.AddDays(3);
                        break;
                    case "1":
                        dt = dt.AddDays(7);
                        break;
                    case "2":
                        dt = dt.AddDays(10);
                        break;

                }
                p.Add("@inDeadline", dt, dbType: DbType.DateTime);


                p.Add("@inSubmittedOn", proposal.SubmittedDate, dbType: DbType.DateTime);
                connection.Execute("usp_addExternalProject", p, commandType: CommandType.StoredProcedure);
                
            }
        }
      

        public void ApproveProposal(int Id)
        {
            using (var connection = GetConnection())
            {
                connection.Execute("Update Project Set project_availability = 1 where project_id = @id", new { date = DateTime.Now, id = Id });
            }
        }

        public void DeleteProposal(int Id)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inProposalId", Id, dbType: DbType.Int32);
                connection.Execute("usp_deleteProposal", p, commandType: CommandType.StoredProcedure);
            }
        }
        public void DeleteProposals(List<int> Ids)
        {
            using (var connection = GetConnection())
            {

                string inClauseParam = "(" + String.Join(",", Ids.ToArray()) + ")";
                connection.Execute("Delete From Project_Choice where project_id In " + inClauseParam);
                connection.Execute("Delete From Industry_Lecturer_Project where project_id In " + inClauseParam);
                connection.Execute("Delete From Project where project_id In " + inClauseParam);
            }
        }

        public void SubmitProposal(int Id)
        {
            throw new NotImplementedException();
        }
    }
}