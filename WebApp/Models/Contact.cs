using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    #region general
    public class StudentContact : Entity
    {
        [Required(ErrorMessage = "Student Admin must be exactly 7 digits. Please recheck.")]
        [MaxLength(7)]
        public string Admin { get; set; }
        [Required(ErrorMessage = "Limit 50 characters")]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(8)]
        [Phone]
        public string Mobile { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        [DefaultValue(1)]
        public int UserTypeId { get; set; }

    }

    public class LecturerContact : Entity
    {
        [Required(ErrorMessage = "Limit 50 characters")]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Staff Id must be exactly 5 digits. Please recheck.")]
        [MaxLength(5)]
        public string StaffId { get; set; }
        [Required]
        [Phone]
        public string Mobile { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [DefaultValue(2)]
        public int UserTypeId { get; set; }
    }
    #endregion

}