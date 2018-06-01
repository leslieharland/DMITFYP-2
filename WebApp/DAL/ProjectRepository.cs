using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.ViewModels.InitiatedProject;

namespace WebApp.DAL
{
    public class ProjectRepository : IProjectRepository
    {
		private readonly IConfiguration configuration;
        public String connectionString;
		public ProjectRepository(){

		}
		public ProjectRepository(IConfiguration config)
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
        public IEnumerable<Project> Get()
        {
            {
                using (var connection = GetConnection())
				{
					return connection.Query<Project>("SELECT * FROM Project");
					//return connection.Query<Project>("SELECT * FROM Project Where sip = 0");
                   
                }
            }
        }

        public Project GetProjectById(int Id) { return new Project(); }
        public void AddProject(Project project) { }

        public void AddSelfInitiatedProject(InitiatedProjectViewModel project, int groupId)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inGroupId", groupId, DbType.Int32);

                p.Add("@inProjectOverview", project.ProjectOverview, dbType: DbType.String);
                p.Add("@inIntroBackground", project.IntroBackground, dbType: DbType.String);
                p.Add("@inKeyInnovation", project.KeyInnovationAndResearchGoals, dbType: DbType.String);
                p.Add("@inComparison", project.ComparisonOfTheMerits, dbType: DbType.String);
                p.Add("@inBusinessModel", project.BusinessModelAndMarketPotential, dbType: DbType.String);
                p.Add("@inProjectPlan", project.ProjectPlan, dbType: DbType.String);
                p.Add("@inProblemsAndCounterMeasure", project.ProblemsAndCountermeasures, dbType: DbType.String);

                p.Add("@inTargetAudience", project.TargetAudience, dbType: DbType.String);
                p.Add("@inMainFunction", project.MainFunction, dbType: DbType.String);
                p.Add("@inHardwareAndSoftwareRequirements", project.HardwareAndSoftwareRequirements, dbType: DbType.String);
                p.Add("@inProjectTitle", project.Title, dbType: DbType.String);

