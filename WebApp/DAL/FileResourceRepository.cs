using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Dapper;
using WebApp.Models;
using Microsoft.Extensions.Configuration;

namespace WebApp.DAL
{
    public class FileResourceRepository: IFileResourceRepository
    {
		private readonly IConfiguration configuration;
        public String connectionString;
		public FileResourceRepository()
        {

        }
		public FileResourceRepository(IConfiguration config)
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
        public int DeleteFileResource(int fileResourceId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("DELETE FROM " + FileResource.FileResourceDatabaseTableName + " WHERE " + FileResource.FileResourceIdDatabaseColumnName + " = @fileResourceId", new { fileResourceId });
                    return numberOfRowsAffected;
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteFileResourcesForAnnouncement(int announcementId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var numberOfRowsAffected = connection.Execute("DELETE FROM " + FileResource.FileResourceDatabaseTableName + " WHERE " + FileResource.AnnouncementIdDatabaseColumnName + " = @announcementId", new { announcementId });
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