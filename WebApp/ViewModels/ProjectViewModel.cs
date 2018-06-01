using ChameleonForms.Component.Config;
using WebApp.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ProjectViewModel
    {
        [Required]
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }


        [Display(Name = "Project Aims")]
        public string ProjectAims { get; set; }
        [Display(Name = "Project Objective")]
        public string ProjectObjective { get; set; }
        [Display(Name = "Target Audience")]
        public string TargetAudience { get; set; }
        [Display(Name = "Main Functions And Deliverables")]
        public string MainFunctionsAndDeliverables { get; set; }
        [Display(Name = "Hardware And Software Configuration")]
        public string HardwareAndSoftwareConfig { get; set; }
        [Display(Name = "Is Project Available for Students?")]
        public string IsProjectAvailable { get; set; }
    }

    namespace InitiatedProject
    {
        public class InitiatedProjectViewModel
        {
            [Display(Name = "Group no. ")]
            public int GroupId { get; set;}
            [ScaffoldColumn(false)]
            [ExcludeProperty]
            public int Id { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(50)]
            public string Title { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string ProjectOverview { get; set; }

            [Required]
            [Display(Name = "Intro/ Background")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string IntroBackground { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [Display(Name = "Key Innovation/ Research Goals/ Technical Approach")]
            [MaxLength(500)]
            public string KeyInnovationAndResearchGoals { get; set; }

            [Required]
            [Display(Name = "Comparison of the Merits")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string ComparisonOfTheMerits { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string TargetAudience { get; set; }

            [Required]
            [Display(Name = "Business Model/ Market Potential")]
            [MaxLength(500)]
            [DataType(DataType.MultilineText)]
            public string BusinessModelAndMarketPotential { get; set; }

            [Required]
            [Display(Name = "Main Functions/ Deliverables")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string MainFunction { get; set; }

            [Required]
            [Display(Name = "Project Plan/ Schedule")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string ProjectPlan { get; set; }

            [Required]
            [Display(Name = "Hardware and software requirements")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]

            public string HardwareAndSoftwareRequirements { get; set; }

            [Required]
            [Display(Name = "Problems and countermeasures")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string ProblemsAndCountermeasures { get; set; }

            [ExcludeProperty]
            public string Submit { get; set; }
            [ExcludeProperty]
            public bool Resubmit { get; set; }
            [ExcludeProperty]
            public bool Availability { get; set; }

            [ExcludeProperty]
            public DateTime? SavedDate { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public string ProjectType { get; set; }

            [ExcludeProperty]
            public string displayFields { get; set; }
            [ExcludeProperty]
            public List<Field> displayFieldsModel { get; set; }
            [ExcludeProperty]
            public string removeIds { get; set; }
            [ExcludeProperty]
            public bool ProjectHistory { get; set; }
        }

        public class Field
        {
            public int id { get; set; }
            public string label { get; set; }
            public string value { get; set; }
        }

        public enum DescriptionHelper
        {
            [Display(Name = "Describes the aims and objectives of the project - abstract")]
            ProjectOverview,
            [Display(Name = "Describe how the project idea come about or current problems")]
            IntroBackground,
            [Display(Name = "Describe what technology or research idea to be used")]
            KeyInnovationAndResearchGoals,
            [Display(Name = "Describe the advantages/ improvements of the project vs. those of the current user")]
            ComparisonOfTheMerits,
            [Display(Name = "Describe the target audience for this project, e.g. mobile users, students, government agency, etc.")]
            TargetAudience,
            [Display(Name = "Describe how to market (or commercialise) your product if successful")]
            BusinessModelAndMarketPotential,
            [Display(Name = "List the main functions of the application and deliverables (usually prototype), includes additional feature enhancement if time allows")]
            MainFunctionsAndDeliverables,
            [Display(Name = "List the main activities for the project by week(s), including manpower requirements - function distribution of group members")]
            ProjectPlan,
            [Display(Name = "Describes/plan the proposed hardware configuration and software required")]
            HardwareAndSoftwareRequirements,
            [Display(Name = "Brief risk analysis - optional")]
            ProblemsAndCountermeasures

        }
    }

    namespace Proposal
    {
        public class ProposalViewModel
        {


            [ScaffoldColumn(false)]
            [ExcludeProperty]
            public int Id { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(50)]
            public string Title { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string Aims { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(1000)]
            public string Objectives { get; set; }

            [ExcludeProperty]
            public ReadyForApproval ReadyForApproval { get; set; }

            public string Deadline { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string Schedule { get; set; }

            [Required]
            [Display(Name = "Target Audience")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string TargetAudience { get; set; }

            [Required]
            [Display(Name = "Main Function")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string MainFunction { get; set; }

            [Required]
            [Display(Name = "Hardware & Software Configuration")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500)]
            public string HardwareAndSoftwareConfiguration { get; set; }

            [Display(Name = "Company Name")]
            [Required]
            [MaxLength(50)]
            public string CompanyName { get; set; }
            [Required]
            [MaxLength(100)]
            public string Address { get; set; }
            [Required]
            [Phone]
            public string Tel { get; set; }
            public string Fax { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Liaison Officer")]
            [MaxLength(50)]
            public string LiaisonOfficer { get; set; }

            [Required]
            public bool WillingToSponsor { get; set; }

            public IFieldConfiguration ModifyConfig(IFieldConfiguration config)
            {
                return config;
            }
            [ExcludeProperty]
            public int LecturerId { get; set; }
            [ExcludeProperty]
            public int CourseId { get; set; }
            [ExcludeProperty]
            public DateTime? SubmittedDate { get; set; }
            [ExcludeProperty]
            public string Submit { get; set; }
            [ExcludeProperty]
            public bool Availability { get; set; }
            public string ProjectType { get; set; }
            [ExcludeProperty]
            public bool ProjectHistory { get; set; }
        }

        public enum ReadyForApproval
        {
            [Description("3 Days")]
            ThreeDays,
            [Description("7 Days")]
            SevenDays,
            [Description("10 Days")]
            TenDays
        }

        public enum DescriptionHelper
        {
            [Display(Name = "Describes the aims of the project")]
            Aims,
            [Display(Name = "List the main activities for the 16-week project")]
            Schedule,
            [Display(Name = "List the objectives of the project")]
            Objectives,
            [Display(Name = "Describe the target audience for this project")]
            TargetAudience,
            [Display(Name = "List the main functions of the application and deliverables, usually a prototype")]
            MainFunctionsAndDeliverables,
            [Display(Name = "Describes/sketch the proposed hardware configuration and software required")]
            HardwareAndSoftwareRequirements

        }
    }
}