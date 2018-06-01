using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class GroupWrapper
    {
        [Key]
        public int groupId {get;set;}
        public string groupNumber{get;set;}
        public bool valid{get;set;}
        public int? lecturerId{get;set;}
        public int? projectId{get;set;}
        public int courseId{get;set;}
        public List<Student> groupMembers{get;set;}
    }
}