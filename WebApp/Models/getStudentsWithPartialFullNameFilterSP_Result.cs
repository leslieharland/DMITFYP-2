//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Models
{
    using System;
    
    public partial class getStudentsWithPartialFullNameFilterSP_Result
    {
        public Nullable<long> Q { get; set; }
        public int student_id { get; set; }
        public string admin_number { get; set; }
        public string full_name { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
        public string group_role { get; set; }      
        public int year { get; set; }
        public int semester { get; set; }
        public bool completed_module { get; set; }
        public Nullable<int> group_id { get; set; }
        public int course_id { get; set; }
    }
}
