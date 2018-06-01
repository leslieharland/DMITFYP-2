
using WebApp.Models;
using WebApp.ModelBinders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewModels
{
    [ModelBinder(typeof(ContactModelBinder))]
    public class ContactViewModel
    {
        public class ContactType
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        private readonly List<ContactType> _types = new List<ContactType>()
        {
            new ContactType {Id=1, Name="Student" },
            new ContactType {Id=2, Name="Lecturer" }
        };

        [Display(Name = "User Type")]
        public int SelectedUserType { get; set; }

        public IEnumerable<SelectListItem> ContactTypes
        {
            get { return new SelectList(_types, "Id", "Name"); }
        }
        public IEnumerable<StudentContact> StudentContacts { get; set; }

        public StudentContact StudentContact { get; set; }

        public LecturerContact LecturerContact { get; set; }
        public bool Export { get; set; }
    }
}