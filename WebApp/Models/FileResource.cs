using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class FileResource
    {
        public const string FileResourceDatabaseTableName = "File_Resource";
        public const string FileResourceIdDatabaseColumnName = "file_resource_id";
        public const string NameDatabaseColumnName = "name";
        public const string ExtensionDatabaseColumnName = "extension";
        public const string DataDatabaseColumnName = "data";
        public const string AnnouncementIdDatabaseColumnName = "announcement_id";
        [Key]
        public int file_resource_id { get; set; }
        public string name { get; set; }
        public string extension { get; set; }
        public byte[] data { get; set; }
		[ForeignKey("Announcement")]
        public int announcement_id { get; set; }
    }
}