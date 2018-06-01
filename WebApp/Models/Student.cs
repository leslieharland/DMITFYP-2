using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Student
    {
        public const string StudentDatabaseTableName = "Student";
        public const string StudentIdDatabaseColumnName = "student_id";
        public const string AdminNumberDatabaseColumnName = "admin_number";
        public const string FullNameDatabaseColumnName = "full_name";
        public const string MobileNumberDatabaseColumnName = "mobile_number";
        public const string EmailAddressDatabaseColumnName = "email_address";
		public const string GroupRoleDatabaseColumnName = "group_role";
        public const string YearDatabaseColumnName = "year";
        public const string SemesterDatabaseColumnName = "semester";
        public const string CompletedModuleDatabaseColumnName = "completed_module";
        public const string GroupIdDatabaseColumnName = "group_id";
        public const string CourseIdDatabaseColumnName = "course_id";

        [Key]
        public int student_id { get; set; }
        public string admin_number { get; set; }
        public string full_name { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
        [ForeignKey("Group")]
        public int? group_id { get; set; }
        public string group_role { get; set; }
        public int year { get; set; }
        public int semester { get; set; }
        public bool completed_module { get; set; }
		[ForeignKey("Course")]
        public int course_id { get; set; }
        public Student() { }
    }

    public class StudentModel
    {
        public string admin_number { get; set; }
        public string full_name { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
    }
}