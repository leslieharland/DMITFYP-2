using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using WebApp.Models;
using Microsoft.Extensions.Configuration;

namespace WebApp.DAL
{
    public class GroupJoiningRequestRepository: IGroupJoiningRequestRepository
    {
		private readonly IConfiguration configuration;
        
        public String connectionString;
		public GroupJoiningRequestRepository(IConfiguration config)
        {
            configuration = config;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

		public GroupJoiningRequestRepository(){
           
		}

        private IDbConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
	

		public int DeleteGroupJoiningRequestsForGroup(int groupId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					var numberOfRowsAffected = conn.Execute("DELETE FROM " + GroupJoiningRequest.GroupJoiningRequestDatabaseTableName + " WHERE " + GroupJoiningRequest.GroupIdDatabaseColumnName + " = @groupId", new { groupId });
					return numberOfRowsAffected;
				}
			}
			catch
			{
				throw;
			}
		}
        
		public GroupJoiningRequest GetGroupJoiningRequestByGroupJoiningRequestId(int groupJoiningRequestId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var groupJoiningRequest = connection.Query<GroupJoiningRequest>("SELECT * FROM " + GroupJoiningRequest.GroupJoiningRequestDatabaseTableName + " WHERE " + GroupJoiningRequest.GroupJoiningRequestIdDatabaseColumnName + " = @groupJoiningRequestId", new { groupJoiningRequestId }).SingleOrDefault();
                    return groupJoiningRequest;
                }
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<dynamic> GetRespondedGroupJoiningRequests(int studentId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var groupJoiningRequests = connection.Query<dynamic>("getRespondedGroupJoiningRequestsSP", new { studentId }, commandType: CommandType.StoredProcedure);
                    return groupJoiningRequests;
                }
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<dynamic> GetPendingResponseGroupJoiningRequests(int groupId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var groupJoiningRequests = connection.Query<dynamic>("getPendingResponseGroupJoiningRequestsSP", new { groupId }, commandType: CommandType.StoredProcedure);
                    return groupJoiningRequests;
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteGroupJoiningRequestsForStudent(int studentId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("DELETE FROM " + GroupJoiningRequest.GroupJoiningRequestDatabaseTableName + " WHERE " + GroupJoiningRequest.StudentIdDatabaseColumnName + " = @studentId", new { studentId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetRequestAcceptanceDate(DateTime requestAcceptanceDate, int groupJoiningRequestId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("UPDATE " + GroupJoiningRequest.GroupJoiningRequestDatabaseTableName + " SET " + GroupJoiningRequest.RequestAcceptanceDateDatabaseColumnName + " = @requestAcceptanceDate WHERE " + GroupJoiningRequest.GroupJoiningRequestIdDatabaseColumnName + " = @groupJoiningRequestId", new { requestAcceptanceDate, groupJoiningRequestId });
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

        public bool CheckGroupJoiningRequestAlreadySentToGroup(int studentId, int groupId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfDuplicates = connection.Query<int>("getNumberOfGroupJoiningRequestsWithSameGroupIdAndStudentIdSP", new { studentId, groupId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    return numberOfDuplicates > 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public int AutoDeletePendingResponseGroupJoinngRequests(int studentId)
        {
            int numberOfRowsAffected;
            try
            {
                using (var connection = GetConnection())
                {
                    numberOfRowsAffected = connection.Execute("deletePendingResponseGroupJoiningRequestsSP", new { studentId }, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }
        public int AutoRejectOtherGroupJoiningRequests(int studentId)
        {
            int numberOfRowsAffected;
            try
            {
                using (var connection = GetConnection())
                {
                    numberOfRowsAffected = connection.Execute("rejectOtherGroupJoiningRequestsSP", new { studentId }, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public int MassSetNotifiedOfGroupJoiningRequestOutcome(bool notifiedOfGroupJoiningRequestOutcome, int studentId)
        {
            int numberOfRowsAffected;
            try
            {
                using (var connection = GetConnection())
                {
                    numberOfRowsAffected = connection.Execute("UPDATE " + GroupJoiningRequest.GroupJoiningRequestDatabaseTableName + " SET " + GroupJoiningRequest.NotifiedOfGroupJoiningRequestOutcome + " = @notifiedOfGroupJoiningRequestOutcome WHERE " + GroupJoiningRequest.StudentIdDatabaseColumnName + " = @studentId", new { notifiedOfGroupJoiningRequestOutcome, studentId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public int SetNotifiedOfGroupJoiningRequestOutcome(bool notifiedOfGroupJoiningRequestOutcome, int groupJoiningRequestId)
        {
            int numberOfRowsAffected;
            try
            {
                using (var connection = GetConnection())
                {
                    numberOfRowsAffected = connection.Execute("UPDATE " + GroupJoiningRequest.GroupJoiningRequestDatabaseTableName + " SET " + GroupJoiningRequest.NotifiedOfGroupJoiningRequestOutcome + " = @notifiedOfGroupJoiningRequestOutcome WHERE " + GroupJoiningRequest.GroupJoiningRequestIdDatabaseColumnName + " = @groupJoiningRequestId", new { notifiedOfGroupJoiningRequestOutcome, groupJoiningRequestId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public int CreateGroupJoiningRequest(GroupJoiningRequest groupJoiningRequest)
        {
            if (CheckStudentAlreadyHaveGroup(groupJoiningRequest.student_id)) throw new Exception("The student already belong to a group.");
            if (CheckGroupJoiningRequestAlreadySentToGroup(groupJoiningRequest.student_id, groupJoiningRequest.group_id)) throw new Exception("Group Joining Request already sent to this group from this student.");
            int numberOfRowsAffected;
            try
            {
                using (var connection = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@" + GroupJoiningRequest.RequestDateDatabaseColumnName, value: groupJoiningRequest.request_date, dbType: DbType.DateTime);
                    p.Add("@" + GroupJoiningRequest.RequestAcceptanceDateDatabaseColumnName, value: groupJoiningRequest.request_acceptance_date, dbType: DbType.DateTime);
                    p.Add("@" + GroupJoiningRequest.StudentIdDatabaseColumnName, value: groupJoiningRequest.student_id, dbType: DbType.Int32);
                    p.Add("@" + GroupJoiningRequest.GroupIdDatabaseColumnName, value: groupJoiningRequest.group_id, dbType: DbType.Int32);
                    p.Add("@" + GroupJoiningRequest.NotifiedOfGroupJoiningRequestOutcome, value: null, dbType: DbType.Boolean);
                    numberOfRowsAffected = connection.Execute("createGroupJoiningRequestSP", p, commandType: CommandType.StoredProcedure);
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }
	      
	}
}