using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using WebApp.Models;
namespace WebApp.DAL
{
    public class ADOFileResourceRepository: IFileResourceRepository
    {
        public int DeleteFileResource(int fileResourceId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Constants.NathanaelDatabaseConnectionStringName].ToString()))
                {
                    conn.Open();
                    var numberOfRowsAffected = conn.Execute("DELETE FROM " + FileResource.FileResourceDatabaseTableName + " WHERE " + FileResource.FileResourceIdDatabaseColumnName + " = @fileResourceId", new { fileResourceId });
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
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Constants.NathanaelDatabaseConnectionStringName].ToString()))
                {
                    conn.Open();
                    var numberOfRowsAffected = conn.Execute("DELETE FROM " + FileResource.FileResourceDatabaseTableName + " WHERE " + FileResource.AnnouncementIdDatabaseColumnName + " = @announcementId", new { announcementId });
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