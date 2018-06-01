using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class GroupStudentViewGroupsViewModel
    {
        public string groupNumber { get; set; }
        public int? supervisorToAssign { get; set; }
        public int? projectToAssign { get; set; }
        public List<SelectListItem> allSupervisors { get; set; }
        public List<SelectListItem> allProjects { get; set; }
        public object groupDetails { get; set; }

        public GroupStudentViewGroupsViewModel() { }

        public void init(IEnumerable<Lecturer> listOfLecturers, IEnumerable<Project> listOfProjects)
        {
            allSupervisors = new List<SelectListItem>();
            allProjects = new List<SelectListItem>();

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

            foreach (Lecturer lecturer in listOfLecturers)
            {
                allSupervisors.Add(new SelectListItem() { Text = lecturer.full_name, Value = lecturer.lecturer_id.ToString() });
            }

            foreach (Project project in listOfProjects)
            {
                allProjects.Add(new SelectListItem() { Text = project.project_title, Value = project.project_id.ToString() });
            }
        }
    }
}