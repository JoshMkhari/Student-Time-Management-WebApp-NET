using System;
using System.Collections.Generic;

namespace _20104681JoshMkhariPOE.Models
{
    public class ViewModuleData
    {
        public String userName { get; set; }

        public IList<ModuleData> modules { get; set; }
    }

    public class ModuleData
    {
        public string ModuleCode { get; set; }
        public int NumberOfCredits { get; set; }
        public int ClassHoursPerWeek { get; set; }
        public int SelfStudyHours { get; set; }
    }
}