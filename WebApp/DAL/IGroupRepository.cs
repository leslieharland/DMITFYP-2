using System.Linq;
using System.Collections.Generic;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.DAL
{
    public interface IGroupRepository
    {
        /* These methods are not used. I will just comment them out.
        IEnumerable<Group> Get();
        int? GetGroupId();
        ICollection<GroupViewModel> GetGroupsByIds(int Id);
        MemberInfo GetMemberInfo(int studentId);
        IQueryable<MemberInfo> GetRelatedMemberInfo(int groupId);
        ICollection<MemberInfo> GetMembersNotInRole(int groupId, int roleId);
        void AddGroup(Group group);
        void DeleteGroup(int Id);
        void Save();
        bool CheckExistingRoles(int groupId, int roleId);
        bool SwitchRole(int oldValue, int newValue, int roleId);*/

        int GetNumberOfGroups(int courseId);
        bool CheckGroupFormationWindowPeriod(int courseId);
        bool CheckStudentAlreadyHaveGroup(int studentId);
        bool CheckGroupEmpty(int groupId);
        int DeleteGroupMember(int studentId);
        int AddGroupMember(int studentId, int groupId);
        int AssignProject(int? projectId, int groupId);
        int AssignSupervisor(int? lecturerId, int groupId);
        int DeleteGroup(int groupId);
        object GetGroup(int groupNumber, int courseId);
        object GetGroup(int groupId);
        string GetProspectiveGroupNumber(int courseId);
        int ChangeGroupRole(int studentId, string groupRole);
        int CreateGroup(Group group);
        List<object> GetGroupsByLecturerId(int lecturerId);
        IEnumerable<GroupWrapper> GetGroups(int courseId);
        List<List<string>> GetGroupProjectSelectionSpreadsheetData(int courseId);

    }
}