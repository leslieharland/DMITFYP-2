using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels
{
    public class StudentAddStudentViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string adminNumber { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string fullName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string mobileNumber { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string emailAddress { get; set; }
    }
}