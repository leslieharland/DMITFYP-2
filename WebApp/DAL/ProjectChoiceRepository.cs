using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using WebApp.Models;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using WebApp.Formatters;

namespace WebApp.DAL
{
    public class ProjectChoiceRepository : IProjectChoiceRepository
    {
		private readonly IConfiguration configuration;
        public String connectionString;
		public ProjectChoiceRepository(){

		}
		public ProjectChoiceRepository(IConfiguration config)
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
		public bool CheckProjectChoiceWindowPeriod(int courseId, int semester)
		{
			DateTime currentDate = DateTime.Now;
            XDocument xDocument = XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            DateTime startDate = DateTime.Parse(XmlReader.GetNodeValue(ref xDocument, "/PersistentData/Course[CourseId=" + courseId + "]/ProjectChoiceWindowPeriod/Semester" + semester + "/StartDate"));
            DateTime endDate = DateTime.Parse(XmlReader.GetNodeValue(ref xDocument, "/PersistentData/Course[CourseId=" + courseId + "]/ProjectChoiceWindowPeriod/Semester" + semester + "/EndDate"));
            return (currentDate > startDate && currentDate < endDate);
		}

		public int CreateProjectChoice(ProjectChoice projectChoice)
		{
			try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@" + ProjectChoice.StudentIdDatabaseColumnName, value: projectChoice.student_id, dbType: DbType.Int32);
                    p.Add("@" + ProjectChoice.ProjectIdDatabaseColumnName, value: projectChoice.project_id, dbType: DbType.Int32);
                    p.Add("@" + ProjectChoice.RankingPrecedenceDatabaseColumnName, value: projectChoice.ranking_precedence, dbType: DbType.Int32);
                    p.Add("@" + ProjectChoice.SubmittedDateDatabaseColumnName, value: projectChoice.submitted_date, dbType: DbType.DateTime);
                    var numberOfRowsAffected = connection.Execute("createProjectChoiceSP", p, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
		}

		public int DeleteProjectChoicesForStudent(int studentId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var numberOfRowsAffected = conn.Execute("DELETE FROM " + ProjectChoice.ProjectChoiceDatabaseTableName + " WHERE " + ProjectChoice.StudentIdDatabaseColumnName + " = @studentId", new { studentId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ProjectChoice> GetProjectChoicesByStudentId(int studentId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var projectChoices = conn.Query<ProjectChoice>("SELECT * FROM " + ProjectChoice.ProjectChoiceDatabaseTableName + " WHERE " + ProjectChoice.StudentIdDatabaseColumnName + " = @studentId", new { studentId });
                    return projectChoices;
                }
            }
            catch
            {
                throw;
            }
        }

		public List<string> GetProjectChoiceWindowPeriod(int courseId, int semester)
		{
			List<string> projectChoiceWindowPeriod = new List<string>();
            XDocument xDocument = XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            string projectChoiceWindowPeriodStartDate = XmlReader.GetNodeValue(ref xDocument, "/PersistentData/Course[CourseId=" + courseId + "]/ProjectChoiceWindowPeriod/Semester" + semester + "/StartDate").Replace("-", "/").Substring(0, 16);
            string projectChoiceWindowPeriodEndDate = XmlReader.GetNodeValue(ref xDocument, "/PersistentData/Course[CourseId=" + courseId + "]/ProjectChoiceWindowPeriod/Semester" + semester + "/EndDate").Replace("-", "/").Substring(0, 16);
            projectChoiceWindowPeriod.Add(projectChoiceWindowPeriodStartDate);
            projectChoiceWindowPeriod.Add(projectChoiceWindowPeriodEndDate);
            return projectChoiceWindowPeriod;
		}

		public IEnumerable<ProjectChoice> GetSortedProjectChoicesByStudentId(int studentId)
		{
			try
            {
                using (var connection = GetConnection())
                {
                    var projectChoices = connection.Query<ProjectChoice>("SELECT * FROM " + ProjectChoice.ProjectChoiceDatabaseTableName + " WHERE " + ProjectChoice.StudentIdDatabaseColumnName + " = @studentId ORDER BY " + ProjectChoice.RankingPrecedenceDatabaseColumnName + " ASC", new { studentId });
                    return projectChoices;
                }
            }
            catch
            {
                throw;
            }
		}

		public void SetProjectChoiceWindowPeriod(int courseId, int semesterId, string startDate, string endDate)
		{
			XDocument xDocument = XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            XmlFormatter.UpdateNodeValue(ref xDocument, "/PersistentData/Course[CourseId=" + courseId + "]/ProjectChoiceWindowPeriod/Semester" + semesterId + "/StartDate", startDate.Replace("/", "-") + ":00");
			XmlFormatter.UpdateNodeValue(ref xDocument, "/PersistentData/Course[CourseId=" + courseId + "]/ProjectChoiceWindowPeriod/Semester" + semesterId + "/EndDate", endDate.Replace("/", "-") + ":00");
            XmlFormatter.FinalizeXmlWriting(ref xDocument, Constants.PersistentDataXmlVirtualFilePath);
		}
	}
}