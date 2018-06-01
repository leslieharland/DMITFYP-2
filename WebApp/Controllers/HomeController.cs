using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApp.Infrastructure.AspNet;
using Microsoft.AspNetCore.Authorization;
using WebApp.DAL;

namespace WebApp.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IAnnouncementRepository _announcementRepository;
		private readonly IGroupJoiningRequestRepository _groupJoiningRepository;
        

		public HomeController(IAnnouncementRepository announcementRepository, IGroupJoiningRequestRepository groupJoiningRequestRepository)
		{
			_announcementRepository = announcementRepository;
			_groupJoiningRepository = groupJoiningRequestRepository;
		}
		public ActionResult Index()
		{

				int courseId = 1;
			int? groupId = HttpContext.Session.GetInt32(Constants.groupId);
			string groupRole = HttpContext.Session.GetString(Constants.groupRole);
			int? studentId = HttpContext.Session.GetInt32(Constants.SID);

				// If the user is a student, not a lecturer or FYP MC, then do the following.
				if (studentId != 0)
				{
					// If the user is a student and he/ she is a leader, then do the following.
					if (groupRole == "1")
					{

						int gId = groupId ?? default(int);
						ViewData["GroupJoiningRequests"] = _groupJoiningRepository.GetPendingResponseGroupJoiningRequests(gId);

					}
					else
					{
						int sId = studentId ?? default(int);
						// If the user is a student and he/ she is either an assistant leader or a group member, then do the following.
						ViewData["GroupJoiningRequests"] = _groupJoiningRepository.GetRespondedGroupJoiningRequests(sId);
					}
				}

				ViewData["Announcements"] = _announcementRepository.Get(null, 1);
				TempData["Page"] = 1;
				if (User.IsInRole("Admin"))
				{
					return View("~/Views/_Admin/Announcement/All.cshtml");
				}
				else
				{
					return View("~/Views/_Admin/Announcement/All.cshtml");
				}

		}
	}
}