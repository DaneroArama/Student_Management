using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Student_Management.Model;
using Student_Management.Repository;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Student_Management.ViewModel
{
    public class MarksViewModel : ViewModelBase
    {
        private readonly MarksRepository _repository;
        private readonly StudentRepository _studentRepository;
        
        #region Properties
        private YearConfig _selectedYear;
        public YearConfig SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClasses();
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

        private ObservableCollection<YearConfig> _years;
        public ObservableCollection<YearConfig> Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
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

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                LoadStudentsForClass();
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

        private ObservableCollection<StudentModel> _students;
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }
        
        private ObservableCollection<MarkModel> _marksRecords;
        public ObservableCollection<MarkModel> MarksRecords
        {
            get => _marksRecords;
            set
            {
                _marksRecords = value;
                OnPropertyChanged(nameof(MarksRecords));
            }
        }
        
        private ObservableCollection<PerformanceModel> _performanceRecords;
        public ObservableCollection<PerformanceModel> PerformanceRecords
        {
            get => _performanceRecords;
            set
            {
                _performanceRecords = value;
                OnPropertyChanged(nameof(PerformanceRecords));
            }
        }
        
        private MarkModel _selectedMark;
        public MarkModel SelectedMark
        {
            get => _selectedMark;
            set
            {
                _selectedMark = value;
                OnPropertyChanged(nameof(SelectedMark));
                LoadMarkDetails();
            }
        }
        
        private int _selectedClassId;
        public int SelectedClassId
        {
            get => _selectedClassId;
            set
            {
                _selectedClassId = value;
                OnPropertyChanged(nameof(SelectedClassId));
                if (value > 0)
                {
                    LoadStudentsForClass();
                }
            }
        }
        
        private int _selectedStudentId;
        public int SelectedStudentId
        {
            get => _selectedStudentId;
            set
            {
                _selectedStudentId = value;
                OnPropertyChanged(nameof(SelectedStudentId));
            }
        }
        
        private string _selectedExamType;
        public string SelectedExamType
        {
            get => _selectedExamType;
            set
            {
                _selectedExamType = value;
                OnPropertyChanged(nameof(SelectedExamType));
            }
        }
        
        private DateTime _examDate = DateTime.Today;
        public DateTime ExamDate
        {
            get => _examDate;
            set
            {
                _examDate = value;
                OnPropertyChanged(nameof(ExamDate));
            }
        }
        
        private decimal _marksObtained;
        public decimal MarksObtained
        {
            get => _marksObtained;
            set
            {
                _marksObtained = value;
                OnPropertyChanged(nameof(MarksObtained));
                UpdatePercentage();
            }
        }
        
        private decimal _totalMarks = 100;
        public decimal TotalMarks
        {
            get => _totalMarks;
            set
            {
                _totalMarks = value;
                OnPropertyChanged(nameof(TotalMarks));
                UpdatePercentage();
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
        
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        
        // Analysis properties
        private int _selectedAnalysisClassId;
        public int SelectedAnalysisClassId
        {
            get => _selectedAnalysisClassId;
            set
            {
                _selectedAnalysisClassId = value;
                OnPropertyChanged(nameof(SelectedAnalysisClassId));
            }
        }
        
        private string _selectedAnalysisExamType;
        public string SelectedAnalysisExamType
        {
            get => _selectedAnalysisExamType;
            set
            {
                _selectedAnalysisExamType = value;
                OnPropertyChanged(nameof(SelectedAnalysisExamType));
            }
        }
        
        private DateTime? _analysisDate;
        public DateTime? AnalysisDate
        {
            get => _analysisDate;
            set
            {
                _analysisDate = value;
                OnPropertyChanged(nameof(AnalysisDate));
            }
        }
        
        private decimal _classAverage;
        public decimal ClassAverage
        {
            get => _classAverage;
            set
            {
                _classAverage = value;
                OnPropertyChanged(nameof(ClassAverage));
            }
        }
        
        private decimal _highestScore;
        public decimal HighestScore
        {
            get => _highestScore;
            set
            {
                _highestScore = value;
                OnPropertyChanged(nameof(HighestScore));
            }
        }
        
        private decimal _lowestScore;
        public decimal LowestScore
        {
            get => _lowestScore;
            set
            {
                _lowestScore = value;
                OnPropertyChanged(nameof(LowestScore));
            }
        }
        
        private decimal _passPercentage;
        public decimal PassPercentage
        {
            get => _passPercentage;
            set
            {
                _passPercentage = value;
                OnPropertyChanged(nameof(PassPercentage));
            }
        }
        
        private int _totalStudents;
        public int TotalStudents
        {
            get => _totalStudents;
            set
            {
                _totalStudents = value;
                OnPropertyChanged(nameof(TotalStudents));
            }
        }
        
        private int _studentsPassed;
        public int StudentsPassed
        {
            get => _studentsPassed;
            set
            {
                _studentsPassed = value;
                OnPropertyChanged(nameof(StudentsPassed));
            }
        }
        
        private decimal _percentage;
        public decimal Percentage
        {
            get => _percentage;
            set
            {
                _percentage = value;
                OnPropertyChanged(nameof(Percentage));
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

        private string _selectedYearType;
        public string SelectedYearType
        {
            get => _selectedYearType;
            set
            {
                _selectedYearType = value;
                OnPropertyChanged(nameof(SelectedYearType));
                LoadClasses();
            }
        }

        private ObservableCollection<string> _examTypes;
        public ObservableCollection<string> ExamTypes
        {
            get => _examTypes;
            set
            {
                _examTypes = value;
                OnPropertyChanged(nameof(ExamTypes));
            }
        }
        #endregion
        
        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand GenerateAnalysisCommand { get; private set; }
        #endregion
        
        public MarksViewModel()
        {
            _repository = new MarksRepository();
            _studentRepository = new StudentRepository();
            
            // Initialize Collections
            Years = new ObservableCollection<YearConfig>();
            Classes = new ObservableCollection<ClassModel>();
            Students = new ObservableCollection<StudentModel>();
            MarksRecords = new ObservableCollection<MarkModel>();
            PerformanceRecords = new ObservableCollection<PerformanceModel>();
            Semesters = new ObservableCollection<int> { 1, 2 };
            ExamTypes = new ObservableCollection<string>();

            // Initialize Commands
            SaveCommand = new RelayCommand(SaveMark);
            ClearCommand = new RelayCommand(ClearForm);
            SearchCommand = new RelayCommand(SearchMarks);
            RefreshCommand = new RelayCommand(LoadMarks);
            EditCommand = new RelayCommand<object>(EditMark);
            DeleteCommand = new RelayCommand<object>(DeleteMark);
            GenerateAnalysisCommand = new RelayCommand(GenerateAnalysis);

            // Set default semester
            _selectedSemester = 1;

            // Load Initial Data
            LoadYears();
            LoadExamTypes();
            
            // Add debug message
            System.Diagnostics.Debug.WriteLine("MarksViewModel initialized");
        }

        #region Data Loading Methods
        private void LoadYears()
        {
            try
            {
                var yearsList = _studentRepository.GetAllYearConfigurations();
                System.Diagnostics.Debug.WriteLine($"Retrieved {yearsList.Count} years from database");
                
                // Convert to list to allow modifications
                var years = yearsList.ToList();
                
                // Make sure each year has a YearType and set Honors for last two years
                for (int i = 0; i < years.Count; i++)
                {
                    var year = years[i];
                    System.Diagnostics.Debug.WriteLine($"Year ID: {year.YearId}, Level: {year.YearLevel}, Type: {year.YearType}");
                    
                    // Set Honors for the last two entries
                    if (i >= years.Count - 2)
                    {
                        year.YearType = "Honors";
                    }
                    else if (string.IsNullOrEmpty(year.YearType))
                    {
                        year.YearType = "Regular";
                    }
                }

                Years = new ObservableCollection<YearConfig>(years);
                
                if (Years.Count > 0)
                {
                    SelectedYear = Years[0];
                    System.Diagnostics.Debug.WriteLine($"Selected Year - Level: {SelectedYear.YearLevel}, Type: {SelectedYear.YearType}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading years: {ex.Message}");
                MessageBox.Show("Error loading years. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadClasses()
        {
            if (SelectedYear == null) return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading classes for Year: {SelectedYear.YearId}, Semester: {SelectedSemester}");
                
                // Get classes for selected year and semester
                var allClasses = _studentRepository.GetClassesForYear(SelectedYear.YearId);
                var filteredClasses = allClasses.Where(c => c.Semester == SelectedSemester).ToList();
                
                Classes.Clear();
                foreach (var cls in filteredClasses)
                {
                    Classes.Add(cls);
                }

                System.Diagnostics.Debug.WriteLine($"Found {Classes.Count} classes");

                if (Classes.Count > 0)
                {
                    SelectedClass = Classes[0];
                }
                else
                {
                    SelectedClass = null;
                    Students.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading classes: {ex.Message}");
                MessageBox.Show($"Error loading classes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Classes.Clear();
            }
        }

        private void LoadStudentsForClass()
        {
            if (SelectedYear == null || SelectedClass == null) 
            {
                Students.Clear();
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading students for Year: {SelectedYear.YearId}, Semester: {SelectedSemester}, Class: {SelectedClass.ClassID}");
                
                var students = _studentRepository.GetStudentsByYearAndSemester(
                    SelectedYear.YearId,
                    SelectedSemester,
                    DateTime.Today,
                    SelectedClass.ClassID);

                Students.Clear();
                foreach (var student in students)
                {
                    Students.Add(student);
                }

                System.Diagnostics.Debug.WriteLine($"Found {Students.Count} students");

                if (Students.Count == 0)
                {
                    MessageBox.Show($"No students found for {SelectedYear.YearLevel}, Semester {SelectedSemester}, Class {SelectedClass.ClassName}.",
                        "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Students.Clear();
            }
        }

        private void LoadExamTypes()
        {
            try
            {
                // Since there's no GetAllExamTypes in repository, we'll hardcode the exam types
                ExamTypes = new ObservableCollection<string>
                {
                    "First Semester",
                    "Second Semester",
                    "Tutorial",
                    "Practical"
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading exam types: {ex.Message}");
                MessageBox.Show("Error loading exam types. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LoadMarks()
        {
            try
            {
                var marks = _repository.GetAllMarks();
                MarksRecords.Clear();
                foreach (var mark in marks)
                {
                    MarksRecords.Add(mark);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading marks: {ex.Message}");
                MessageBox.Show("Error loading marks. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMarkDetails()
        {
            if (SelectedMark == null) return;

            SelectedClassId = SelectedMark.ClassID;
            SelectedStudentId = SelectedMark.StudentID;
            SelectedExamType = SelectedMark.ExamType;
            ExamDate = SelectedMark.ExamDate;
            MarksObtained = SelectedMark.MarksObtained;
            TotalMarks = SelectedMark.TotalMarks;
            Remarks = SelectedMark.Remarks;
        }
        #endregion

        #region Command Methods
        private bool CanSaveMark()
        {
            return SelectedStudentId > 0 && 
                   SelectedClassId > 0 && 
                   !string.IsNullOrEmpty(SelectedExamType) && 
                   MarksObtained >= 0 && 
                   TotalMarks > 0 &&
                   MarksObtained <= TotalMarks;
        }

        private void SaveMark()
        {
            try
            {
                if (SelectedClass == null)
                {
                    MessageBox.Show("Please select a class first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SelectedStudentId <= 0)
                {
                    MessageBox.Show("Please select a student first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check for duplicate marks - now ignoring date
                var existingMark = MarksRecords.FirstOrDefault(m => 
                    m.StudentID == SelectedStudentId &&
                    m.ClassID == SelectedClass.ClassID &&
                    m.ExamType == SelectedExamType &&
                    m.MarkID != (SelectedMark?.MarkID ?? 0)); // Exclude current mark when editing

                if (existingMark != null)
                {
                    var message = $"A mark already exists for this student in {SelectedClass.ClassName} for {SelectedExamType}.\n" +
                                $"Existing mark: {existingMark.MarksObtained}/{existingMark.TotalMarks} on {existingMark.ExamDate:dd/MM/yyyy}\n\n" +
                                "Please edit the existing mark instead of creating a new one.";
                    
                    MessageBox.Show(message, "Duplicate Mark", MessageBoxButton.OK, MessageBoxImage.Warning);
                    
                    // Optionally, load the existing mark for editing
                    SelectedMark = existingMark;
                    LoadMarkDetails();
                    return;
                }

                var mark = new MarkModel
                {
                    StudentID = SelectedStudentId,
                    ClassID = SelectedClass.ClassID,
                    ExamType = SelectedExamType,
                    ExamDate = ExamDate,
                    MarksObtained = MarksObtained,
                    TotalMarks = TotalMarks,
                    Remarks = Remarks
                };

                System.Diagnostics.Debug.WriteLine($"Saving mark for Student ID: {mark.StudentID}, Class ID: {mark.ClassID}");

                if (SelectedMark != null)
                {
                    mark.MarkID = SelectedMark.MarkID;
                    _repository.UpdateMark(mark);
                }
                else
                {
                    _repository.AddMark(mark);
                }

                LoadMarks();
                ClearForm();
                MessageBox.Show("Marks saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving marks: {ex.Message}");
                MessageBox.Show($"Error saving marks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            SelectedMark = null;
            SelectedStudentId = 0;
            SelectedExamType = null;
            ExamDate = DateTime.Today;
            MarksObtained = 0;
            TotalMarks = 100;
            Remarks = string.Empty;
        }

        private void SearchMarks()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadMarks();
                return;
            }

            try
            {
                var searchResults = _repository.SearchMarks(SearchText);
                MarksRecords.Clear();
                foreach (var mark in searchResults)
                {
                    MarksRecords.Add(mark);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error searching marks: {ex.Message}");
                MessageBox.Show("Error searching marks. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditMark(object parameter)
        {
            if (parameter is MarkModel mark)
            {
                SelectedMark = mark;
                LoadMarkDetails();
            }
        }

        private void DeleteMark(object parameter)
        {
            if (parameter is MarkModel mark)
            {
                var result = MessageBox.Show("Are you sure you want to delete this mark?", "Confirm Delete", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.DeleteMark(mark.MarkID);
                        LoadMarks();
                        MessageBox.Show("Mark deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error deleting mark: {ex.Message}");
                        MessageBox.Show("Error deleting mark. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void GenerateAnalysis()
        {
            if (SelectedAnalysisClassId <= 0)
            {
                MessageBox.Show("Please select a class for analysis.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var analysisData = _repository.GetMarksAnalytics(
                    SelectedAnalysisClassId,
                    null,  // No subject name
                    SelectedAnalysisExamType,
                    AnalysisDate);

                // Update statistics
                ClassAverage = Convert.ToDecimal(analysisData["ClassAverage"]);
                HighestScore = Convert.ToDecimal(analysisData["HighestScore"]);
                LowestScore = Convert.ToDecimal(analysisData["LowestScore"]);
                PassPercentage = Convert.ToDecimal(analysisData["PassPercentage"]);
                TotalStudents = Convert.ToInt32(analysisData["TotalStudents"]);
                StudentsPassed = Convert.ToInt32(analysisData["StudentsPassed"]);

                // Update performance records
                LoadPerformanceRecords(SelectedAnalysisClassId, SelectedAnalysisExamType, AnalysisDate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating analysis: {ex.Message}");
                MessageBox.Show("Error generating analysis. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void LoadPerformanceRecords(int classId, string examType, DateTime? examDate)
        {
            try
            {
                var marks = _repository.GetMarksByClassId(classId);
                
                if (!string.IsNullOrEmpty(examType))
                {
                    marks = new ObservableCollection<MarkModel>(
                        marks.Where(m => m.ExamType == examType));
                }
                
                if (examDate.HasValue)
                {
                    marks = new ObservableCollection<MarkModel>(
                        marks.Where(m => m.ExamDate.Date == examDate.Value.Date));
                }
                
                PerformanceRecords.Clear();
                
                // Group marks by student
                var studentGroups = marks.GroupBy(m => m.StudentID);
                
                foreach (var group in studentGroups)
                {
                    var studentMarks = group.ToList();
                    if (studentMarks.Count == 0) continue;
                    
                    var student = studentMarks.First();
                    var totalObtained = studentMarks.Sum(m => m.MarksObtained);
                    var totalPossible = studentMarks.Sum(m => m.TotalMarks);
                    
                    PerformanceRecords.Add(new PerformanceModel
                    {
                        StudentID = student.StudentID,
                        StudentName = student.StudentName,
                        MarksObtained = totalObtained,
                        TotalMarks = totalPossible
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading performance records: {ex.Message}");
                MessageBox.Show("Error loading performance records. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePercentage()
        {
            if (TotalMarks > 0)
            {
                Percentage = Math.Round((MarksObtained / TotalMarks) * 100, 2);
            }
        }
    }
}