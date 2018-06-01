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
    using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Group_Joining_Request
    {
        [Key]
        public int group_joining_request_id { get; set; }
        public System.DateTime request_date { get; set; }
        public Nullable<System.DateTime> request_acceptance_date { get; set; }
		[ForeignKey("Student")]
        public int student_id { get; set; }
		[ForeignKey("Group")]
        public int group_id { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual Student Student { get; set; }
    }
}
