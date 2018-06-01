using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

using WebApp.Filter;
using WebApp.DAL;

namespace WebApp.Models
{
    public class MemberInfo
    {
        public enum Roles
        {
            Leader = 1,
            [Display(Name = "Assistant Leader")]
            AsstLeader = 2,
            Member = 3
        }
        [Required]
        public int? GroupId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string StudentName { get; set; }
        public string AdminNumber { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int RoleId { get; set; }
        public string Role { get; set; }

        public IEnumerable<MemberInfo> Members { get; set; }

        public IEnumerable<SelectListItem> MemberList
        {
            get { return new SelectList(Members, "StudentId", "StudentName"); }
        }

    }
}