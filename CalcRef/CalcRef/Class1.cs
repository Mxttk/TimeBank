using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcRef
{
    public class Class1
    {
        // dll class used to calculate weekly study hours
        public static int Calculation(int credits, int totalWeeks, int weeklyClassHours)
        {
            // calculates weeklyhours required
            int weeklyStudyHours = ((credits * 10) / totalWeeks) - weeklyClassHours;
            return weeklyStudyHours;
        }

        // calc temp variables
        public static int creditsTemp { get; set; }
        public static int weeksTemp { get; set; }
        public static int hoursTemp { get; set; }

        // display temp variables 
        public static string moduleCode { get; set; }
        public static int selfStudyHours { get; set; }
        public static int studyHoursRemaining { get; set; }
        public static string studyDate { get; set; }
        public static string studyDuration { get; set; }


    }
}
