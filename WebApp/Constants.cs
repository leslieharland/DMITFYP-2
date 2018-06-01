using System;
using Microsoft.Extensions.Configuration;

namespace WebApp
{
    public class Constants
	{
		public const string LID = "LID";
        public const string SID = "SID";
        public const string CID = "CID";
        public const string isAdmin = "Admin";
        public const string groupRole = "GroupRole";
        public const string groupId = "GroupId";


		public const string AscendingOrderSql = "ASC";
        public const string DescendingOrderSql = "DESC";
        public const int PaginationPageSize = 5;
        public static readonly string[] FileSizeSuffixes = { "bytes", "KB", "MB", "GB" };
        public static readonly DateTime FYPFirstSemesterResetSystemDate = new DateTime(DateTime.Now.Year, 2, 20, 0, 0, 0);
        public static readonly DateTime FYPSecondSemesterResetSystemDate = new DateTime(DateTime.Now.Year, 8, 20, 0, 0, 0);
        public const string PersistentDataXmlVirtualFilePath = "~/Content/persistentdata/PersistentData.xml";
    }
}
