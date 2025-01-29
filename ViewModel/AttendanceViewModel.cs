using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Student_Management.Model;
using System.ComponentModel;

namespace Student_Management.ViewModel
{
    public class AttendanceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<AttendanceModel> _attendanceRecords;
        private AttendanceModel _selectedAttendance;
        private ObservableCollection<DateTime> _holidays;
        private DateTime _selectedDate;

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

        public ICommand MarkAttendanceCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AttendanceViewModel()
        {
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
    }
}
