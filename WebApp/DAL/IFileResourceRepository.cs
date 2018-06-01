using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;


namespace WebApp.DAL
{
    public interface IFileResourceRepository
    {
        int DeleteFileResource(int fileResourceId);
        int DeleteFileResourcesForAnnouncement(int announcementId);
    }
}