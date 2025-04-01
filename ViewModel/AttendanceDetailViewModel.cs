using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Student_Management.Model;
using Student_Management.View;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Student_Management.Repository;
using Student_Management.ViewModel;

namespace Student_Management.ViewModel
{
    public class AttendanceDetailViewModel : ViewModelBase
    {
        private readonly Action _navigateBack;
        private readonly StudentRepository _studentRepository;
        private readonly AttendanceRepository _attendanceRepository;

        #region Properties
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadStudents();
            }
        }

        private ObservableCollection<YearConfig> _yearLevels;
        public ObservableCollection<YearConfig> YearLevels
        {
            get => _yearLevels;
            set
            {
                _yearLevels = value;
                OnPropertyChanged(nameof(YearLevels));
            }
        }

        // Make sure the SelectedYearLevel and SelectedSemester properties have proper property change notifications
        private YearConfig _selectedYearLevel;
        public YearConfig SelectedYearLevel
        {
            get => _selectedYearLevel;
            set
            {
                if (_selectedYearLevel != value)
                {
                    _selectedYearLevel = value;
                    OnPropertyChanged(nameof(SelectedYearLevel));
                    LoadClasses();
                }
            }
        }

        private ObservableCollection<string> _yearTypes;
        public ObservableCollection<string> YearTypes
        {
            get => _yearTypes;
            set
            {
                _yearTypes = value;
                OnPropertyChanged(nameof(YearTypes));
            }
        }

        private ObservableCollection<int> _semesters;
        public ObservableCollection<int> Semesters
        {
            get => _semesters;
            set
            {
                _semesters = value;
                OnPropertyChanged(nameof(Semesters));
            }
        }

        private int _selectedSemester;
        public int SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                LoadClasses();
            }
        }

        private ObservableCollection<ClassModel> _classes;
        public ObservableCollection<ClassModel> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                OnPropertyChanged(nameof(Classes));
            }
        }

        private ObservableCollection<StudentAttendanceModel> _students;
        public ObservableCollection<StudentAttendanceModel> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }
        private string _searchText = string.Empty;
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

        private ObservableCollection<StudentAttendanceModel> _filteredStudents;
        public ObservableCollection<StudentAttendanceModel> FilteredStudents
        {
            get => _filteredStudents;
            set
            {
                _filteredStudents = value;
                OnPropertyChanged(nameof(FilteredStudents));
            }
        }
        #endregion

        #region Commands
        public ICommand SaveAttendanceCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand ClearSearchCommand { get; private set; }
        #endregion

        public AttendanceDetailViewModel(Action navigateBack)
        {
            _navigateBack = navigateBack;
            _studentRepository = new StudentRepository();
            _attendanceRepository = new AttendanceRepository();
            
            // Initialize collections
            CurrentDate = DateTime.Now;
            SelectedDate = DateTime.Now;
            YearLevels = new ObservableCollection<YearConfig>();
            Semesters = new ObservableCollection<int> { 1, 2 };
            Classes = new ObservableCollection<ClassModel>();
            Students = new ObservableCollection<StudentAttendanceModel>();
            FilteredStudents = new ObservableCollection<StudentAttendanceModel>();

            // Initialize commands
            SaveAttendanceCommand = new RelayCommand(SaveAttendance, CanSaveAttendance);
            BackCommand = new RelayCommand(ExecuteNavigateBack);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            
            // Load data immediately
            LoadYearLevels();
            
            // If we have year levels, select the first one
            if (YearLevels.Count > 0)
            {
                SelectedYearLevel = YearLevels[0];
                
                // If we have semesters, select the first one
                if (Semesters.Count > 0)
                {
                    SelectedSemester = Semesters[0];
                }
            }
        }

        private void ExecuteNavigateBack()
        {
            _navigateBack?.Invoke();
        }

        private void LoadYearLevels()
        {
            try
            {
                // Load from database using the same method as MarksViewModel
                var yearsList = _studentRepository.GetAllYearConfigurations();
                
                YearLevels.Clear();
                foreach (var year in yearsList)
                {
                    YearLevels.Add(year);
                    // Debug output to check what's being loaded - include DisplayName
                    System.Diagnostics.Debug.WriteLine($"Added year: {year.YearId} - {year.YearLevel} - {year.YearType} - DisplayName: {year.DisplayName}");
                }
                
                System.Diagnostics.Debug.WriteLine($"Loaded {YearLevels.Count} year levels");
                
                // If we have years but the dropdown is still empty, check if DisplayName is null
                if (YearLevels.Count > 0)
                {
                    foreach (var year in YearLevels)
                    {
                        if (string.IsNullOrEmpty(year.DisplayName))
                        {
                            System.Diagnostics.Debug.WriteLine($"Warning: Year {year.YearId} has null or empty DisplayName");
                            // Try to fix it by setting DisplayName explicitly
                            if (!string.IsNullOrEmpty(year.YearLevel) && !string.IsNullOrEmpty(year.YearType))
                            {
                                // This is a workaround if the property doesn't auto-update
                                var displayName = $"{year.YearLevel} ({year.YearType})";
                                System.Diagnostics.Debug.WriteLine($"Setting DisplayName to: {displayName}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading year levels: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSemesters()
        {
            // Semesters are fixed as 1 and 2
            Semesters.Clear();
            Semesters.Add(1);
            Semesters.Add(2);
        }

        private void LoadClasses()
        {
            if (SelectedYearLevel == null) return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading classes for Year: {SelectedYearLevel.YearId} ({SelectedYearLevel.YearLevel}), Semester: {SelectedSemester}");
                
                // Get classes for selected year and semester
                var allClasses = _studentRepository.GetClassesForYear(SelectedYearLevel.YearId);
                var filteredClasses = allClasses.Where(c => c.Semester == SelectedSemester).ToList();
                
                // Clear and update the Classes collection
                Classes.Clear();
                foreach (var cls in filteredClasses)
                {
                    Classes.Add(cls);
                    System.Diagnostics.Debug.WriteLine($"Added class: {cls.ClassName} (ID: {cls.ClassID})");
                }

                System.Diagnostics.Debug.WriteLine($"Found {Classes.Count} classes for {SelectedYearLevel.YearLevel} year, semester {SelectedSemester}");
                
                // Load students after classes are loaded
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading classes: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStudents()
        {
            if (SelectedYearLevel == null || Classes.Count == 0) return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading students for Year: {SelectedYearLevel.YearLevel}, Semester: {SelectedSemester}, Date: {SelectedDate:yyyy-MM-dd}");
                
                Students.Clear();
                
                // Get students for the selected year and semester
                var studentsInYear = _studentRepository.GetStudentsByYearAndSemester(
                    SelectedYearLevel.YearId, 
                    SelectedSemester,
                    SelectedDate,
                    0);  // Pass 0 or appropriate class ID if needed
                
                System.Diagnostics.Debug.WriteLine($"Found {studentsInYear.Count()} students");
                
                foreach (var student in studentsInYear)
                {
                    var studentAttendance = new StudentAttendanceModel
                    {
                        StudentId = int.Parse(student.Id),
                        Name = student.Name,
                        RollNo = student.RollNo,
                        ClassAttendance = new ObservableCollection<ClassAttendance>()
                    };
                    
                    // Add attendance status for each class
                    foreach (var cls in Classes)
                    {
                        System.Diagnostics.Debug.WriteLine($"Adding attendance for class: {cls.ClassName}");
                        
                        // Check if attendance record exists
                        bool isPresent = _attendanceRepository.CheckAttendance(
                            studentAttendance.StudentId,
                            cls.ClassID,
                            SelectedDate);
                        
                        studentAttendance.ClassAttendance.Add(new ClassAttendance
                        {
                            ClassId = cls.ClassID,
                            ClassName = cls.ClassName,
                            IsPresent = isPresent
                        });
                    }
                    
                    Students.Add(studentAttendance);
                }
                
                System.Diagnostics.Debug.WriteLine($"Loaded {Students.Count} students with {Classes.Count} classes each");
                OnPropertyChanged(nameof(Students)); // Ensure UI is notified
                
                // Apply initial filtering
                FilterStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSaveAttendance()
        {
            return SelectedYearLevel != null && 
                   SelectedSemester > 0 && 
                   Classes.Any() && 
                   Students.Any();
        }

        private void SaveAttendance()
        {
            try
            {
                // Save attendance records to database
                foreach (var student in Students)
                {
                    foreach (var classAttendance in student.ClassAttendance)
                    {
                        _attendanceRepository.SaveAttendance(
                            student.StudentId,
                            classAttendance.ClassId,
                            SelectedDate,
                            classAttendance.IsPresent ? "Present" : "Absent");
                    }
                }
                
                MessageBox.Show("Attendance saved successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                _navigateBack?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving attendance: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        // Add these methods after the SaveAttendance method
        
        private void FilterStudents()
        {
            if (Students == null) return;
        
            FilteredStudents = new ObservableCollection<StudentAttendanceModel>();
        
            // If search text is empty, show all students
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var student in Students)
                {
                    FilteredStudents.Add(student);
                }
            }
            else
            {
                // Filter students based on search text (case-insensitive)
                string searchLower = SearchText.ToLower();
                foreach (var student in Students)
                {
                    if (student.Name?.ToLower().Contains(searchLower) == true || 
                        student.RollNo?.ToLower().Contains(searchLower) == true)
                    {
                        FilteredStudents.Add(student);
                    }
                }
            }
        
            System.Diagnostics.Debug.WriteLine($"Filtered students: {FilteredStudents.Count} of {Students.Count}");
            OnPropertyChanged(nameof(FilteredStudents));
        }
        
        private void ClearSearch()
        {
            SearchText = string.Empty;
            // FilterStudents() will be called by the SearchText property setter
        }

        public void InitializeData()
        {
            // Load initial data
            LoadYearLevels();
            
            // If we have year levels, select the first one
            if (YearLevels.Count > 0 && SelectedYearLevel == null)
            {
                SelectedYearLevel = YearLevels[0];
                System.Diagnostics.Debug.WriteLine($"Setting SelectedYearLevel to: {SelectedYearLevel.YearLevel} ({SelectedYearLevel.YearType})");
                
                // If we have semesters, select the first one
                if (Semesters.Count > 0)
                {
                    SelectedSemester = Semesters[0];
                    System.Diagnostics.Debug.WriteLine($"Setting SelectedSemester to: {SelectedSemester}");
                }
            }
        }
    }

    

    public class StudentAttendanceModel : ViewModelBase
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string RollNo { get; set; }
        
        private ObservableCollection<ClassAttendance> _classAttendance;
        public ObservableCollection<ClassAttendance> ClassAttendance
        {
            get => _classAttendance;
            set
            {
                _classAttendance = value;
                OnPropertyChanged(nameof(ClassAttendance));
            }
        }

        private string _remarks;
        public string Remarks
        {
            get => _remarks;
            set
            {
                _remarks = value;
                OnPropertyChanged(nameof(Remarks));
            }
        }
    }
    
    public class ClassAttendance : ViewModelBase
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        
        private bool _isPresent;
        public bool IsPresent
        {
            get => _isPresent;
            set
            {
                _isPresent = value;
                OnPropertyChanged(nameof(IsPresent));
            }
        }
    }
}
