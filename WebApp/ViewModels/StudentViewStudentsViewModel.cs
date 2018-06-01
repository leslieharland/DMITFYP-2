using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class StudentViewStudentsViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string partialFullName { get; set; }
        public string startIndex { get; set; }
        public string sortingOrder { get; set; }
        public string databaseColumnToSortBy { get; set; }
        public IEnumerable<Student> students { get; set; }
        public List<SelectListItem> databaseColumns { get; set; }
        public List<SelectListItem> sortingOrders { get; set; }
        public StudentViewStudentsViewModel()
        {
            sortingOrders = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Ascending", Value = "ASC"},
                new SelectListItem(){Text = "Descending", Value = "DESC"}
            };
            databaseColumns = new List<SelectListItem>(){
                new SelectListItem(){ Text = "Admin Number", Value = Student.AdminNumberDatabaseColumnName},
                new SelectListItem(){ Text = "Student Name", Value = Student.FullNameDatabaseColumnName },
                new SelectListItem(){ Text = "Mobile Number", Value = Student.MobileNumberDatabaseColumnName},
                new SelectListItem(){ Text = "Email Address", Value = Student.EmailAddressDatabaseColumnName},
            };
        }
    }
}