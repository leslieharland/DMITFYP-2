using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
namespace WebApp.ViewModels
{
    /*
     * There is not a definition for ViewModel is not included in ASP.NET MVC specification. However it is necessary in some cases where
     * it is practically impossible to have a view bound to a entity. In this example of group viewing, binding the LecturerViewGroups.cshtml
     * view to the Group entity model is impractical because Group has irrelevant attributes such as "valid". Additionally, on the contrary,
     * Group is also missing of some attributes which are crucial to the view, for example the allSupervisors attribute
     * (of type; List<SelectlistItem>).
     * 
     * As from my research, ViewModels unlike models contains only properties which are needed in the View.
     * 
     * With the introduction of ViewModels into the application, it can be argued that View Models are a violation of the MVC technology,
     * I would however not contest to that, for some reasons or so. One of which is; the three layers M, V and C are still retained, with only an
     * additional layer.
     */
    public class GroupViewGroupsViewModel
    {
        public string groupNumber { get; set; }
        public int? supervisorToAssign { get; set; }
        public int? projectToAssign { get; set; }
        public List<SelectListItem> allSupervisors { get; set; }
        public List<SelectListItem> allProjects { get; set; }
        public List<SelectListItem> groupRoles { get; set; }
        public IEnumerable<Student> allStudents { get; set; }
        public object groupDetails { get; set; }
        public GroupViewGroupsViewModel()
        {
            groupRoles = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Leader", Value = "1"},
                new SelectListItem(){Text = "Assistant Leader", Value = "2"},
                new SelectListItem(){Text = "Member", Value = "3"}
            };
        }

        public void init(IEnumerable<Lecturer> listOfLecturerOnes, IEnumerable<Project> listOfProjects, IEnumerable<Student> listOfStudents)
        {
            allSupervisors = new List<SelectListItem>();
            allProjects = new List<SelectListItem>();
            allStudents = listOfStudents;

            if ((groupDetails.GetType().GetProperty("lecturerId").GetValue(groupDetails, null)) != null)
            {
                supervisorToAssign = ((int)(groupDetails.GetType().GetProperty("lecturerId").GetValue(groupDetails, null)));
            }
            else
            {
                supervisorToAssign = null;
            }

            if ((groupDetails.GetType().GetProperty("projectId").GetValue(groupDetails, null)) != null)
            {
                projectToAssign = ((int)(groupDetails.GetType().GetProperty("projectId").GetValue(groupDetails, null)));
            }
            else
            {
                projectToAssign = null;
            }

            foreach (Lecturer lo in listOfLecturerOnes)
            {
                allSupervisors.Add(new SelectListItem() { Text = lo.full_name, Value = lo.lecturer_id.ToString() });
            }

            foreach (Project p in listOfProjects)
            {
                allProjects.Add(new SelectListItem() { Text = p.project_title, Value = p.project_id.ToString() });
            }
        }
    }
}