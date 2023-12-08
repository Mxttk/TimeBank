using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace POE_PART_3_ST0084433.Models
{
    // self study model class
    public class SelfStudy
    {
        [Key]
        public int ID { get; set; }

        public string userName { get; set; }

        public string moduleCode { get; set; }

        public int selfStudyHours { get; set; }

        public int studyHoursRemaining { get; set; }

        [DataType(DataType.Date)] 
        public DateTime studyDate { get; set; }

        public string studyDuration { get; set; }
    }
}

