using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Group
    {
        public const string GroupDatabaseTableName = "\"Group\"";
        public const string GroupIdDatabaseColumnName = "group_id";
        public const string GroupNumberDatabaseColumnName = "group_number";
        public const string ValidDatabaseColumnName = "valid";
        public const string LecturerIdDatabaseColumnName = "lecturer_id";
        public const string ProjectIdDatabaseColumnName = "project_id";
        public const string CourseIdDatabaseColumnName = "course_id";
        [Key]
        public int group_id { get; set; }
        public string group_number { get; set; }
		[ForeignKey("Lecturer")]
        public int? lecturer_id { get; set; }
		[ForeignKey("Project")]
        public int? project_id { get; set; }
		[ForeignKey("Course")]
        public int course_id { get; set; }
        public bool valid { get; set; }
    }
}