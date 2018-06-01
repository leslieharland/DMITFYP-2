using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class StudentProject
    {
        [Key]
        public int Id { get; set; }
        public string Desc_Background { get; set; }
        public string Desc_StrategyApproach { get; set; }
        public string Desc_MeritComparison { get; set; }
        public string Desc_CommercialisationStrategy { get; set; }
        public string Desc_ProjectMilestoneAndWorkload { get; set; }
        public string Desc_ProblemsAndCountermeasures { get; set; }
    }
}