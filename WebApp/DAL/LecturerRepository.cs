using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Dapper;
using WebApp.Infrastructure.AspNet;
using WebApp.Utils.Helpers;
using WebApp.Models;
using Microsoft.Extensions.Configuration;

namespace WebApp.DAL
{
    public class LecturerRepository : ILecturerRepository
    {
		private readonly IConfiguration configuration;
        public String connectionString;

		public LecturerRepository(){

		}
		public LecturerRepository(IConfiguration config)
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
        public void AddLecturer(Lecturer lecturer)
        {
            using (var connection = GetConnection())
            {             
                var p = new DynamicParameters();
                p.Add("@inStaffId", lecturer.staff_id, DbType.StringFixedLength);
                p.Add("@inFullName", lecturer.full_name, dbType: DbType.String);
                p.Add("@inContactNumber", lecturer.contact_number, dbType: DbType.StringFixedLength);
                p.Add("@inEmailAddress", lecturer.email_address, dbType: DbType.String);

				connection.Execute("xyxy54.13@ichat.sp.edu.sg.com", p, commandType: CommandType.StoredProcedure);
            }
        }
        public void UpdateLecturerProfile(Lecturer lecturer)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inLecturerId", lecturer.lecturer_id, DbType.Int32);
                p.Add("@inStaffId", lecturer.staff_id, DbType.StringFixedLength);
                p.Add("@inFullName", lecturer.full_name, dbType: DbType.String);
                p.Add("@inContactNumber", lecturer.contact_number, dbType: DbType.StringFixedLength);
                p.Add("@inEmailAddress", lecturer.email_address, dbType: DbType.String);

