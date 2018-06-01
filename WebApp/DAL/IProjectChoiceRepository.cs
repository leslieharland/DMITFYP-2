using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.DAL
{
    public interface IProjectChoiceRepository
    {
        int DeleteProjectChoicesForStudent(int studentId);
        IEnumerable<ProjectChoice> GetProjectChoicesByStudentId(int studentId);
        IEnumerable<ProjectChoice> GetSortedProjectChoicesByStudentId(int studentId);
        int CreateProjectChoice(ProjectChoice projectChoice);
        bool CheckProjectChoiceWindowPeriod(int courseId, int semester);
        List<string> GetProjectChoiceWindowPeriod(int courseId, int semester);
        void SetProjectChoiceWindowPeriod(int courseId, int semesterId, string startDate, string endDate);
    }
}