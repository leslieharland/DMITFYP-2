using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using WebApp.Models;
using WebApp.Formatters;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace WebApp.DAL
{
    public class GroupRepository: IGroupRepository
    {

		private readonly IConfiguration configuration;
        public String connectionString;
		public GroupRepository(){

		}
		public GroupRepository(IConfiguration config)
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
        public IEnumerable<Lecturer> Get()
        {
            using (var connection = GetConnection())
            {
                IEnumerable<Lecturer> lecturers = connection.Query<Lecturer>("Select * from Lecturer");
                return lecturers;
            }
        }      
      
        public List<List<string>> GetGroupProjectSelectionSpreadsheetData()
        {
            int courseId = 1; // Assume that this course id was retrieved from the session
            IStudentRepository studentRepository = new StudentRepository();
            IProjectChoiceRepository projectChoiceRepository = new ProjectChoiceRepository();
            IEnumerable<ProjectChoice> projectChoices = new List<ProjectChoice>();
            List<string> studentProjectChoices = new List<string>();
            List<string> temporaryList = new List<string>();
            List<List<string>> allData = new List<List<string>>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var students = studentRepository.GetStudents(courseId);
                    foreach (Student s in students)
                    {
                        if (s.group_id == null && s.group_role == null) continue;
                        var group = GetGroup((int)s.group_id);
                        studentProjectChoices = new List<string>();
                        if (s.group_role == "1")
                        {
                            projectChoices = projectChoiceRepository.GetProjectChoicesByStudentId(s.student_id);
                            foreach (ProjectChoice pc in projectChoices)
                            {
                                studentProjectChoices.Add(pc.ranking_precedence.ToString());
                            }
                        } else {
                            studentProjectChoices.AddRange(new string[]{"", "", "", "" });
                        }
                        temporaryList = new List<string>();
                        temporaryList.Add(group.GetType().GetProperty("groupNumber").GetValue(group, null).ToString());
                        temporaryList.Add(s.admin_number);
                        temporaryList.Add(s.full_name);
                        temporaryList.Add(s.mobile_number);
                        temporaryList.Add(s.email_address);
                        temporaryList.AddRange(studentProjectChoices);
                        allData.Add(temporaryList);
                    }
                    return allData;
                }
            }
            catch
            {
                throw;
            }
        }
		public int CreateGroup(Group group)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                p.Add("@" + Group.GroupNumberDatabaseColumnName, value: group.group_number, dbType: DbType.String);
                p.Add("@" + Group.ValidDatabaseColumnName, value: group.valid, dbType: DbType.Boolean);
                p.Add("@" + Group.LecturerIdDatabaseColumnName, value: group.lecturer_id, dbType: DbType.Int32);
                p.Add("@" + Group.ProjectIdDatabaseColumnName, value: group.project_id, dbType: DbType.Int32);
                p.Add("@" + Group.CourseIdDatabaseColumnName, value: group.course_id, dbType: DbType.Int32);

                var numberOfRowsAffected = connection.Execute("createGroupSP", p, commandType: CommandType.StoredProcedure);
                return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }
        public int ChangeGroupRole(int studentId, string groupRole)
        {
            int numberOfRowsAffected;

            try
            {
                numberOfRowsAffected = new StudentRepository().SetGroupRole(studentId, groupRole);
            }
            catch (Exception e)
            {
                throw;
            }
            return numberOfRowsAffected;
        }
        public string GetProspectiveGroupNumber(int courseId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@" + Group.CourseIdDatabaseColumnName, value: courseId, dbType: DbType.Int32);
                    var prospectiveGroupNumber = connection.Query<string>("getProspectiveGroupNumberSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    return prospectiveGroupNumber;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckGroupFormationWindowPeriod(int courseId)
        {
            XDocument xDocument = XmlReader.ReadXmlDocument(Constants.PersistentDataXmlVirtualFilePath);
            XmlReader.GetNodeValue(ref xDocument, "");
            return false;
        }

        public List<object> GetGroupsByLecturerId(int lecturerId)
        {
            List<object> listOfGroups = new List<object>();
            try
            {
                using (var connection = GetConnection())
                {
                    var groups = connection.Query<Group>("SELECT * FROM " + Group.GroupDatabaseTableName + " WHERE " + Group.LecturerIdDatabaseColumnName + " = @lecturerId", new { lecturerId });

                    foreach (Group group in groups)
                    {
                        listOfGroups.Add(GetGroup(group.group_id));
                    }
                    return listOfGroups;
                }
            }
            catch
            {
                throw;
            }
        }

        public object GetGroup(int groupId)
        {
            using (var connection = GetConnection())
            {
                var group = connection.Query<Group>("SELECT * FROM " + Group.GroupDatabaseTableName + " WHERE " + Group.GroupIdDatabaseColumnName + " = @groupId", new { groupId }).SingleOrDefault();
                if (group == null)
                {
                    return new
                    {
                        groupId = (Type)null,
                        groupNumber = (Type)null,
                        valid = (Type)null,
                        lecturerId = (Type)null,
                        projectId = (Type)null,
                        courseId = (Type)null,
                        groupMembers = (Type)null
                    };
                }
                return new
                {
                    groupId = group.group_id,
                    groupNumber = group.group_number,
                    valid = group.valid,
                    lecturerId = group.lecturer_id,
                    projectId = group.project_id,
                    courseId = group.course_id,
                    groupMembers = new StudentRepository().GetStudentsByGroupId(group.group_id)
                                         
                };
            }
        }
        public IEnumerable<GroupWrapper> GetGroups(int courseId)
        {
            List<GroupWrapper> groupsToReturn = new List<GroupWrapper>();
            try
            {
                using (var connection = GetConnection())
                {
                    var groups = connection.Query<Group>("SELECT * FROM " + Group.GroupDatabaseTableName + " WHERE " + Group.CourseIdDatabaseColumnName + " = @courseId", new { courseId });
                    foreach (Group group in groups)
                    {
                        groupsToReturn.Add(new GroupWrapper()
                        {
                            groupId = group.group_id,
                            groupNumber = group.group_number,
                            valid = group.valid,
                            lecturerId = group.lecturer_id,
                            projectId = group.project_id,
                            courseId = group.course_id,
                            groupMembers = (List<Student>)new StudentRepository().GetStudentsByGroupId(group.group_id)
                        });
                    }
                    return groupsToReturn;
                }
            }
            catch
            {
                throw;
            }
        }
        public object GetGroup(int groupNumber, int courseId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@" + Group.CourseIdDatabaseColumnName, value: courseId, dbType: DbType.Int32);
                    p.Add("@group_no", value: groupNumber, dbType: DbType.Int32);

                    var group = connection.Query<Group>("getGroupSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    if (group == null)
                    {
                        return new
                        {
                            groupId = (Type)null,
                            groupNumber = (Type)null,
                            valid = (Type)null,
                            lecturerId = (Type)null,
                            projectId = (Type)null,
                            courseId = (Type)null,
                            groupMembers = (Type)null
                        };
                    }
                    return new
                    {
                        groupId = group.group_id,
                        groupNumber = group.group_number,
                        valid = group.valid,
                        lecturerId = group.lecturer_id,
                        projectId = group.project_id,
                        courseId = group.course_id,
                        groupMembers = new StudentRepository().GetStudentsByGroupId(group.group_id)
                    };
            }
            }
            catch
            {
                throw;
            }
        }
        public int DeleteGroup(int groupId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("DELETE FROM " + Group.GroupDatabaseTableName + " WHERE " + Group.GroupIdDatabaseColumnName + " = @groupId", new { groupId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }
        public int AssignSupervisor(int? lecturerId, int groupId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@" + Group.GroupIdDatabaseColumnName, value: groupId, dbType: DbType.Int32);
                    p.Add("@" + Group.LecturerIdDatabaseColumnName, value: lecturerId, dbType: DbType.Int32);

                    var numberOfRowsAffected = connection.Execute("setGroupLecturerIdSP", p, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }
        public int AssignProject(int? projectId, int groupId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@" + Group.GroupIdDatabaseColumnName, value: groupId, dbType: DbType.Int32);
                    p.Add("@" + Group.ProjectIdDatabaseColumnName, value: projectId, dbType: DbType.Int32);

                    var numberOfRowsAffected = connection.Execute("setGroupProjectIdSP", p, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckStudentAlreadyHaveGroup(int studentId)
        {
            return (new StudentRepository().GetStudentByStudentId(studentId).group_role != null);
        }
        public int AddGroupMember(int studentId, int groupId)
        {
            if (CheckStudentAlreadyHaveGroup(studentId)) throw new Exception("The student(s) already belong to a group.");

            int numberOfRowsAffected;

            try
            {
                numberOfRowsAffected = new StudentRepository().SetGroupRoleAndGroupId(studentId, CheckGroupEmpty(groupId) ? "1" : "3", groupId);
            }
            catch (Exception e)
            {
                throw;
            }

            return numberOfRowsAffected;
        }
        public int DeleteGroupMember(int studentId)
        {
            int numberOfRowsAffected;

            try
            {
                numberOfRowsAffected = new StudentRepository().SetGroupRoleAndGroupId(studentId, null, null);
            }
            catch (Exception e)
            {
                throw;
            }

            return numberOfRowsAffected;
        }

        public bool CheckGroupEmpty(int groupId)
        {
            return (new StudentRepository().GetStudentsByGroupId(groupId).Count() == 0);
        }

        public int GetNumberOfGroups(int courseId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfGroups = connection.Query<int>("SELECT COUNT(*) FROM " + Group.GroupDatabaseTableName + " WHERE " + Group.CourseIdDatabaseColumnName + " = @courseId", new { courseId }).FirstOrDefault();
                    return numberOfGroups;
                }
            }
            catch
            {
                throw;
            }
        }
        public List<List<string>> GetGroupProjectSelectionSpreadsheetData(int courseId)
        {
			IProjectChoiceRepository projectChoiceRepository = new ProjectChoiceRepository();
            IEnumerable<ProjectChoice> projectChoices = new List<ProjectChoice>();
            List<string> studentProjectChoices = new List<string>();
            List<string> rowData = new List<string>();
            List<List<string>> columnData = new List<List<string>>();
			int totalNumberOfProjects = new ProjectRepository().GetAvailableProjects(courseId).Count();
            try
            {
                using (var connection = GetConnection())
                {
                    var students = new StudentRepository().GetGroupSortedStudents(courseId);
                    foreach (Student s in students)
                    {
                        if (s.group_id == null && s.group_role == null) continue;
                        var group = GetGroup((int)s.group_id);
                        studentProjectChoices = new List<string>();
                        if (s.group_role == "1")
                        {
                            projectChoices = projectChoiceRepository.GetProjectChoicesByStudentId(s.student_id);
                            foreach (ProjectChoice pc in projectChoices)
                            {
                                studentProjectChoices.Add(pc.ranking_precedence.ToString());
                            }
                        } else {
                            for (int i = 0; i < totalNumberOfProjects; ++i)
                            {
                                studentProjectChoices.Add("");
                            }
                        }
                        rowData = new List<string>();
                        rowData.Add(group.GetType().GetProperty("groupNumber").GetValue(group, null).ToString());
                        rowData.Add(s.admin_number);
                        rowData.Add(s.full_name);
                        rowData.Add(s.mobile_number);
                        rowData.Add(s.email_address);
                        rowData.AddRange(studentProjectChoices);
                        columnData.Add(rowData);
                    }
                    return columnData;
                }
            }
            catch
            {
                throw;
            }
        }
  
      
     
   

	}
}