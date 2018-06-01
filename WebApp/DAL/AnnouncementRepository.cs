using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Dapper;
using WebApp.Infrastructure.AspNet;
using WebApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace WebApp.DAL
{
	public class AnnouncementRepository : IAnnouncementRepository
	{
		private readonly IConfiguration configuration;
		public String connectionString;
		public AnnouncementRepository(IConfiguration config)
		{
			configuration = config;
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public AnnouncementRepository() { }

		private IDbConnection GetConnection()
		{
			var connection = new SqlConnection(connectionString);
			connection.Open();
			return connection;
		}

		public List<Announcement> Get(int? page, int courseId)
		{
			int rowsToFetch = 10;
			if (!page.HasValue) { page = 1; }

			using (var connection = GetConnection())
			{
				return connection.Query("SELECT * FROM Announcement WHERE course_id = @courseId ORDER BY announcement_date DESC", new { courseId }).Select<dynamic, Announcement>(row => { return GetAnnouncementData(row); }).Skip((Convert.ToInt16(page) - 1) * rowsToFetch).Take(rowsToFetch).ToList();
			}
		}
		private Announcement GetAnnouncementData(dynamic data)
		{
			Announcement announcement = new Announcement();
			announcement.announcement_id = data.announcement_id;
			announcement.lecturer_id = data.lecturer_id;
			announcement.message = data.message;
			announcement.title = data.title;
			announcement.announcement_date = data.announcement_date;
			//Lazy<List<FileResource>> files = new Lazy<List<FileResource>>(() => GetFileResource(data.announcement_id));
			List<FileResource> files = new List<FileResource>();
			files = GetFileResource(data.announcement_id);
			announcement.filesDisplay = files;
			return announcement;
		}

		public Announcement GetAnnouncementById(int Id)
		{

			using (var connection = GetConnection())
			{
				var announcement = new Announcement();
				announcement = connection.Query<Announcement>("Select * from Announcement Where announcement_id = @Id", new { Id }).SingleOrDefault();
				List<FileResource> files = new List<FileResource>();
				files = GetFileResource(announcement.announcement_id);
				announcement.filesDisplay = files;
				return announcement;
			}
		}

		public FileResource GetFile(int Id)
		{
			using (var connection = GetConnection())
			{
				return connection.Query<FileResource>(
					"SELECT * FROM File_Resource WHERE file_resource_id = @Id", new { Id }).SingleOrDefault();
			}
		}
		public List<FileResource> GetFileResource(int Id)
		{
			using (var connection = GetConnection())
			{
				return connection.Query<FileResource>(
					"Select * from File_Resource where announcement_id = @Id", new { Id }).ToList();
			}
		}
		public int AddAnnouncement(Announcement announcement)
		{
			DateTime currentTime = DateTime.Now;
			announcement.lecturer_id = 1;
			using (var connection = GetConnection())
			{
				var p = new DynamicParameters();
				p.Add("@announcementDate", value: currentTime, dbType: DbType.DateTime);
				p.Add("@message", value: announcement.message, dbType: DbType.String, size: 1000);
				p.Add("@title", value: announcement.title, dbType: DbType.String, size: 100);
				p.Add("@lastEditDate", value: currentTime, dbType: DbType.DateTime);
				p.Add("@lecturerId", value: announcement.lecturer_id, dbType: DbType.Int32);
				p.Add("@courseId", value: announcement.course_id, dbType: DbType.Int32);
				p.Add("@outAnnouncementId", dbType: DbType.Int32, direction: ParameterDirection.Output);

				connection.Execute("usp_addAnnouncement", p, commandType: CommandType.StoredProcedure);

				int announcementId = p.Get<int>("@outAnnouncementId");
				return announcementId;
			}

		}
		public int UpdateAnnouncement(Announcement announcement)
		{
			announcement.lecturer_id = 1;
			List<FileResource> files = new List<FileResource>();
			files = GetFileResource(announcement.announcement_id);
			announcement.filesDisplay = files;
			using (var connection = GetConnection())
			{
				var p = new DynamicParameters();
				p.Add("@inMessage", announcement.message, dbType: DbType.String);
				if (!String.IsNullOrEmpty(announcement.title))
				{
					p.Add("@inTitle", announcement.title, dbType: DbType.String);
				}
				p.Add("@inLecturerId", announcement.lecturer_id, dbType: DbType.Int32);
				p.Add("@inAnnouncementId", announcement.announcement_id, dbType: DbType.Int32);

				int rowCount = connection.Execute("usp_updateAnnouncement", p, commandType: CommandType.StoredProcedure);
				return rowCount;
			}
		}
		public int DeleteAnnouncement(int Id)
		{
			using (var connection = GetConnection())
			{
				var p = new DynamicParameters();
				p.Add("@inAnnouncementId", dbType: DbType.Int32, value: Id);

				int rowCount = connection.Execute("usp_deleteAnnouncement", p, commandType: CommandType.StoredProcedure);
				return rowCount;
			}

		}
		public void Save() { }


		public int DeleteAnnouncementsForLecturer(int lecturerId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var numberOfRowsAffected = connection.Execute("DELETE FROM Announcement " + " WHERE lecturer_id = @lecturerId", new { lecturerId });
					return numberOfRowsAffected;
				}
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<Announcement> GetAnnouncementsByLecturerId(int lecturerId)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var announcements = connection.Query<Announcement>("SELECT * FROM Announcement WHERE lecturer_id = @lecturerId", new { lecturerId });
					return announcements;
				}
			}
			catch
			{
				throw;
			}
		}
	}
}