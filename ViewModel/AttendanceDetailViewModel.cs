using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using Student_Management.View;
using Student_Management.Repository;
using Student_Management.Model;

namespace Student_Management.ViewModel
{
    public class AttendanceDetailViewModel : INotifyPropertyChanged
    {
        private readonly StudentRepository _repository;
        private string _selectedYear;
        private string _selectedSemester;
        private ObservableCollection<string> _years;
        private ObservableCollection<string> _semesters;
        private ObservableCollection<StudentModel> _students;
        private ObservableCollection<ClassModel> _classes;
        private ClassModel _selectedClass;
        private DateTime _selectedDate;
        private string _searchText;
        private Visibility _viewVisibility = Visibility.Visible;
        public event PropertyChangedEventHandler PropertyChanged;

        public Visibility ViewVisibility
        {
            get => _viewVisibility;
            set
            {
                _viewVisibility = value;
                OnPropertyChanged(nameof(ViewVisibility));
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClasses(); // Reload classes when year changes
            }
        }

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                LoadClasses(); // Reload classes when semester changes
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
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

        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                LoadStudents();
            }
        }

        public ObservableCollection<string> Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }

        public ObservableCollection<string> Semesters
        {
            get => _semesters;
            set
            {
                _semesters = value;
                OnPropertyChanged(nameof(Semesters));
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

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterStudents();
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand BackCommand { get; }

        public AttendanceDetailViewModel()
        {
            _repository = new StudentRepository();
            _selectedDate = DateTime.Today;
            Classes = new ObservableCollection<ClassModel>();
            Students = new ObservableCollection<StudentModel>();

            // Initialize commands
            ConfirmCommand = new RelayCommand(ConfirmAttendance);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            BackCommand = new RelayCommand(GoBack);

            // Load initial data
            LoadYears();
            LoadSemesters();
        }

        private void LoadYears()
        {
            Years = _repository.GetAllYears();
        }

        private void LoadSemesters()
        {
            Semesters = _repository.GetAllSemesters();
        }

        private void LoadClasses()
        {
            if (!string.IsNullOrEmpty(SelectedYear) && !string.IsNullOrEmpty(SelectedSemester))
            {
                Classes = _repository.GetClassesByYearAndSemester(SelectedYear, SelectedSemester);
            }
            else
            {
                Classes = new ObservableCollection<ClassModel>();
            }
        }

        private void LoadStudents()
        {
            if (!string.IsNullOrEmpty(SelectedYear) &&
                !string.IsNullOrEmpty(SelectedSemester) &&
                SelectedClass != null)
            {
                int year = ConvertYearTextToNumber(SelectedYear);
                int semester = SelectedSemester.StartsWith("First") ? 1 : 2;

                // Get students with their attendance status for the selected date
                Students = _repository.GetStudentsByYearAndSemester(year, semester, SelectedDate);

                // Filter students by class if needed
                if (Students != null && Students.Any() && SelectedClass != null)
                {
                    var filteredStudents = Students.Where(s =>
                        s.Year == SelectedClass.Year.ToString() &&
                        s.Semester == SelectedClass.Semester.ToString()
                    ).ToList();

                    Students = new ObservableCollection<StudentModel>(filteredStudents);
                }
            }
            else
            {
                Students = new ObservableCollection<StudentModel>();
            }
        }

        private int ConvertYearTextToNumber(string yearText)
        {
            switch (yearText)
            {
                case "First Year": return 1;
                case "Second Year": return 2;
                case "Third Year": return 3;
                case "Fourth Year": return 4;
                case "First Year (Honors)": return 5;
                case "Second Year (Honors)": return 6;
                case "Third Year (Honors)": return 7;
                case "Fourth Year (Honors)": return 8;
                default: return 1;
            }
        }

        private void FilterStudents()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadStudents();
            }
            else
            {
                var filteredStudents = _students
                    .Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Students = new ObservableCollection<StudentModel>(filteredStudents);
            }
        }

        private void ClearSearch()
        {
            SearchText = string.Empty;
        }

        private void ConfirmAttendance()
        {
            try
            {
                if (SelectedClass == null)
                {
                    MessageBox.Show("Please select a class first.", "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                foreach (var student in Students)
                {
                    string status = student.IsPresent ? "Present" : "Absent";
                    _repository.SaveAttendance(
                        Convert.ToInt32(student.Id),
                        SelectedClass.ClassID, // Add the ClassID
                        SelectedDate,
                        status
                    );
                }

                MessageBox.Show("Attendance has been saved successfully!", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving attendance: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GoBack()
        {
            ViewVisibility = Visibility.Collapsed;
        }
    }
}