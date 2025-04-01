using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Student_Management.Model
{
    public class StudentModel : INotifyPropertyChanged
    {
        private string _id;
        private string _rollNo;
        private string _name;
        private int _yearId;
        private string _yearLevel;
        private string _yearType;
        private int _semester;
        private string _phone;
        private string _address;
        private string _profilePicture;
        private ImageSource _profileImageSource;
        private bool _isPresent;
        private string _notes;
        private ObservableCollection<ClassAttendanceModel> _classAttendance;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string RollNo
        {
            get => _rollNo;
            set
            {
                _rollNo = value;
                OnPropertyChanged(nameof(RollNo));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public int Semester
        {
            get => _semester;
            set
            {
                _semester = value;
                OnPropertyChanged(nameof(Semester));
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string ProfilePicture
        {
            get => _profilePicture;
            set
            {
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged(nameof(ProfileImageSource));
            }
        }

        public bool IsPresent
        {
            get => _isPresent;
            set
            {
                _isPresent = value;
                OnPropertyChanged(nameof(IsPresent));
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public ObservableCollection<ClassAttendanceModel> ClassAttendance
        {
            get => _classAttendance;
            set
            {
                _classAttendance = value;
                OnPropertyChanged(nameof(ClassAttendance));
            }
        }

        public StudentModel()
        {
            ClassAttendance = new ObservableCollection<ClassAttendanceModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
