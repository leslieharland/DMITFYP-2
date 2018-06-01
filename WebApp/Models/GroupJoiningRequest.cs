using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class GroupJoiningRequest
    {
        public const string GroupJoiningRequestDatabaseTableName = "Group_Joining_Request";
        public const string GroupJoiningRequestIdDatabaseColumnName = "group_joining_request_id";
        public const string RequestDateDatabaseColumnName = "request_date";
        public const string RequestAcceptanceDateDatabaseColumnName = "request_acceptance_date";
        public const string StudentIdDatabaseColumnName = "student_id";
        public const string GroupIdDatabaseColumnName = "group_id";
        public const string NotifiedOfGroupJoiningRequestOutcome = "notified_of_group_joining_request_outcome";
        
        [Key]
        public int group_joining_request_id { get; set; }
        public DateTime request_date { get; set; }
        public DateTime? request_acceptance_date { get; set; }
		[ForeignKey("Student")]
        public int student_id { get; set; }
		[ForeignKey("Group")]
        public int group_id { get; set; }
        public bool notified_of_group_joining_request_outcome { get; set; }
    }
}