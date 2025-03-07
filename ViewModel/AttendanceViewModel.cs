using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Student_Management.Model;
using System.ComponentModel;
using System.Linq;
using Student_Management.Repository;

namespace Student_Management.ViewModel
{
    public class AttendanceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<AttendanceModel> _attendanceRecords;
        private AttendanceModel _selectedAttendance;
        private ObservableCollection<DateTime> _holidays;
        private DateTime _selectedDate;
        private ObservableCollection<ClassModel> _classes;
        private string _selectedYear;
        private string _selectedSemester;
        private ClassModel _selectedClass;
        private readonly StudentRepository _repository;

        public ObservableCollection<AttendanceModel> AttendanceRecords
        {
            get => _attendanceRecords;
            set
            {
                _attendanceRecords = value;
                OnPropertyChanged(nameof(AttendanceRecords));
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadAttendanceForDate(value); // Optional: Load attendance for the new date
            }
        }

        public AttendanceModel SelectedAttendance
        {
            get => _selectedAttendance;
            set
            {
                _selectedAttendance = value;
                OnPropertyChanged(nameof(SelectedAttendance));
            }
        }

        public ObservableCollection<DateTime> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }

        public ObservableCollection<ClassModel> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                OnPropertyChanged(nameof(Classes));
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClassesForYearAndSemester(); // Reload classes when year changes
            }
        }

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                LoadClassesForYearAndSemester(); // Reload classes when semester changes
            }
        }

        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                // Load students or perform other actions when class is selected
            }
        }

        public ICommand MarkAttendanceCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AttendanceViewModel()
        {
            _repository = new StudentRepository();
            Classes = new ObservableCollection<ClassModel>();
            
            // Initialize with default values if needed
            SelectedYear = "First Year";
            SelectedSemester = "First Semester";
            
            // Initial load of classes
            LoadClassesForYearAndSemester();
            
            // Initialize with today's date
            SelectedDate = DateTime.Today;
            
            AttendanceRecords = new ObservableCollection<AttendanceModel>();
            Holidays = new ObservableCollection<DateTime>();
            MarkAttendanceCommand = new RelayCommand(MarkAttendance);
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        private void MarkAttendance()
        {
            // Logic to mark attendance for the selected day
        }

        public void SaveChanges()
        {
            // Logic to save attendance changes to the database
            // After saving, you may want to refresh the attendance records
        }

        public void LoadAttendanceForDate(DateTime date)
        {
            // Logic to load attendance records for the specified date
            // This should populate the AttendanceRecords collection
        }

        public void ToggleHoliday(DateTime date)
        {
            if (Holidays.Contains(date))
            {
                Holidays.Remove(date);
            }
            else
            {
                Holidays.Add(date);
            }
        }

        private void LoadClassesForYearAndSemester()
        {
            if (string.IsNullOrEmpty(SelectedYear) || string.IsNullOrEmpty(SelectedSemester))
                return;

            Classes = _repository.GetClassesByYearAndSemester(SelectedYear, SelectedSemester);
            
            // If no class is selected or the previously selected class isn't in the new list
            if (Classes.Count > 0 && (SelectedClass == null || !Classes.Contains(SelectedClass)))
            {
                SelectedClass = Classes.FirstOrDefault();
            }
        }
    }
}
