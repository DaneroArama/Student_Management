using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Management.Model
{
    public class ClassModel
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }

        public override string ToString()
        {
            return ClassName;
        }
    }
}
