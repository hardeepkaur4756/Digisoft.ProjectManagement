using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Digisoft.ProjectManagement.Utility
{
    public class StaticMethods
    {
        public static int GetAverage(int numberOfUsers, int Total)
        {
            var toReturn = 0;
            if (numberOfUsers != 0 && Total != 0)
                toReturn = Total / numberOfUsers;
            return toReturn;
        }
        //get the List of days of the month
        public static List<string> GetListOfDays(DateTime date)
        {
            var toReturn = new List<string>();
            var month = date.Month;
            var year = date.Year;

            int days = DateTime.DaysInMonth(year, month);

            for (var i = 1; i <= days; i++)
            {
                toReturn.Add(string.Format("{0}", i));
            }
            return toReturn;
        }

        public static List<string> GetListOfMonths()
        {
            string[] toReturn = DateTimeFormatInfo.CurrentInfo.MonthNames;
            return toReturn.Take(toReturn.Length - 1).ToList();
        }

        public static bool isAdminOrHr()
        {
            if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("HR"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}