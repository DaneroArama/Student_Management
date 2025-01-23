using System;

namespace Student_Management.Model
{
    public class AttendanceModel
    {
        public int AttendanceID { get; set; }
        public int StudentID { get; set; }
        public int ClassID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; } // 'Present', 'Absent', 'Excused'
        public string Remarks { get; set; }
    }
} 