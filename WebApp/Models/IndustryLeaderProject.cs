using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class IndustryLeaderProject
    {
        [Key]
        public int project_id { get; set; }
        public string project_milestone { get; set; }
        public string company_name { get; set; }
        public string company_postal_address { get; set; }
        public string company_telephone_number { get; set; }
        public string company_fax_number { get; set; }
        public string company_email_address { get; set; }
        public bool company_sponsorship_willingness { get; set; }
        public string company_liaison_officer_name { get; set; }
        public DateTime deadline { get; set; }
        public int lecturer_id { get; set; }
    }
}