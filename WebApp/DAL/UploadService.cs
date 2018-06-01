using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using WebApp.Models;
using System;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace WebApp.DAL
{
    public class UploadService
    {
		private readonly IConfiguration configuration;
        public String connectionString;
		public UploadService(IConfiguration config)
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

        /*  A simplified strategy to convert from bytes to KB/GB/MB:
         * http://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
         */
        public static string GetFileSize(double fileSize)
        {
            double f = fileSize;
            int i = 0;

            while (f >= 1024 && i + 1 < Constants.FileSizeSuffixes.Length)
            {
                ++i;
                f /= 1024;
            }

            return string.Format("{0:0.##} {1}", f, Constants.FileSizeSuffixes[i]);
        }

        public void AddFile(int AnnouncementId, Dictionary<string, byte[]> d)
        {

            List<string> compiledFiledata = new List<string>(d.Keys);
            int size = compiledFiledata.Count;
            List<FileResource> listForDatabase = new List<FileResource>();
            foreach (string s in compiledFiledata)
            {
                FileResource f = new FileResource();
                f.name = s.Split('.')[0];
                f.extension = s.Split('.')[1];
                f.data = d[s];
                f.announcement_id = AnnouncementId;
                listForDatabase.Add(f);
            }

            using (var conn = GetConnection())
            {
                conn.Execute(@"insert File_Resource(name, extension, data, announcement_id) values (@name, @extension, @data, @announcement_id)",
                listForDatabase
                );
            }
        }
    }
}