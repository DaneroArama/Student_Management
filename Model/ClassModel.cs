using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Student_Management.Model
{
    public class ClassModel : INotifyPropertyChanged
    {
        private int _classId;
        private string _className;
        private int _yearId;
        private int _semester;
        private string _yearLevel;
        private string _yearType;
        private ObservableCollection<StudentModel> _students;

        public int ClassID
        {
            get => _classId;
            set
            {
                _classId = value;
                OnPropertyChanged(nameof(ClassID));
            }
        }

        public string ClassName
        {
            get => _className;
            set
            {
                _className = value;
                OnPropertyChanged(nameof(ClassName));
            }
        }

        public int YearId
        {
            get => _yearId;
            set
            {
                _yearId = value;
                OnPropertyChanged(nameof(YearId));
            }
        }

        public int Semester
        {
            get => _semester;
            set
            {
                _semester = value;
                OnPropertyChanged(nameof(Semester));
            }
        }

        public string YearLevel
        {
            get => _yearLevel;
            set
            {
                _yearLevel = value;
                OnPropertyChanged(nameof(YearLevel));
            }
        }

        public string YearType
        {
            get => _yearType;
            set
            {
                _yearType = value;
                OnPropertyChanged(nameof(YearType));
            }
        }

        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return ClassName;
        }

         public ClassModel()
        {
            Students = new ObservableCollection<StudentModel>();
        }
    }
}
