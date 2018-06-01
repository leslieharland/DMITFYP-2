using WebApp.Models;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class GroupViewModel
    {
        public ICollection<MemberInfo> Members { get; set; }
        public string Supervisor { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public bool GroupFull { get; set; }
        public bool GroupApproved { get; set; }
    }
}