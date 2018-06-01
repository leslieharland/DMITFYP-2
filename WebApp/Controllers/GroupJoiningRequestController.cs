using System;
using System.Web;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApp.DAL;
using WebApp.Models;
using WebApp.Infrastructure.AspNet;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class GroupJoiningRequestController : Controller
    {
        private readonly IGroupJoiningRequestRepository groupJoiningRequestRepository;
        private readonly IGroupRepository groupRepository;
        public GroupJoiningRequestController()
        {
            groupJoiningRequestRepository = new GroupJoiningRequestRepository();
            groupRepository = new GroupRepository();
        }

        public ActionResult IndicateNotificationOfAllGroupJoiningRequestStatus(int studentId)
        {
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupJoiningRequestRepository.MassSetNotifiedOfGroupJoiningRequestOutcome(true, studentId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult IndicateNotificationOfGroupJoiningRequestStatus(int groupJoiningRequestId)
        {
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupJoiningRequestRepository.SetNotifiedOfGroupJoiningRequestOutcome(true, groupJoiningRequestId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message});
            }
        }
        public ActionResult AcceptGroupJoiningRequest(int groupJoiningRequestId)
        {
            int numberOfRowsAffected = 0;
            GroupJoiningRequest groupJoiningRequest = groupJoiningRequestRepository.GetGroupJoiningRequestByGroupJoiningRequestId(groupJoiningRequestId);
            try
            {
                numberOfRowsAffected = groupJoiningRequestRepository.SetRequestAcceptanceDate(DateTime.Now, groupJoiningRequestId);
                groupJoiningRequestRepository.SetNotifiedOfGroupJoiningRequestOutcome(false, groupJoiningRequestId);
                groupRepository.AddGroupMember(groupJoiningRequest.student_id, groupJoiningRequest.group_id);
                groupJoiningRequestRepository.AutoRejectOtherGroupJoiningRequests(groupJoiningRequest.student_id);
                return Json(new {rowsAffected = numberOfRowsAffected});
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult RejectGroupJoiningRequest(int groupJoiningRequestId)
        {
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupJoiningRequestRepository.SetRequestAcceptanceDate(new DateTime(1970, 1, 1), groupJoiningRequestId);
                groupJoiningRequestRepository.SetNotifiedOfGroupJoiningRequestOutcome(false, groupJoiningRequestId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult AddGroupJoiningRequest(int groupId)
        {
            int studentId = HttpContext.Session.GetInt32("SID") ?? default(int);
            int numberOfRowsAffected = 0;
            try
            {
                numberOfRowsAffected = groupJoiningRequestRepository.CreateGroupJoiningRequest(new GroupJoiningRequest()
                {
                    group_joining_request_id = 0,
                    request_date = DateTime.Now,
                    request_acceptance_date = null,
                    student_id = studentId,
                    group_id = groupId
                });
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }
    }
}