using System.Collections.ObjectModel;
using System.Windows.Input;
using Student_Management.Model;

namespace Student_Management.ViewModel
{
    public class AttendanceViewModel : ViewModelBase
    {
        private ObservableCollection<AttendanceModel> _attendanceRecords;
        private AttendanceModel _selectedAttendance;

        public ObservableCollection<AttendanceModel> AttendanceRecords
        {
            get => _attendanceRecords;
            set
            {
                _attendanceRecords = value;
                OnPropertyChanged(nameof(AttendanceRecords));
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

        public ICommand MarkAttendanceCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public AttendanceViewModel()
        {
            AttendanceRecords = new ObservableCollection<AttendanceModel>();
            MarkAttendanceCommand = new RelayCommand(MarkAttendance);
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        private void MarkAttendance()
        {
            // Logic to mark attendance for the selected day
        }

        private void SaveChanges()
        {
            // Logic to save attendance changes to the database
        }

        public void LoadAttendanceForDate(DateTime date)
        {
            // Logic to load attendance records for the specified date
        }
    }
}
