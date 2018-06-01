using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ProjectChoice
    {
        public const string ProjectChoiceDatabaseTableName = "Project_Choice";
        public const string StudentIdDatabaseColumnName = "student_id";
        public const string ProjectIdDatabaseColumnName = "project_id";
        public const string RankingPrecedenceDatabaseColumnName = "ranking_precedence";
        public const string SubmittedDateDatabaseColumnName = "submitted_date";

        public int student_id { get; set; }
        public int project_id { get; set; }
        public int ranking_precedence { get; set; }
        public DateTime submitted_date { get; set; }
    }
}