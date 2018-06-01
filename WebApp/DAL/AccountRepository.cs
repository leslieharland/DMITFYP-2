using System;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Utils.Helpers;
using Microsoft.Extensions.Configuration;

namespace WebApp.DAL
{
    public class AccountRepository
    {
        private readonly IConfiguration configuration;
		private readonly ILecturerRepository ctx;
		private readonly IStudentRepository studCtx;
		public AccountRepository(IConfiguration configuration, ILecturerRepository lecturerRepository, IStudentRepository studentRepository)
        {
			ctx = lecturerRepository;
			studCtx = studentRepository;
			this.configuration = configuration;
        }
		public string constr = "";
        /// <summary>
        /// This resets the password link for the user 
        /// Enable user to set new password
        /// userId is either admin/ staff id
        /// </summary>
        public async Task ResetUserPassword(string userId)
        {
            int userType = GetUserType(userId);
            if (userType == 1)
            {
                Lecturer lecturer = ctx.GetLecturer(userId);
                if (lecturer != null)
                {
                    await UpdateUserPasswordLink(lecturer.email_address, 1, userId);
                }
            }

            if (userType == 3)
            {
                Student student = studCtx.GetStudent(userId);
                if (student != null)
                {
                    await UpdateUserPasswordLink(student.email_address, 3, userId);
                }
            }


        }

        protected async Task UpdateUserPasswordLink(string emailAddress, int userType, string userId)
        {
            using (var connection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@inUserType", userType, dbType: DbType.Int32);
                p.Add("@inUserId", userId, dbType: DbType.StringFixedLength, size: 7);
                p.Add("@outRandomString", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                connection.Execute("usp_updateUserPasswordLink", p, commandType: CommandType.StoredProcedure);
                string passwordLink = p.Get<string>("@outRandomString");

                string refid = "";
                if (userType == 1)
                    refid = "1184249453";
                if (userType == 3)
                    refid = "1240126338";
                await new EmailService().sendRecoveryEmail(emailAddress, passwordLink,refid);
            }
        }

        public void UpdateProfile(UpdateProfile profile)
        {
            using (var connection = GetConnection())
            {
                if (UserSecretsConfigurationExtensions.)
                {
                    connection.Execute("Update Student Set email_address = @emailAddress, mobile_number = @mobileNo where student_id = @Id", new { Id = SessionVar.GetInt("SID"), emailAddress = profile.Email, mobileNo = profile.Mobile });
                }

                if (User.lecturer)
                {
                    connection.Execute("Update Lecturer Set email_address = @emailAddress, contact_number = @mobileNo where lecturer_id = @Id", new { Id = SessionVar.GetInt("LID"), emailAddress = profile.Email, mobileNo = profile.Mobile });
                }
            }
        }

        public bool RequireNewPassword(string passwordLink, string password, string refId)
        {
            string activateCol = "url_embedded_account_activation_token";
            int userUniqueId = 0;
            int userType = 0;
            using (var connection = GetConnection())
            {             
                if (refId == "1240126338") {
                    Student user = connection.Query<Student>("SELECT * FROM Student WHERE " + activateCol + " = @passwordLink", new { passwordLink }).SingleOrDefault();
                    userUniqueId = user.student_id;
                    userType = 3;
                }
                if (refId == "1184249453")
                {
                    Lecturer user = connection.Query<Lecturer>("SELECT * FROM Lecturer WHERE  " + activateCol + " = @passwordLink", new { passwordLink }).SingleOrDefault();
                    userUniqueId = user.lecturer_id;
                    userType = 1;
                }
                return SetPassword(password, userUniqueId, userType);
            }
        }

        public bool SetPassword(string password, int userUniqueId, int userType)
        {
            AppPassword pm = new AppPassword();
            string salt = pm.generateSalt();
            string sPassword = password + salt;
            string hPassword = pm.EncodePassword(sPassword, salt);
            string tableToUpdate = "";
            string colUniqueName = "";
            if (userType == 1)
            {
                tableToUpdate = "Lecturer";
                colUniqueName = Lecturer.LecturerIdDatabaseColumnName;
            }
            if (userType == 3)
            {
                tableToUpdate = "Student";
                colUniqueName = Student.StudentIdDatabaseColumnName;
            }
            
            using (var connection = GetConnection())
            {
                return (connection.Execute("Update " + tableToUpdate + " Set hashed_password = @hPassword, cryptographic_salt = @salt where " + colUniqueName + " = @userUniqueId", new { hPassword, salt, userUniqueId }) > 0);
            }
        }


        public dynamic CheckUserExistWithPasswordLink(string urlEmbeddedAccountActivationToken, string refId)
        {
            string activateCol = "url_embedded_account_activation_token";
            try
            {
                using (var connection = GetConnection())
                {
                    dynamic user = null;
                    if (refId == "1184249453") { user = connection.Query<Lecturer>("SELECT * FROM " + Lecturer.LecturerDatabaseTableName + " WHERE " + activateCol + " = @urlEmbeddedAccountActivationToken", new { urlEmbeddedAccountActivationToken }).SingleOrDefault(); }
                    if (refId == "1240126338") { user = connection.Query<Student>("SELECT * FROM " + Student.StudentDatabaseTableName + " WHERE " + activateCol + " = @urlEmbeddedAccountActivationToken", new { urlEmbeddedAccountActivationToken }).SingleOrDefault(); }                   
                    
                    return user;
                }
            }
            catch
            {
                throw new Exception("No user found");
            }
        }
        private int GetUserType(string userId)
        {
            int userType = 0;
            switch (userId.Length)
            {
                case 5:
                    userType = 1;
                    break;
                case 7:
                    userType = 3;
                    break;

            }

            if (userType == 0)
            {
                throw new Exception("Unable to get user type.");
            }
            return userType;
        }
        private IDbConnection GetConnection()
        {
            var connection = new SqlConnection(constr);
            connection.Open();
            return connection;
        }

    }

}