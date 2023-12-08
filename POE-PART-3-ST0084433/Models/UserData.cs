using System;
using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models
{
    // User Data model class
    public class UserData
    {
        public int ID { get; set; }
        public string userName { get; set; }
        public string moduleCode { get; set; }
        public string moduleName { get; set; }
        public int numberOfCredits { get; set; }
        public int classHours { get; set; }
        public string semesterName { get; set; }
        public int semesterDuration { get; set; }

        [DataType(DataType.Date)]
        public DateTime semesterDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime dateToStudy { get; set; }
    }
}
