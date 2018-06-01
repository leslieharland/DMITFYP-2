using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CourseEntity
    {
        public const string CourseDatabaseTableName = "Course";
        public const string CourseIdDatabaseColumnName = "course_id";
        public const string CourseFullNameDatabaseColumnName = "course_full_name";
        public const string CourseAcronymDatabaseColumnName = "course_acronym";

        [Key]
        public int course_id { get; set; }
        public string course_full_name { get; set; }
        public string course_acronym { get; set; }
    }
}