using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class Pagination
    {
        public Pagination(int inStartIndex, int inInterval, string inDatabaseColumnToSort, string inSortingOrder)
        {
            startIndex = inStartIndex;
            interval = inInterval;
            databaseColumnToSort = inDatabaseColumnToSort;
            sortingOrder = inSortingOrder;
        }

        public int startIndex { get; set; }
        public int interval { get; set; }
        public string databaseColumnToSort { get; set; }
        public string sortingOrder { get; set; }
    }
}
