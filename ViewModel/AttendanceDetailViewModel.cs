using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Student_Management.ViewModel
{
    public class AttendanceDetailViewModel : INotifyPropertyChanged
    {
        private DateTime _selectedDate;
        private int _selectedYear;
        private int _selectedSemester;
        private int _selectedClass;
        private ObservableCollection<int> _years;
        private ObservableCollection<int> _semesters;
        private ObservableCollection<Class> _classes;
        private ObservableCollection<Student> _allStudents; // Store all students for filtering
        private ObservableCollection<Student> _students;
        private string _searchText;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClasses(); // Load classes when year changes
            }
        }

        public int SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                LoadClasses(); // Load classes when semester changes
            }
        }

        public int SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                LoadStudents(); // Load students when class changes
            }
        }

        public ObservableCollection<int> Years => new ObservableCollection<int> { 2021, 2022, 2023, 2024, 2025, 2026 };
        public ObservableCollection<int> Semesters => new ObservableCollection<int> { 1, 2, 3, 4, 5, 6 }; // Example semesters
        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                OnPropertyChanged(nameof(Classes));
            }
        }

        public ObservableCollection<Student> Students
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
                FilterStudents(); // Filter students when search text changes
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public AttendanceDetailViewModel()
        {
            ConfirmCommand = new RelayCommand(ConfirmAttendance);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            LoadClasses();
            LoadStudents(); // Load students initially
        }

        private void LoadClasses()
        {
            // Load classes based on selected year and semester
            // This is a placeholder for actual data retrieval logic
            Classes = new ObservableCollection<Class>
            {
                new Class { ClassID = 1, ClassName = "Class A", Year = SelectedYear, Semester = SelectedSemester },
                new Class { ClassID = 2, ClassName = "Class B", Year = SelectedYear, Semester = SelectedSemester }
            };
        }

        private void LoadStudents()
        {
            // Load all students from the database or service
            _allStudents = new ObservableCollection<Student>
            {
                new Student { ID = 1, Name = "John Doe", IsPresent = false },
                new Student { ID = 2, Name = "Jane Smith", IsPresent = false }
                // Add more students as needed
            };
            Students = new ObservableCollection<Student>(_allStudents); // Initialize with all students
        }

        private void FilterStudents()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Students = new ObservableCollection<Student>(_allStudents); // Show all if search is empty
            }
            else
            {
                var filteredStudents = _allStudents.Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                Students = new ObservableCollection<Student>(filteredStudents); // Update the list with filtered results
            }
        }

        private void ClearSearch()
        {
            SearchText = string.Empty; // Clear the search text
        }

        private void ConfirmAttendance()
        {
            // Logic to save attendance
            foreach (var student in Students)
            {
                // Save attendance for each student
                // Example: SaveAttendance(student.ID, SelectedClass, SelectedDate, student.IsPresent);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Class
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsPresent { get; set; }
    }
} 