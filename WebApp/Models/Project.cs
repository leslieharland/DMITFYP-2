using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Project
    {
		[Key]
        public int project_id {get;set;}
        public string project_title {get;set;}
        public string project_aims {get;set;}
        public string project_objectives {get;set;}
        public string target_audience {get;set;}
        public string main_functions_and_deliverables {get;set;}
        public string hardware_and_software_requirements{get;set;}
        public bool project_availability {get;set;}
    }
}
        