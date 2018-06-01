using System;
using System.Web;
using System.Linq;
using WebApp.Models;
using System.Collections.Generic;

namespace WebApp.DAL
{
    public interface IGroupJoiningRequestRepository
    {
        int DeleteGroupJoiningRequestsForStudent(int studentId);
        int DeleteGroupJoiningRequestsForGroup(int groupId);
        IEnumerable<dynamic> GetPendingResponseGroupJoiningRequests(int groupId);
        IEnumerable<dynamic> GetRespondedGroupJoiningRequests(int studentId);
        int SetRequestAcceptanceDate(DateTime requestAcceptanceDate, int groupJoiningRequestId);
        int SetNotifiedOfGroupJoiningRequestOutcome(bool notifiedOfGroupJoiningRequestOutcome, int groupJoiningRequestId);
        int MassSetNotifiedOfGroupJoiningRequestOutcome(bool notifiedOfGroupJoiningRequestOutcome, int studentId);
        int CreateGroupJoiningRequest(GroupJoiningRequest groupJoiningRequest);
        bool CheckGroupJoiningRequestAlreadySentToGroup(int studentId, int groupId);
        bool CheckStudentAlreadyHaveGroup(int studentId);
        GroupJoiningRequest GetGroupJoiningRequestByGroupJoiningRequestId(int groupJoiningRequestId);
        int AutoRejectOtherGroupJoiningRequests(int studentId);
        int AutoDeletePendingResponseGroupJoinngRequests(int studentId);
    }
}