                connection.Execute("usp_updateLecturer", p, commandType: CommandType.StoredProcedure);
            }
        }

        //public void UpdateAccount(Lecturer lecturer)
        //{
        //    using (var connection = GetConnection())
        //    {
        //        var p = new DynamicParameters();
        //        p.Add("@inLecturerId", lecturer.lecturer_id, DbType.Int32);
        //        p.Add("@inPassword", lecturer.hashed_password, dbType: DbType.String);
        //        p.Add("@inSalt", lecturer.cryptographic_salt, dbType: DbType.String);

        //        connection.Execute("usp_updateLecturerSecurity", p, commandType: CommandType.StoredProcedure);
        //    }
        //}
       
        public void Save() { }
  
              
        public Lecturer GetLecturerById(int Id)
        {
            using (var connection = GetConnection())
            {
                Lecturer lecturer = connection.Query<Lecturer>("Select * from Lecturer where lecturer_id = @Id", new { Id }).SingleOrDefault();
                return lecturer;
            }

        }

        public Lecturer GetStaffFromSId(string staffId)
        {
            Lecturer lecturer = null;
            using (var connection = GetConnection())
            {
                return lecturer = connection.Query<Lecturer>("SELECT * FROM " + Lecturer.LecturerDatabaseTableName + " WHERE " + Lecturer.StaffIdDatabaseColumnName + " = @staffId", new { staffId }).FirstOrDefault();
            }
        }
        public string GenerateRandomUrlEmbeddedAccountActivationToken()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var randomUrlEmbeddedAccountActivationToken = connection.Query<string>("getRandomUrlEmbeddedAccountActivationToken", commandType: CommandType.StoredProcedure).SingleOrDefault();
                    return randomUrlEmbeddedAccountActivationToken;
                }
            }
            catch
            {
                throw;
            }
        }

        /*
         * It actually makes more sense to have access modifier of this method as private as this method is solely referenced from within this
         * class. There would however be a compile-time error on doing so as the C# compiler expects a public access-modifier for such a method
         * which is implementing an abstract method.
         */
        public bool CheckDuplicateStaffId(string staffId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();

                    p.Add("@" + Lecturer.StaffIdDatabaseColumnName, value: staffId, dbType: DbType.String, size: 5);

                    var numberOfDuplicates = connection.Query<int>("getNumberOfLecturersWithSameStaffIdSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    return numberOfDuplicates > 0;
                }
            }
            catch
            {
                throw;
            }
        }

        ///*
        // * It actually makes more sense to have access modifier of this method as private as this method is solely referenced from within this
        // * class. There would however be a compile-time error on doing so as the C# compiler expects a public access-modifier for such a method
        // * which is implementing an abstract method.
        // */
        //public bool CheckDuplicateEmailAddress(string emailAddress)
        //{
        //    try
        //    {
        //        using (var connection = GetConnection())
        //        {
        //            var p = new DynamicParameters();

        //            p.Add("@" + Lecturer.EmailAddressDatabaseColumnName, value: emailAddress, dbType: DbType.String, size: 50);

        //            var numberOfDuplicates = connection.Query<int>("getNumberOfLecturersWithSameEmailAddressSP", p, commandType: CommandType.StoredProcedure).SingleOrDefault();

        //            return numberOfDuplicates > 0;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        public int CreateLecturer(Lecturer lecturer)
        {
            //if (CheckDuplicateStaffId(lecturer.staff_id) || CheckDuplicateEmailAddress(lecturer.email_address)) throw new DuplicateNameException("The staff id and/ or email address already exist in the database.");

            try
            {
                using (var connection = GetConnection())
                {

                    var p = new DynamicParameters();
					p.Add("@" + Lecturer.StaffIdDatabaseColumnName, value: lecturer.lecturer_id, dbType: DbType.Int32);
                    p.Add("@" + Lecturer.StaffIdDatabaseColumnName, value: lecturer.staff_id, dbType: DbType.String);
                    p.Add("@" + Lecturer.FullNameDatabaseColumnName, value: lecturer.full_name, dbType: DbType.String, size: 50);
                    p.Add("@" + Lecturer.ContactNumberDatabaseColumnName, value: lecturer.contact_number, dbType: DbType.String, size: 8);
                    p.Add("@" + Lecturer.EmailAddressDatabaseColumnName, value: lecturer.email_address, dbType: DbType.String, size: 50);
                    
                    p.Add("@" + Lecturer.AdminDatabaseColumnName, value: lecturer.admin, dbType: DbType.Boolean);
                
                    p.Add("@" + Lecturer.CourseIdDatabaseColumnName, value: lecturer.course_id, dbType: DbType.Int32);

                    var numberOfRowsAffected = connection.Execute("createLecturerSP", p, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }




        public IEnumerable<LecturerContact> GetContacts()
        {
            IEnumerable<Lecturer> lecturers = Get();
            using (var connection = GetConnection())
            {
                ICollection<LecturerContact> lecturerContacts = null;
                foreach (Lecturer i in lecturers)
                {
                    LecturerContact contact = new LecturerContact();
                    contact.Id = i.lecturer_id;
                    contact.FullName = i.full_name;
                    contact.Mobile = i.contact_number;
                    contact.IsAdmin = i.admin;
                    lecturerContacts.Add(contact);
                }
                return lecturerContacts;
            }
        }

        public void CreateAccount(LecturerContact lecturer)
        {
            using (var connection = GetConnection())
            {
                string email = lecturer.Email;
                var p = new DynamicParameters();
                p.Add("@inFullName", lecturer.FullName, dbType: DbType.String, size: 50);
                p.Add("@inStaffId", lecturer.StaffId, dbType: DbType.StringFixedLength, size: 5);
                p.Add("@inEmailAddress", email, dbType: DbType.String, size: 50);
                p.Add("@inContactNumber", lecturer.Mobile, dbType: DbType.StringFixedLength, size: 8);
                p.Add("@inIsAdmin", lecturer.IsAdmin, dbType: DbType.Boolean);
                p.Add("@outRandomString", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                connection.Execute("usp_addNewLecturer", p, commandType: CommandType.StoredProcedure);
                string passwordLink = p.Get<string>("@outRandomString");
                //new EmailService().sendNewAccountEmail(email, passwordLink);
            }
        }

        public IEnumerable<Lecturer> GetLecturers(int courseId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var lecturers = connection.Query<Lecturer>("SELECT * FROM " + Lecturer.LecturerDatabaseTableName + " WHERE " + Lecturer.CourseIdDatabaseColumnName + " = @courseId", new { courseId });
                    return lecturers;
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteLecturer(int lecturerId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("DELETE FROM " + Lecturer.LecturerDatabaseTableName + " WHERE " + Lecturer.LecturerIdDatabaseColumnName + " = @lecturerId", new { lecturerId });
                    return numberOfRowsAffected;

					//var p = new DynamicParameters();
                    //p.Add("@inLecturerId", Id, DbType.Int32);

                    //connection.Execute("usp_updateLecturerSecurity", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }
        }



        public Lecturer GetLecturer(string staffId)
        {
            using (var connection = GetConnection())
            {
                return connection.Query<Lecturer>("SELECT * FROM " + Lecturer.LecturerDatabaseTableName + " WHERE " + Lecturer.StaffIdDatabaseColumnName + " = @staffId", new { staffId }).SingleOrDefault();
            }
        }
	}
}