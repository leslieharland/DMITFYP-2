using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Dapper;
using WebApp.Infrastructure.AspNet;
using WebApp.Utils.Helpers;
using WebApp.Models;
using WebApp.Controllers;
using Microsoft.Extensions.Configuration;
using WebApp.ViewModels.Contact;

namespace WebApp.DAL
{
	public class StudentRepository : IStudentRepository
	{
		private readonly IConfiguration configuration;

		public String connectionString;
		public StudentRepository() { }

		public StudentRepository(IConfiguration config)
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

		public virtual Task<Student> getStudentById(string studentId)
		{
			if (string.IsNullOrWhiteSpace(studentId))
				throw new ArgumentNullException("studentId");

			Guid parsedStudentId;
			if (!Guid.TryParse(studentId, out parsedStudentId))
				throw new ArgumentOutOfRangeException("studentId", string.Format("'{0}' is not a valid GUID.", new { studentId }));

			return Task.Factory.StartNew(() =>
			{
				using (var connection = GetConnection())
					return connection.Query<Student>("spGetUser", new { Id = parsedStudentId },
				  commandType: CommandType.StoredProcedure).SingleOrDefault();
			});

		}

		public List<Tag> GetAllStudentsFullName()
		{
			List<Tag> students = new List<Tag>();
			foreach (Student s in Get())
			{
				students.Add(new Tag { id = s.student_id, name = s.full_name, email = s.email_address, mobile = s.mobile_number });
			}
			return students;
		}

		public int GetStudentId(string adminId)
		{
			Console.Write(adminId);
			using (var connection = new SqlConnection(connectionString))
			{
				return connection.Query<int>("Select student_id from Student where admin_number = @adminId", new { adminId }).FirstOrDefault();

			}
		}

		public IEnumerable<Student> Get()
		{
			using (var connection = GetConnection())
			{
				IEnumerable<Student> students = connection.Query<Student>("Select * from Student");
				return students;
			}
		}

		public IEnumerable<Student> GetStudentsFromIds(List<int> Ids)
		{
			//var IdArr = new HashSet<string>(Ids);
			var students = Get().Where(s => Ids.Contains(s.student_id));
			return students;
		}

		public IEnumerable<StudentContact> GetContacts()
		{
			IEnumerable<Student> students = Get();
			using (var connection = GetConnection())
			{
				List<StudentContact> studentContacts = new List<StudentContact>();
				foreach (Student i in students)
				{
					StudentContact contact = new StudentContact();
					contact.Id = i.student_id;
					contact.Admin = i.admin_number;
					contact.FullName = i.full_name;
					contact.Email = i.email_address;
					contact.Mobile = i.mobile_number;
					contact.Semester = i.semester;
					contact.Year = i.year;
					studentContacts.Add(contact);
				}
				return studentContacts;
			}
		}

		public IEnumerable<StudentContact> GetPagedStudents(int? page, int pageSize)
		{
			var pageIndex = page - 1 ?? 0;
			return GetContacts()
				.OrderBy(e => e.FullName)
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();
		}

		public void DeleteAccount(int studentId)
		{
			using (var connection = GetConnection())
			{
				connection.Execute("Delete from Student where student_id = @studentId", new { studentId });
			}
		}

		public Student GetStudentById(string studentId)
		{
			using (var connection = new SqlConnection(connectionString))
            {
				return connection.Query<Student>("Select * from Student where student_id = @studentId", new { studentId }).FirstOrDefault();

            }
		}

		//public bool RequireNewPassword(string passwordLink, string password)
		//{
		//    using (var connection = GetConnection())
		//    {
		//        Student student =  connection.Query<Student>("SELECT * FROM " + Student.StudentDatabaseTableName + " WHERE " + Student.UrlEmbeddedAccountActivationToken + " = @passwordLink", new { passwordLink }).SingleOrDefault();
		//        if (String.IsNullOrEmpty(student.hashed_password))
		//        {
		//            SetPassword(password, student.student_id);
		//            return true;
		//        }
		//        return false;
		//    }
		//}
            

		public void Save()
		{
			throw new NotImplementedException();
		}

		//public void SetPassword(string password, int studentId)
		//{
		//	AppPassword pm = new AppPassword();
		//	string salt = pm.generateSalt();
		//	string sPassword = password + salt;
		//	string hPassword = pm.EncodePassword(sPassword, salt);

		//	using (var connection = GetConnection())
		//	{
		//		connection.Execute("Update Student Set hashed_password = @hPassword, cryptographic_salt = @salt where student_id = @studentId", new { hPassword, salt, studentId });
		//	}
		//}

