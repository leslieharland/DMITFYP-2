using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Lecturer
    {
        public const string LecturerDatabaseTableName = "Lecturer";
        public const string LecturerIdDatabaseColumnName = "lecturer_id";
        public const string StaffIdDatabaseColumnName = "staff_id";
        public const string FullNameDatabaseColumnName = "full_name";
        public const string ContactNumberDatabaseColumnName = "contact_number";
        public const string EmailAddressDatabaseColumnName = "email_address";
        public const string AdminDatabaseColumnName = "admin";
        public const string CourseIdDatabaseColumnName = "course_id";

		[Key]
        public int lecturer_id { get; set; }
        [Required(ErrorMessage="No staff id given")]
        public string staff_id { get; set; }
        public string full_name { get; set; }
        public string contact_number { get; set; }
        public string email_address { get; set; }
        public bool admin { get; set; }
		[ForeignKey("Course")]
        public int course_id { get; set; }       
    }

    // This class is used in the DataTable API.
    public class LecturerModel
    {
        public string staff_id { get; set; }
        public string full_name { get; set; }
        public bool admin { get; set; }
        public string contact_number { get; set; }
        public string email_address { get; set; }
    }
}