using _20104681JoshMkhariPOE.Logic;
using System;
using System.Collections.Generic;

namespace _20104681JoshMkhariPOE.Models
{
    public class ViewCalendarData
    {
        public String userName;
        public IList<StoredDates> calendar;
    }

    public class DoubleList
    {
        public List<string> currentCode;

        public List<int> currentHours;
    }


    public class GraphViewData
    {
        public String userName;
        public List<String> dates;
        public List<codeDays> data;
    }


    public class codeDays
    {
        public string name;
        public List<int> data;
    }

    //public class CalendarData
    //{
    //    public string ModuleCode { get; set; }
    //    public int remainingHours { get; set; }


    //}
}