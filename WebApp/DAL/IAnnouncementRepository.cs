using WebApp.Models;
using System.Collections.Generic;

namespace WebApp.DAL
{
    public interface IAnnouncementRepository
    {
        List<Announcement> Get(int? page, int courseId);
        IEnumerable<Announcement> GetAnnouncementsByLecturerId(int lecturerId);
        Announcement GetAnnouncementById(int Id);
        FileResource GetFile(int Id);
        List<FileResource> GetFileResource(int Id);
        int AddAnnouncement(Announcement announcement);
        int UpdateAnnouncement(Announcement announcement);
        int DeleteAnnouncement(int Id);
        int DeleteAnnouncementsForLecturer(int lecturerId);
    }
}
