using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.DAL
{
    public interface ICourseRepository
    {
        IEnumerable<CourseEntity> GetCourses();
    }
}