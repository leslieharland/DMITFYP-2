using System;
using System.Web;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Configuration;
using System.Collections.Generic;
using WebApp.DAL;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Infrastructure.AspNet;


namespace WebApp.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILecturerRepository lecturerRepository;

        public LecturerController()
        {
            lecturerRepository = new LecturerRepository();
        }

        public ActionResult ViewLecturers()
        {
            return View();
        }
        public ActionResult AddLecturer()
        {
            return View("~/Views/Lecturer/AddLecturer.cshtml");
        }

        [HttpPost]
        public ActionResult AddLecturer(LecturerAddLecturerViewModel model)
        {
            int numberOfRowsAffected = 0;
            int courseId = 1; // Assume this courseId was acquired from the current HTTP Session.
            try
            {
                string urlEmbeddedAccountActivationToken = lecturerRepository.GenerateRandomUrlEmbeddedAccountActivationToken();

                numberOfRowsAffected = lecturerRepository.CreateLecturer(new Lecturer()
                {
                    staff_id = model.staffId,
                    full_name = model.fullName,
                    contact_number = model.contactNumber,
                    email_address = model.emailAddress,
                    admin = model.admin,                  
                    course_id = courseId
                });

                //new EmailService().sendNewAccountEmail(model.emailAddress, urlEmbeddedAccountActivationToken, "1184249453");
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e) // Generic Exception handler.
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }

        public ActionResult DeleteLecturer(int lecturerId)
        {
            /*
             * Deleting a lecturer is quite an impactful action to the system's data because a lecturer record
             * has its primary key (student_id) as a foriegn key in other entities, thus prompting the associated
             * records in those entities to also be deleted.
             * 
             * The worst case scenario for this, is when the deletion of a lecturer affects n number of entities (prompting their records to be removed also), where n is the maximum number of entities that
             * can be affected.
             * 
             * This scenario occurs when the FYP time-period has commenced and the lecturer, being in supervision
             * of FYP group(s) and also, happened to propose "lecturer-intiated" projects and made announcements
             * (also attaching file resources to at least one of the announcement(s)) before (the commenement of
             * the FYP time-period), has decided to leave SP. In this worst case scenario, n turns out to be 4
             * (The four affected entities are namely; Announcement, File_Resource, Group,
             * Industry_Lecturer_Project).
             * 
             * Superificially, it might seem OK to have those record(s) in (all) the associated tables, deleted.
             * However practically, it would make no sense, to have a group (with students in it deleted) merely
             * because its supervisor has left SP.
             * 
             * Hence, in practicality n would be modified as; n = 4 - 1 = 3, where the entity, "exempted" from
             * deletion is; Group.
             * 
             * Additionally, it should be noted that n can be further reduced to; n = 3 - 2 = 1, where the
             * additional two entities, which should also be exempted deletion (alike the above-mentioned
             * Group entity) are; Announcement and File_Resource. However due to technical database
             * constraint, those two entities would not be removed from deletion. Thus, n is still; 3.
             * 
             * Also, like Group, the records in the Industry_Lecturer_Project entity should not be deleted. Thus
             * n = 3 - 1 = 2;
             * 
             * Hence as dicussed above, in the worst case scenario, only at most n entities can be affected, where
             * n is revealed to be 2. The affected entities are thus Announcement and File_Resource.
             * 
             * Thus, with the above discussion, the code below deletes associated records from the the Announcement
             * and File_Resource entity.
             */

            // Variable declaration and initialization.
            int numberOfRowsAffected = 0;
            IAnnouncementRepository announcementRepository = new AnnouncementRepository();
            IFileResourceRepository fileResourceRepository = new FileResourceRepository();
            IProjectRepository projectRepository = new ProjectRepository();
            IGroupRepository groupRepository = new GroupRepository();
            IEnumerable<Announcement> associatedAnnouncements = null;
            IEnumerable<object> associatedGroups = null;
            IEnumerable<dynamic> associatedIndustryLecturerProjects = null;

            try
            {
                associatedAnnouncements = announcementRepository.GetAnnouncementsByLecturerId(lecturerId);

                // Deleting records from the File_Resource entity.
                foreach (Announcement announcement in associatedAnnouncements)
                {
                    fileResourceRepository.DeleteFileResourcesForAnnouncement(announcement.announcement_id);
                }

                // Deleting Announcement's records.
                announcementRepository.DeleteAnnouncementsForLecturer(lecturerId);

                associatedGroups = groupRepository.GetGroupsByLecturerId(lecturerId);

                // Disassociating any group records which are currently associated to the (to be) deleted lecturer record, by setting the group_role field in the Group entity to NULL.
                foreach (object group in associatedGroups)
                {
                    int groupId = int.Parse(group.GetType().GetProperty("groupId").GetValue(group, null).ToString());
                    groupRepository.AssignSupervisor(null, groupId);
                }

                associatedIndustryLecturerProjects = projectRepository.GetIndustryLecturerProjectsByLecturerId(lecturerId);

                // Disassociating any project records which are currently associated to the (to be) deleted lecturer record, by setting the lecturer_id field in the Lecturer entity to NULL.
                foreach (dynamic project in associatedIndustryLecturerProjects)
                {
                    projectRepository.SetLecturerId(project.project_id, null);
                }

                // Finally, once all records are properly settled, as from above, delete the lecturer (from the Lecturer entity).
                numberOfRowsAffected = lecturerRepository.DeleteLecturer(lecturerId);

                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message });
            }
        }
    }
}