		public void UpdateStudent(MemberInfo memberInfo)
		{
			using (var connection = GetConnection())
			{
				var p = new DynamicParameters();
				p.Add("@inStudentId", memberInfo.StudentId, dbType: DbType.Int32);
				p.Add("@inGroupId", memberInfo.GroupId, dbType: DbType.Int32);
				p.Add("@inFullName", memberInfo.StudentName, dbType: DbType.String, size: 50);
				p.Add("@inContactNumber", memberInfo.Mobile, dbType: DbType.StringFixedLength, size: 8);
				p.Add("@inEmailAddress", memberInfo.Email, dbType: DbType.String, size: 50);
				p.Add("@inGroupRole", memberInfo.RoleId, dbType: DbType.StringFixedLength, size: 1);

				connection.Execute("usp_updateStudent", p, commandType: CommandType.StoredProcedure);
			}
		}

		public bool CheckDuplicateEmailAddress(string emailAddress)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var p = new DynamicParameters();
					p.Add("@" + Student.EmailAddressDatabaseColumnName, value: emailAddress, dbType: DbType.String, size: 50);
                  
					var numberOfDuplicates = connection.Query<int>("getNumberOfStudentsWithSameEmailAddressSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();
					return numberOfDuplicates > 0;
				}
			}
			catch
			{
				throw;
			}
		}
		public bool CheckDuplicateAdminNumber(string adminNumber)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var p = new DynamicParameters();
					p.Add("@" + Student.AdminNumberDatabaseColumnName, value: adminNumber, dbType: DbType.String, size: 7);
     
					var numberOfDuplicates = connection.Query<int>("getNumberOfStudentsWithSameAdminNumberSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();
					return numberOfDuplicates > 0;
				}
			}
			catch
			{
				throw;
			}
		}
		public IEnumerable<Student> GetStudentsWithPartialFullNameFilter(string partialFullName, Pagination paginationDetails, int courseId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var p = new DynamicParameters();
					p.Add("@partial_" + Student.FullNameDatabaseColumnName, value: partialFullName, dbType: DbType.String, size: 50);
					p.Add("@column_to_sort", value: paginationDetails.databaseColumnToSort, dbType: DbType.String, size: 100);
					p.Add("@sorting_order", value: paginationDetails.sortingOrder, dbType: DbType.String, size: 4);
					p.Add("@start_index", value: paginationDetails.startIndex, dbType: DbType.Int32);
					p.Add("@interval", value: paginationDetails.interval, dbType: DbType.Int32);
					p.Add("@" + Student.CourseIdDatabaseColumnName, value: courseId, dbType: DbType.Int32);

					var students = connection.Query<Student>("getStudentsWithPartialFullNameFilterSP", p, commandType: CommandType.StoredProcedure);
					return students;
				}
			}
			catch
			{
				throw;
			}
		}
		public Student GetStudentByStudentId(int studentId)
		{
			try
			{
				using (var connection = GetConnection())
				{
				
					var student = connection.Query<Student>("SELECT * FROM " + Student.StudentDatabaseTableName + " WHERE " + Student.StudentIdDatabaseColumnName + " = @studentId", new { studentId }).SingleOrDefault();
					return student;
				}
			}
			catch
			{
				throw;
			}
		}

		public int SetGroupRole(int studentId, string groupRole)
		{
			try
			{
				using (var connection = GetConnection())
				{
				
					var numberOfRowsAffected = connection.Execute("UPDATE " + Student.StudentDatabaseTableName + " SET " + Student.GroupRoleDatabaseColumnName + " = @groupRole WHERE " + Student.StudentIdDatabaseColumnName + " = @studentId", new { groupRole, studentId });
					return numberOfRowsAffected;
				}
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<Student> GetStudentsByGroupId(int groupId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var students = connection.Query<Student>("SELECT * FROM " + Student.StudentDatabaseTableName + " WHERE " + Student.GroupIdDatabaseColumnName + " = @groupId", new { groupId });
					return students;
				}
			}
			catch
			{
				throw;
			}
		}

		public int GetNumberOfStudentsAfterPartialFullNameFilter(string partialFullName, int courseId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var p = new DynamicParameters();
					p.Add("@partial_full_name", value: partialFullName, dbType: DbType.String, size: 50);
					p.Add("@" + Student.CourseIdDatabaseColumnName, value: courseId, dbType: DbType.Int32);

					var totalNumberOfStudents = connection.Query<int>("getNumberOfStudentsWithPartialFullNameFilterSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();
					return totalNumberOfStudents;
				}
			}
			catch
			{
				throw;
			}
		}

		public int DeleteStudent(int studentId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var numberOfRowsAffectd = connection.Execute("DELETE FROM " + Student.StudentDatabaseTableName + " WHERE " + Student.StudentIdDatabaseColumnName + " = @studentId", new { studentId });
					return numberOfRowsAffectd;
				}
			}
			catch (Exception e)
			{
				if (e.Message.Contains("fk_project_choice_" + Student.StudentIdDatabaseColumnName)) throw new Exception("There are existing project choice(s) associated to this student you are trying to delete, delete those project choice(s) first before attempting this action.");

				if (e.Message.Contains("fk_group_joining_request_" + Student.StudentIdDatabaseColumnName)) throw new Exception("There are existing group joining request(s) associated to this student you are trying to delete, delete those group joining request(s) before attempting this action.");

				// If the exception was infused by other causes apart from the foreign key constraint, then perform the statement below.
				throw;
			}
		}


		public int SetGroupRoleAndGroupId(int studentId, string groupRole, int? groupId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var p = new DynamicParameters();
					p.Add("@" + Student.StudentIdDatabaseColumnName, value: studentId, dbType: DbType.Int32);
					p.Add("@" + Student.GroupRoleDatabaseColumnName, value: groupRole, dbType: DbType.String, size: 1);
					p.Add("@" + Student.GroupIdDatabaseColumnName, value: groupId, dbType: DbType.Int32);

			
					var numberOfRowsAffected = connection.Execute("setStudentGroupRoleAndGroupIdSP", p, commandType: CommandType.StoredProcedure);
					return numberOfRowsAffected;
				}
			}
			catch
			{
				throw;
			}
		}

		public int CreateStudent(Student student)
		{
			if (CheckDuplicateAdminNumber(student.admin_number) || CheckDuplicateEmailAddress(student.email_address)) throw new DuplicateNameException("The admin number and/ or email address already exist in the database.");

			try
			{
				using (var connection = GetConnection())
				{
					var p = new DynamicParameters();
					p.Add("@" + Student.AdminNumberDatabaseColumnName, value: student.admin_number, dbType: DbType.String, size: 7);
					p.Add("@" + Student.FullNameDatabaseColumnName, value: student.full_name, dbType: DbType.String, size: 50);
					p.Add("@" + Student.MobileNumberDatabaseColumnName, value: student.mobile_number, dbType: DbType.String, size: 8);
					p.Add("@" + Student.EmailAddressDatabaseColumnName, value: student.email_address, dbType: DbType.String, size: 50);
					p.Add("@" + Student.GroupRoleDatabaseColumnName, value: student.group_role, dbType: DbType.String, size: 1);
					p.Add("@" + Student.YearDatabaseColumnName, value: student.year, dbType: DbType.Int32);
					p.Add("@" + Student.SemesterDatabaseColumnName, value: student.semester, dbType: DbType.Int32);
					p.Add("@" + Student.CompletedModuleDatabaseColumnName, value: student.completed_module, dbType: DbType.Boolean);
					p.Add("@" + Student.GroupIdDatabaseColumnName, value: student.group_id, dbType: DbType.Int32);
					p.Add("@" + Student.CourseIdDatabaseColumnName, value: student.course_id, dbType: DbType.Int32);

					var numberOfRowsAffected = connection.Execute("createStudentSP", p, commandType: CommandType.StoredProcedure);
					return numberOfRowsAffected;
				}
			}
			catch
			{
				throw;
			}
		}
		//public Student GetStudentByUrlEmbeddedAccountActivationToken(string urlEmbeddedAccountActivationToken)
		//{
		//	try
		//	{
		//		using (var connection = GetConnection())
		//		{
		//			var student = connection.Query<Student>("SELECT * FROM " + Student.StudentDatabaseTableName + " WHERE " + Student.UrlEmbeddedAccountActivationToken + " = @urlEmbeddedAccountActivationToken", new { urlEmbeddedAccountActivationToken }).SingleOrDefault();
		//			return student;
		//		}
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//}

		public IEnumerable<Student> GetStudents(int courseId)
		{
			try
			{
				using (var connection = GetConnection())
				{


					var students = connection.Query<Student>("SELECT * FROM " + Student.StudentDatabaseTableName + " WHERE (" + Student.CourseIdDatabaseColumnName + " = @courseId" + " AND " + Student.CompletedModuleDatabaseColumnName + " = 0)", new { courseId });
					return students;
				}
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<Student> GetGroupSortedStudents(int courseId)
		{
			throw new NotImplementedException();
		}

		public Student GetStudent(string admin_number)
		{
			using (var connection = new SqlConnection(connectionString))
            {
				return connection.Query<Student>("Select * from Student where admin_number = @admin_number", new { admin_number }).FirstOrDefault();

            }
		}

       
	}
}
