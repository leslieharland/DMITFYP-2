using System.Collections.Generic;
using WebApp.Models;


namespace WebApp.DAL
{
    public interface ILecturerRepository
    {
        IEnumerable<Lecturer> Get();
        Lecturer GetLecturer(string staffId);
        Lecturer GetLecturerById(int Id);
        void CreateAccount(LecturerContact lecturer);
        IEnumerable<Lecturer> GetLecturers(int courseId);
        int DeleteLecturer(int lecturerId);
        string GenerateRandomUrlEmbeddedAccountActivationToken();
        int CreateLecturer(Lecturer student);
        bool CheckDuplicateStaffId(string staffId);
    }
}
