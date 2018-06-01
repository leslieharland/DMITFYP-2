using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApp.Models;
using WebApp.DAL;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementRepository _announcementctx;
        private List<string> FileList = new List<string>();      
        public AnnouncementController(IAnnouncementRepository announcement)
        {
            _announcementctx = announcement;
        }

        public ActionResult DeleteFileResource(int fileResourceId)
        {
            //IFileResourceRepository fileResourceRepository = new FileResourceRepository();
            int numberOfRowsAffected = 0;
            try
            {
               //numberOfRowsAffected = fileResourceRepository.DeleteFileResource(fileResourceId);
                return Json(new { rowsAffected = numberOfRowsAffected });
            }
            catch (Exception e)
            {
                return Json(new { rowsAffected = numberOfRowsAffected, error = e.Message});
            }
        }

        public ActionResult GetUpdatedPost(int id)
        {
            Announcement announcement = _announcementctx.GetAnnouncementById(id);
            return PartialView("Post", announcement);
        }
        public ActionResult GetEditForm(int id)
        {
            Announcement announcement = _announcementctx.GetAnnouncementById(id);
            return PartialView("Edit", announcement);
        }

        public ActionResult Get(int? page)
        {
			int? courseId = HttpContext.Session.GetInt32("CourseId");
			int cId = courseId ?? default(int);
            List<Announcement> a = _announcementctx.Get(page, cId);
            ViewData["Announcements"] = a;
            bool stopAppend = false;

            if (stopAppend == false)
            {
                if (page == null)
                {
                    TempData["Page"] = 1;
                }
                else
                {
                    TempData["Page"] = page + 1;
                }

                if (a.Count != 10)
                {
                    stopAppend = true;
                }
                return PartialView("~/Views/_Admin/Announcement/Get.cshtml");
            }
            return Json("last");

        }
        public ActionResult DeleteUploadedFiles()
        {
            int? staffId = HttpContext.Session.GetInt32("LID");
			int sId = staffId ?? default(int);
			//string pathString = Path.Combine(Server.MapPath("~/Content/uploads"), sId.ToString());
			string pathString = "";
            if (Directory.Exists(pathString)) Directory.Delete(pathString, true);

            return null;
        }
        public ActionResult AddFileResources(int announcementId)
        {
			int? staffId = HttpContext.Session.GetInt32("LID");
            int sId = staffId ?? default(int);
            string path = Path.Combine("~/Content/uploads/", staffId.ToString());
			string pathString = "";
           // string pathString = Path.Combine(Server.MapPath("~/Content/uploads"), staffId.ToString());
            if (Directory.Exists(pathString))
            {
				DirectoryInfo uploadFolder = new DirectoryInfo(pathString);
                FileInfo[] files = uploadFolder.GetFiles();

                Dictionary<string, byte[]> d = new Dictionary<string, byte[]>();
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = files[i].ToString();
                    byte[] fileData = GetBytes(Path.Combine(path, fileName));
                    d.Add(fileName, fileData);
                }

                Directory.Delete(pathString, true);
               // new UploadService().AddFile(announcementId, d);
            }
            return PartialView("Post", _announcementctx.GetAnnouncementById(announcementId));
        }
        [HttpPost]
        public ActionResult Post(Announcement announcement)
        {
            //ILecturerRepository lecturerRepository = new LecturerRepository();
            DateTime currentTime = DateTime.Now;
            int? staffId = HttpContext.Session.GetInt32("LID");
            string path = Path.Combine("~/Content/uploads/", staffId.ToString());
			//string pathString = Path.Combine(Server.MapPath("~/Content/uploads"), staffId.ToString());
			string pathString = "";
			announcement.course_id = HttpContext.Session.GetInt32("CourseId") ?? default(int);;
            announcement.last_edit_date = currentTime;
            announcement.announcement_date = currentTime;
			announcement.lecturer_id = HttpContext.Session.GetInt32("LID") ?? default(int);

            int announcementId = _announcementctx.AddAnnouncement(announcement);

            if (Directory.Exists(pathString))
            {
               // DirectoryInfo uploadFolder = new DirectoryInfo(Server.MapPath(path));
                //FileInfo[] files = uploadFolder.GetFiles();
               // UploadService uploadsvc = new UploadService();

                Dictionary<string, byte[]> d = new Dictionary<string, byte[]>();
                List<FileResource> fd = null;
                //for (int i = 0; i < files.Length; i++)
                //{
                //    string fileName = files[i].ToString();
                //    byte[] fileData = GetBytes(Path.Combine(path, fileName));
                //    d.Add(fileName, fileData);
                
                //}
                
                //Directory.Delete(pathString, true);
                //uploadsvc.AddFile(announcementId, d);
                announcement.filesDisplay = _announcementctx.GetFileResource(announcementId);
            }
            else
            {
                announcement.filesDisplay = new List<FileResource>() ;
            }

            //Before returning the view, update announcement with its id.
            announcement.announcement_id = announcementId;

            return PartialView("Post", announcement);
        }

        [HttpPost]
        public ActionResult Edit(Announcement announcement)
        {

            int count = _announcementctx.UpdateAnnouncement(announcement);
            return PartialView("Post", announcement);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
			//IFileResourceRepository fileResourceRepository = new FileResourceRepository();
			//fileResourceRepository.DeleteFileResourcesForAnnouncement(id);
			//int count = _announcementctx.DeleteAnnouncement(id);
			//bool removed = (count > 0) ? true : false;
			//return Json(removed);
			return null;
        }

        public byte[] GetBytes(string path)
        {
			//FileStream fs = null;
			//BinaryReader br = null;
			//try
			//{
			//    byte[] buffer = null;
			//    fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.Read);
			//    br = new BinaryReader(fs);
			//    long numBytes = new FileInfo(Server.MapPath(path)).Length;
			//    buffer = br.ReadBytes((int)numBytes);
			//    br.Close();
			//    fs.Close();
			//    return buffer;
			//}
			//catch (Exception err)
			//{
			//    throw new Exception("SaveFileIntoDatabase: " + err.Message);
			//}
			return null;
        }

        public ActionResult FileRequest(int id)
        {
            FileResource fileData = _announcementctx.GetFile(id);

            if (fileData == null)
            {
                // Clears the response stream.
                Response.Clear();
                //Response.AddHeader("Content-Disposition", "attachment;filename=FileNotFound.txt");
                //Response.Write("Sorry, the requested file could not be found on the server.");

                // Sends whatever that is in the buffer.
                //Response.Flush();
                return null;
            }

            string fullFileName = fileData.name + "." + fileData.extension;

            if ((new[] { "png", "jpg", "jpeg" }).ToList().Contains(fileData.extension))
            {
                return File(fileData.data, "image");
            }
            else if (((new[] { "wmv", "mp4" }).ToList().Contains(fileData.extension)))
            {
                return File(fileData.data, "video");
            }

            switch (fileData.extension)
            {
                case "pdf":
                    return File(fileData.data, "application/pdf", fullFileName);
                case "doc":
                    return File(fileData.data, "application/msword", fullFileName);
                case "docx":
                    return File(fileData.data, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fullFileName);
                case "xls":
                    return File(fileData.data, "application/vnd.ms-excel", fullFileName);
                case "xlsx":
                    return File(fileData.data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fullFileName);
                case "ppt":
                    return File(fileData.data, "application/vnd.ms-powerpoint", fullFileName);
                case "pptx":
                    return File(fileData.data, "application/vnd.openxmlformats_officedocument.presentationml.presentation", fullFileName);
                default:
                    return null;
            }
        }

   //     public ActionResult UploadFile(int? entityId)
   //     {
   //         var statuses = new List<ViewDataUploadFileResult>();
			//int userId = HttpContext.Session.GetInt32("LID")?? default(int);;
        //    string pathString = Path.Combine(Server.MapPath("~/Content/uploads"), userId.ToString());
        //    if (!Directory.Exists(pathString)) Directory.CreateDirectory(pathString);
        //    for (var i = 0; i < Request.Files.Count; i++)
        //    {
        //        var st = FileSaver.StoreFile(x =>
        //        {
        //            x.File = Request.Files[i];
        //            x.DeleteUrl = Url.Action("DeleteFile", new { entityId = entityId });
        //            DateTime timestamp = DateTime.Now;
        //            x.StorageDirectory = pathString;
        //            x.UrlPrefix = "/Content/uploads/" + userId;
        //            x.FileName = Request.Files[i].FileName;
        //            x.ThrowExceptions = true;
        //        });
        //        statuses.Add(st);
        //    }


        //    statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80");
        //    statuses.ForEach(x => x.url = Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));


        //    var viewresult = Json(new { files = statuses });
        //    //for IE8 which does not accept application/json
        //    if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
        //        viewresult.ContentType = "text/plain";

        //    return viewresult;
        //}


        //[HttpPost]
        //public ActionResult DeleteFile(int? entityId, string fileUrl)
        //{
        //    var filePath = Server.MapPath("~" + fileUrl);

        //    if (System.IO.File.Exists(filePath))
        //        System.IO.File.Delete(filePath);

        //    var viewresult = Json(new { error = String.Empty });
        //    //for IE8 which does not accept application/json
        //    if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
        //        viewresult.ContentType = "text/plain";

        //    return viewresult; // trigger success
        //}


        //public ActionResult DownloadFile(string fileUrl, string mimetype)
        //{
        //    var filePath = Server.MapPath("~" + fileUrl);

        //    if (System.IO.File.Exists(filePath))
        //        return File(filePath, mimetype);
        //    else
        //    {
        //        return new HttpNotFoundResult("File not found");
        //    }
        //}
    }
}