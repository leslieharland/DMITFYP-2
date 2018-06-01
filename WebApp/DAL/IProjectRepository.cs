using WebApp.Models;
using System.Collections.Generic;
using WebApp.ViewModels.InitiatedProject;

namespace WebApp.DAL
{
    public interface IProjectRepository
    {
        IEnumerable<Project> Get();
        InitiatedProjectViewModel GetProject(int Id, int groupId = 0);
        InitiatedProjectViewModel GetProjectByGroup(int groupId);
        IEnumerable<dynamic> GetSelfInitiatedProjects(int courseId);
        IEnumerable<dynamic> GetIndustryLecturerProjects(int courseId);
        IEnumerable<dynamic> GetIndustryLecturerProjectsByLecturerId(int lecturerId);
        void AddSelfInitiatedProject(InitiatedProjectViewModel project, int groupId, int courseId);
        void UpdateProject(InitiatedProjectViewModel project);
        void DeleteProject(int Id);
        void DeleteProjectFields(InitiatedProjectViewModel project);
        void AddProjectHistory(int Id);
        bool IsProjectHistory(int Id);
        IEnumerable<Project> GetProjects(int courseId);
        IEnumerable<Project> GetAvailableProjects(int courseId);

        // These methods are useful for churning data to be inserted in a dynamically generated spreadsheet.
        List<List<string>> GetSelfInitiatedProjectsSpreadsheetData(int courseId);
        List<List<string>> GetIndustryLecturerProjectsSpreadsheetData(int courseId);
        List<List<string>> GetCombinedProjectsSpreadsheetData(int courseId);
        List<List<string>> GetAvailableProjectsSpreadsheetData(int courseId);

        // This method only applies to projects which are from the Industry_Lecturer_Project entity.
        int SetLecturerId(int projectId, int? lecturerId);
    }
}
