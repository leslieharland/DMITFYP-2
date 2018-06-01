using System.Collections.Generic;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.ViewModels.Contact;

namespace WebApp.DAL
{
	public interface IStudentRepository
    {
        IEnumerable<Student> Get();
        IEnumerable<Student> GetStudentsFromIds(List<int> Ids);
        IEnumerable<StudentContact> GetContacts();
        void DeleteAccount(int Id);
        List<Tag> GetAllStudentsFullName();
        IEnumerable<Student> GetStudentsWithPartialFullNameFilter(string partialFullName, Pagination paginationDetails, int courseId);
        IEnumerable<Student> GetStudents(int courseId);
        IEnumerable<Student> GetGroupSortedStudents(int courseId);
        int CreateStudent(Student student);
        bool CheckDuplicateAdminNumber(string adminNumber);
        bool CheckDuplicateEmailAddress(string emailAddress);
        int SetGroupRoleAndGroupId(int studentId, string groupRole, int? groupId);
        int SetGroupRole(int studentId, string groupRole);
        int DeleteStudent(int studentId);
        int GetNumberOfStudentsAfterPartialFullNameFilter(string partialFullName, int courseId);
        IEnumerable<Student> GetStudentsByGroupId(int groupId);
        int GetStudentId(string admin);
		Student GetStudent(string admin);
        Student GetStudentByStudentId(int studentId);
    }
}