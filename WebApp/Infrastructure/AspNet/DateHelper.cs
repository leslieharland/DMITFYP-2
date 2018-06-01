using System;

namespace WebApp.Infrastructure.AspNet
{
    public class DateHelper
    {
        public static bool Between(DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }
    }
}