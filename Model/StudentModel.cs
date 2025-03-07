using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Student_Management.Model
{
    public class StudentModel
    {
        public string Id { get; set; }
        public string RollNo { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Semester { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public ImageSource ProfileImageSource { get; set; }
        public bool IsPresent { get; set; }
    }
}
