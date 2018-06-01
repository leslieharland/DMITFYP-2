using WebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Contact
{
    public class ComposeViewModel
    {
        [Required(ErrorMessage = "Must send to at least one recipient")]
        public string recipients { get; set; }
        [Required(ErrorMessage = "Don't forget the subject")]
        public string subject { get; set; }
        [Required(ErrorMessage = "Text message required")]
        public string content { get; set; }
        [Required]
        public string typetosend { get; set; }
        public IEnumerable<StudentContact> StudentContacts { get; set; }
    }
}