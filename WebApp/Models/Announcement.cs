using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Announcement
    {
        [Key]
        public int announcement_id { get; set; }
        public DateTime announcement_date { get; set; }
        [Required]
        public string message { get; set; }
        public string title { get; set; }
        public DateTime last_edit_date { get; set; }
		[Required]
		[ForeignKey("Lecturer")]
        public int lecturer_id { get; set; }
		[ForeignKey("Course")]
        public int course_id { get; set; }
        public Lazy<List<FileResource>> files { get; set; }
        public List<FileResource> filesDisplay { get; set; }
    }
}