                connection.Execute("usp_addSelfInitiatedProject", p, commandType: CommandType.StoredProcedure);
            }
        }
       
        public IEnumerable<Project> GetAvailableProjects(int courseId)
        {
            try
            {
                using (var connection = GetConnection())
				{
					var projects = connection.Query<Project>("SELECT * FROM Project WHERE (course_id = @courseId AND project_availability = 1)", new { courseId });
                    return projects;
                }
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<Project> GetProjects(int courseId)
        {
            try
            {
                using (var connection = GetConnection())
				{
					var projects = connection.Query<Project>("SELECT * FROM Project WHERE course_id = @courseId", new { courseId });
                    return projects;
                }
            }
            catch
            {
                throw;
            }
        }

	

        public void AddSelfInitiatedProject(InitiatedProjectViewModel project, int groupId, int courseId)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inGroupId", groupId, DbType.Int32);
                p.Add("@inCourseId", courseId, DbType.Int32);

                p.Add("@inProjectOverview", project.ProjectOverview, dbType: DbType.String);
                p.Add("@inIntroBackground", project.IntroBackground, dbType: DbType.String);
                p.Add("@inKeyInnovation", project.KeyInnovationAndResearchGoals, dbType: DbType.String);
                p.Add("@inComparison", project.ComparisonOfTheMerits, dbType: DbType.String);
                p.Add("@inBusinessModel", project.BusinessModelAndMarketPotential, dbType: DbType.String);
                p.Add("@inProjectPlan", project.ProjectPlan, dbType: DbType.String);
                p.Add("@inProblemsAndCounterMeasure", project.ProblemsAndCountermeasures, dbType: DbType.String);

                p.Add("@inTargetAudience", project.TargetAudience, dbType: DbType.String);
                p.Add("@inMainFunction", project.MainFunction, dbType: DbType.String);
                p.Add("@inHardwareAndSoftwareRequirements", project.HardwareAndSoftwareRequirements, dbType: DbType.String);
                p.Add("@inProjectTitle", project.Title, dbType: DbType.String);
                p.Add("@inSavedDate", project.SavedDate, dbType: DbType.DateTime);
                p.Add("@inSubmittedOn", project.SubmittedDate, dbType: DbType.DateTime);
                p.Add("@outProjectId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("usp_addSelfInitiatedProject", p, commandType: CommandType.StoredProcedure);

                int projectId = p.Get<int>("@outProjectId");
                //var model = JsonConvert.DeserializeObject<List<Field>>(project.displayFields);

                //string s1 = fieldValues[0].ToString();
                //if (labels.Count() > 0)
                //{
                //    for (int i = 0; i < labels.Count(); i++)
                //        connection.Execute("Insert Into Student_Project_Extra(field_name, text_value, project_id) values(@fieldName, @textValue, @projectId)", new { fieldName = labels[i].ToString(), textValue = fieldValues[i].ToString(), projectId });
                //}   

                if (project.displayFields != null)
                {
                    var model = JsonConvert.DeserializeObject<List<Field>>(project.displayFields);
                    foreach (Field f in model)
                    {
                        connection.Execute("Insert Into Student_Project_Extra(field_name, text_value, project_id) values(@fieldName, @textValue, @projectId)"
                            , new { fieldName = f.label, textValue = f.value, projectId });
                    }
                }
            }
        }

        public void AddProjectHistory(int Id)
        {
            using (var connection = GetConnection())
            {
                connection.Execute("Insert into Student_Project_History (project_id) values(@id)", new { id = Id });
            }
        }

        public bool IsProjectHistory(int Id)
        {
            using (var connection = GetConnection())
            {
                return connection.Query("Select response_id from Student_Project_History where project_id = @id", new { id = Id }).Count() > 0;
            }
        }
        public void DeleteProject(int Id)
        {
            using (var connection = GetConnection())
            {
                connection.Execute("Delete from Student_Project_Extra where project_id = @id", new { id = Id });
                connection.Execute("Delete Student_Project where project_id = @id", new { id = Id });
                connection.Execute("Delete Project where project_id = @id", new { id = Id });
            }
        }

   

        public InitiatedProjectViewModel GetProject(int Id, int groupId = 0)
        {
            using (var connection = GetConnection())
            {
                string command = "Select * from Student_Project Inner Join Project on Student_Project.project_id = Project.project_id where Project.project_id = @Id";
                if (groupId != 0)
                {
                    command += " and group_id = @groupId";
                }
                string query = @"Select * from Student_Project Inner Join Project on Student_Project.project_id = Project.project_id where Project.project_id = @Id
                    Select * from Student_Project_Extra where project_id = @Id";

                var p = new DynamicParameters();
                p.Add("Id", Id);
                p.Add("groupId", groupId);

                using (var multi = connection.QueryMultiple(query, p))
                {
                    var project = multi.Read<dynamic>().Single();
                    var fields = multi.Read<dynamic>();
                    InitiatedProjectViewModel model = new InitiatedProjectViewModel();
                    if (project != null)
                    {
                        model.GroupId = project.group_id;
                        model.Id = project.project_id;
                        model.Title = project.project_title;
                        model.ProjectOverview = project.project_overview;
                        model.IntroBackground = project.project_background;
                        model.KeyInnovationAndResearchGoals = project.strategy_and_approach;
                        model.ComparisonOfTheMerits = project.merits_comparison;
                        model.TargetAudience = project.target_audience;
                        model.BusinessModelAndMarketPotential = project.commercialisation_strategy;
                        model.MainFunction = project.main_functions_and_deliverables;
                        model.ProjectPlan = project.project_milestones_and_workload_allocation;
                        model.HardwareAndSoftwareRequirements = project.hardware_and_software_requirements;
                        model.ProblemsAndCountermeasures = project.problems_and_countermeasures;
                        model.SavedDate = project.updated_at;
                        model.SubmittedDate = project.submitted_on;
                        model.Availability = project.project_availability;
                        List<Field> dplFields = new List<Field>();
                        foreach (var i in fields)
                        {
                            Field f = new Field();
                            f.id = i.field_id;
                            f.label = i.field_name;
                            f.value = i.text_value;
                            dplFields.Add(f);
                        }
                        model.displayFieldsModel = dplFields;
                        return model;
                    }
                }
                return null;
            }
        }

        public InitiatedProjectViewModel GetProjectByGroup(int groupId)
        {
            //We allow only one submission with history
            InitiatedProjectViewModel model = new InitiatedProjectViewModel();
            using (var connection = GetConnection())
            {
                var project = connection.Query("Select TOP 1 * from Student_Project Inner Join Project on Student_Project.project_id = Project.project_id where group_id = 8 ORDER BY submitted_on DESC", new { groupId = groupId }).SingleOrDefault();
                if (project != null)
                {
                    model.Id = project.project_id;
                    model.Title = project.project_title;
                    model.ProjectOverview = project.project_overview;
                    model.IntroBackground = project.project_background;
                    model.KeyInnovationAndResearchGoals = project.strategy_and_approach;
                    model.ComparisonOfTheMerits = project.merits_comparison;
                    model.TargetAudience = project.target_audience;
                    model.BusinessModelAndMarketPotential = project.commercialisation_strategy;
                    model.MainFunction = project.main_functions_and_deliverables;
                    model.ProjectPlan = project.project_milestones_and_workload_allocation;
                    model.HardwareAndSoftwareRequirements = project.hardware_and_software_requirements;
                    model.ProblemsAndCountermeasures = project.problems_and_countermeasures;
                    model.SavedDate = project.updated_at;
                    model.SubmittedDate = project.submitted_on;
                    model.Availability = project.project_availability;
                    return model;
                }
                return null;
            }
        }

        public int SetLecturerId(int projectId, int? lecturerId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("UPDATE Industry_Lecturer_Project SET lecturer_id = @lecturerId WHERE project_id = @projectId", new { lecturerId, projectId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<dynamic> GetIndustryLecturerProjectsByLecturerId(int lecturerId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var industryLecturerProjects = connection.Query<dynamic>("getIndustryLecturerProjectsByLecturerSP", new { lecturerId }, commandType: CommandType.StoredProcedure);
                    return industryLecturerProjects;
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<dynamic> GetIndustryLecturerProjects(int courseId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var industryLecturerProjects = conn.Query<dynamic>("getIndustryLecturerProjectsSP", new { courseId }, commandType: CommandType.StoredProcedure);
                    return industryLecturerProjects;
                }
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<dynamic> GetSelfInitiatedProjects(int courseId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var selfInitiatedProjects = conn.Query<dynamic>("getStudentProjectsSP", new { courseId }, commandType: CommandType.StoredProcedure);
                    return selfInitiatedProjects;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<List<string>> GetAvailableProjectsSpreadsheetData(int courseId)
        {
            List<string> rowData = new List<string>();
            List<List<string>> columnData = new List<List<string>>();
            int iterationCount = 0;
            try
            {
                var availableProjects = GetAvailableProjects(courseId);
                foreach (Project p in availableProjects)
                {
                    ++iterationCount;
                    rowData = new List<string>();
                    rowData.Add(iterationCount.ToString());
                    rowData.Add(p.project_title);
                    rowData.Add(p.project_aims);
                    rowData.Add(p.project_objectives);
                    rowData.Add(p.main_functions_and_deliverables);
                    rowData.Add(p.hardware_and_software_requirements);
                    columnData.Add(rowData);
                }
                return columnData;
            }
            catch
            {
                throw;
            }
        }

        public List<List<string>> GetIndustryLecturerProjectsSpreadsheetData(int courseId)
        {
            List<string> rowData = new List<string>();
            List<List<string>> columnData = new List<List<string>>();
            int iterationCount = 0;
            try
            {
                var industryLecturerProjects = GetIndustryLecturerProjects(courseId);
                foreach (dynamic d in industryLecturerProjects)
                {
                    ++iterationCount;
                    rowData = new List<string>();
                    rowData.Add(iterationCount.ToString());
                    rowData.Add(d.company_name);
                    rowData.Add(d.project_title);
                    rowData.Add(d.project_aims);
                    rowData.Add(d.hardware_and_software_requirements);
                    columnData.Add(rowData);
                }
                return columnData;
            }
            catch
            {
                throw;
            }
        }

        public List<List<string>> GetCombinedProjectsSpreadsheetData(int courseId)
        {
            List<string> rowData = new List<string>();
            List<List<string>> columnData = new List<List<string>>();
            int iterationCount = 0;
            try
            {
                var selfInitiatedProjects = GetSelfInitiatedProjects(courseId);
                var industryLecturerProjects = GetIndustryLecturerProjects(courseId);

                foreach (dynamic d in selfInitiatedProjects)
                {
                    ++iterationCount;
                    rowData = new List<string>();
                    rowData.Add(iterationCount.ToString());
                    rowData.Add(null);
                    rowData.Add(d.project_background);
                    rowData.Add(d.project_title);
                    rowData.Add(d.project_aims);
                    rowData.Add(d.hardware_and_software_requirements);
                    columnData.Add(rowData);
                }

                foreach (dynamic d in industryLecturerProjects)
                {
                    ++iterationCount;
                    rowData = new List<string>();
                    rowData.Add(iterationCount.ToString());
                    rowData.Add(d.company_name);
                    rowData.Add(null);
                    rowData.Add(d.project_title);
                    rowData.Add(d.project_aims);
                    rowData.Add(d.hardware_and_software_requirements);
                    columnData.Add(rowData);
                }
                return columnData;
            }
            catch
            {
                throw;
            }
        }

        public List<List<string>> GetSelfInitiatedProjectsSpreadsheetData(int courseId)
        {
            List<string> rowData = new List<string>();
            List<List<string>> columnData = new List<List<string>>();
            int iterationCount = 0;
            try
            {
                var selfInitiatedProjects = GetSelfInitiatedProjects(courseId);
                foreach (dynamic d in selfInitiatedProjects)
                {
                    ++iterationCount;
                    rowData = new List<string>();
                    rowData.Add(iterationCount.ToString());
                    rowData.Add(d.project_background);
                    rowData.Add(d.project_title);
                    rowData.Add(d.project_aims);
                    rowData.Add(d.hardware_and_software_requirements);
                    columnData.Add(rowData);
                }
                return columnData;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateProject(InitiatedProjectViewModel project)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inProjectId", project.Id, dbType: DbType.Int32);
                p.Add("@inTitle", project.Title, dbType: DbType.String, size: 50);
                p.Add("@inOverview", project.ProjectOverview, dbType: DbType.String, size: 500);
                p.Add("@inBackground", project.IntroBackground, dbType: DbType.String, size: 500);
                p.Add("@inKeyInnovationAndResearchGoal", project.TargetAudience, dbType: DbType.String, size: 500);
                p.Add("@inComparisonOfTheMerits", project.ComparisonOfTheMerits, dbType: DbType.String, size: 500);
                p.Add("@inTargetAudience", project.TargetAudience, dbType: DbType.String, size: 500);
                p.Add("@inBusinessModelAndMarketPotential", project.BusinessModelAndMarketPotential, dbType: DbType.String, size: 500);
                p.Add("@inMainFunction", project.MainFunction, dbType: DbType.String, size: 500);
                p.Add("@inProjectPlan", project.ProjectPlan, dbType: DbType.String, size: 500);
                p.Add("@inHardwareAndSoftwareRequirements", project.HardwareAndSoftwareRequirements, dbType: DbType.String, size: 500);
                p.Add("@inProblemsAndCountermeasures", project.ProblemsAndCountermeasures, dbType: DbType.String, size: 500);
                p.Add("@inSavedDate", project.SavedDate, dbType: DbType.DateTime);
                p.Add("@inSubmittedDate", project.SubmittedDate, dbType: DbType.DateTime);

                connection.Execute("usp_updateStudentProject", p, commandType: CommandType.StoredProcedure);

                var model = JsonConvert.DeserializeObject<List<Field>>(project.displayFields);
                foreach (Field f in model)
                {
                    if (f.id != 0)
                    {
                        connection.Execute("Update Student_Project_Extra Set field_name = @fieldName, text_value = @textValue where field_id = @fieldId"
                        , new { fieldName = f.label, textValue = f.value, fieldId = f.id });
                        //connection.Execute("MERGE Student_Project_Extra AS target " +
                        //                "USING(SELECT @fieldId, @fieldName, @fieldValue, @projectId) AS source(field_id, field_name, text_value, project_id) " +
                        //                "ON(target.field_id = source.field_id) " +
                        //                "WHEN MATCHED THEN " +
                        //                "UPDATE SET field_name = source.field_name, text_value = source.text_value " +
                        //                "WHEN NOT MATCHED THEN " +
                        //                "INSERT(field_name, text_value, project_id) " +
                        //                "VALUES(source.field_name, source.text_value, source.project_id); "
                        //                , new { fieldId = f.id, fieldName = f.label, fieldValue = f.value, projectId = project.Id });
                    }
                    else
                    {
                        connection.Execute("Insert Into Student_Project_Extra(field_name, text_value, project_id) values(@fieldName, @textValue, @projectId)"
                       , new { fieldName = f.label, textValue = f.value, projectId = project.Id });
                    }

                }
            }
        }

        public void DeleteProjectFields(InitiatedProjectViewModel project)
        {
            if (project.removeIds != null)
            {
                string inClauseParam = "(" + project.removeIds + ")";
                using (var connection = GetConnection())
                {
                    connection.Execute("Delete From Student_Project_Extra where field_id In " + inClauseParam);
                }
            }
        }         
	}
}

