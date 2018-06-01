using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using WebApp.Models;
using Microsoft.Extensions.Configuration;

namespace WebApp.DAL
{
    public class CourseRepository: ICourseRepository
    {
		private readonly IConfiguration configuration;
        public String connectionString;
		public CourseRepository(IConfiguration config)
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
      

        public IEnumerable<CourseEntity> GetCourses()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    var courses = connection.Query<CourseEntity>("SELECT * FROM Course");
                    return courses;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}