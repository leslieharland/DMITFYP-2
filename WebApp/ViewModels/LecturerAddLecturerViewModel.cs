using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels
{
    public class LecturerAddLecturerViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string staffId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string fullName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string contactNumber { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string emailAddress { get; set; }
        public bool admin { get; set; }
    }
}