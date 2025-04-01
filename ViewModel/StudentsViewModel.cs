using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Microsoft.Win32;
using Student_Management.Model;
using Student_Management.Repository;
using Student_Management.View;
using Microsoft.Data.SqlClient;

namespace Student_Management.ViewModel
{
    /// <summary>
    /// ViewModel for managing the students list and edit views
    /// </summary>
    public class StudentsViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly StudentRepository _repository;
        private ObservableCollection<StudentModel> _allStudents;
        private ObservableCollection<StudentModel> _students;
        private ObservableCollection<YearConfig> _years;
        private YearConfig _selectedYear;
        private int _selectedSemester;
        private StudentModel _selectedStudent;
        private StudentModel _editingStudent;
        private bool _isEditMode;
        private string _searchText = string.Empty;
        private UserControl _editContent;
        private ObservableCollection<int> _semesterOptions;
        private string _selectedYearLevel = "All";
        private string _selectedYearType = "All";
        private ObservableCollection<string> _yearLevels;
        private ObservableCollection<string> _yearTypes;
        private bool _isLoading;

        // Commands
        private ICommand _editCommand;
        private ICommand _addCommand;
        private ICommand _clearSearchCommand;
        private ICommand _deleteCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of students displayed in the UI
        /// </summary>
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set
            {
                if (_students != value)
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
                }
            }
        }

        /// <summary>
        /// Collection of all year configurations
        /// </summary>
        public ObservableCollection<YearConfig> YearOptions
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged(nameof(YearOptions));
            }
        }

        /// <summary>
        /// Currently selected year
        /// </summary>
        public YearConfig SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                FilterStudents();
            }
        }

        /// <summary>
        /// Currently selected semester
        /// </summary>
        public int SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                FilterStudents();
            }
        }

        /// <summary>
        /// Currently selected student
        /// </summary>
        public StudentModel SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                OnPropertyChanged(nameof(IsStudentSelected));
            }
        }

        /// <summary>
        /// Student being edited
        /// </summary>
        public StudentModel EditingStudent
        {
            get => _editingStudent;
            set
            {
                _editingStudent = value;
                OnPropertyChanged(nameof(EditingStudent));
            }
        }

        /// <summary>
        /// Flag indicating if the view is in edit mode
        /// </summary>
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                Debug.WriteLine($"IsEditMode changing from {_isEditMode} to {value}");
                // Always update and notify, even if the value is the same
                _isEditMode = value;
                Debug.WriteLine("Raising property changed notifications");
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(IsListVisible));
                OnPropertyChanged(nameof(IsEditVisible));
                Debug.WriteLine($"After change - IsEditMode: {_isEditMode}, IsEditVisible: {IsEditVisible}, IsListVisible: {IsListVisible}");
            }
        }

        /// <summary>
        /// Flag indicating if the list view is visible
        /// </summary>
        public bool IsListVisible => !IsEditMode;

        /// <summary>
        /// Flag indicating if the edit view is visible
        /// </summary>
        public bool IsEditVisible => IsEditMode;

        /// <summary>
        /// Flag indicating if a student is selected
        /// </summary>
        public bool IsStudentSelected => SelectedStudent != null;

        /// <summary>
        /// Search text for filtering students
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterStudents();
                }
            }
        }

        /// <summary>
        /// Collection of available semester options
        /// </summary>
        public ObservableCollection<int> SemesterOptions
        {
            get => _semesterOptions;
            set
            {
                _semesterOptions = value;
                OnPropertyChanged(nameof(SemesterOptions));
            }
        }

        /// <summary>
        /// Collection of available year levels
        /// </summary>
        public ObservableCollection<string> YearLevels
        {
            get => _yearLevels;
            set
            {
                _yearLevels = value;
                OnPropertyChanged(nameof(YearLevels));
            }
        }

        /// <summary>
        /// Collection of available year types
        /// </summary>
        public ObservableCollection<string> YearTypes
        {
            get => _yearTypes;
            set
            {
                _yearTypes = value;
                OnPropertyChanged(nameof(YearTypes));
            }
        }

        /// <summary>
        /// Currently selected year level
        /// </summary>
        public string SelectedYearLevel
        {
            get => _selectedYearLevel;
            set
            {
                _selectedYearLevel = value;
                OnPropertyChanged(nameof(SelectedYearLevel));
                FilterStudents();
            }
        }

        /// <summary>
        /// Currently selected year type
        /// </summary>
        public string SelectedYearType
        {
            get => _selectedYearType;
            set
            {
                _selectedYearType = value;
                OnPropertyChanged(nameof(SelectedYearType));
                FilterStudents();
            }
        }

        /// <summary>
        /// Content for the edit view
        /// </summary>
        public UserControl EditContent
        {
            get => _editContent;
            set
            {
                Debug.WriteLine($"EditContent changing from {_editContent?.GetType().Name} to {value?.GetType().Name}");
                // Always update and notify, even if the value is the same
                _editContent = value;
                Debug.WriteLine("Raising EditContent property changed notification");
                OnPropertyChanged(nameof(EditContent));
            }
        }

        /// <summary>
        /// Flag indicating if data is currently loading
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to edit the selected student
        /// </summary>
        public ICommand EditCommand => _editCommand ??= new RelayCommand(() =>
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Debug.WriteLine($"Edit button clicked for student: {SelectedStudent.Name}");
            try
            {
                // Create a new instance of EditStudentViewModel
                var editViewModel = new EditStudentViewModel(SelectedStudent, () =>
                {
                    // Callback when editing is done
                    LoadStudents();
                    IsEditMode = false;
                });
                
                // Create EditStudentView with the new view model
                var editView = new EditStudentView();
                editView.DataContext = editViewModel;
                
                // Update UI properties in the right order
                EditContent = editView;
                IsEditMode = true;
                
                Debug.WriteLine("Switched to edit mode for existing student");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in EditCommand: {ex}");
                MessageBox.Show($"Error editing student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        /// <summary>
        /// Command to add a new student
        /// </summary>
        public ICommand AddCommand => _addCommand ??= new RelayCommand(() =>
        {
            Debug.WriteLine("Add button clicked");
            try
            {
                Debug.WriteLine("Creating new EditStudentView...");
                
                // Create a new instance of EditStudentViewModel for a new student
                var editViewModel = new EditStudentViewModel(null, () =>
                {
                    // Callback when adding is done
                    LoadStudents();
                    IsEditMode = false;
                });
                
                // Create EditStudentView with the new view model
                var editView = new EditStudentView();
                editView.DataContext = editViewModel;
                
                // Update UI properties in the right order
                EditContent = editView;
                IsEditMode = true;
                
                Debug.WriteLine("Switched to edit mode for new student");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddCommand: {ex}");
                MessageBox.Show($"Error adding student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        /// <summary>
        /// Command to clear the search text
        /// </summary>
        public ICommand ClearSearchCommand => _clearSearchCommand ??= new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(_searchText))
            {
                _searchText = string.Empty;
                OnPropertyChanged(nameof(SearchText));
                FilterStudents();
            }
        });

        /// <summary>
        /// Command to delete the selected student
        /// </summary>
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(() =>
        {
            if (SelectedStudent == null) 
                return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this student? This will also delete all attendance records.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.DeleteStudent(int.Parse(SelectedStudent.Id));
                        LoadStudents();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting student: {ex}");
                    MessageBox.Show($"Error deleting student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        });

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StudentsViewModel class
        /// </summary>
        public StudentsViewModel()
        {
            try
            {
                _repository = new StudentRepository();
                
                // Initialize collections
                _students = new ObservableCollection<StudentModel>();
                _allStudents = new ObservableCollection<StudentModel>();
                _yearLevels = new ObservableCollection<string> { "All" };
                _yearTypes = new ObservableCollection<string> { "All", "Regular", "Honors" };
                SemesterOptions = new ObservableCollection<int> { 0, 1, 2 }; // 0 represents "All"
                
                // Set default values
                _selectedSemester = 0; // Default to "All"
                _selectedYearLevel = "All";
                _selectedYearType = "All";
                _searchText = string.Empty;
                
                // Set default IsEditMode to ensure initial view state is correct
                _isEditMode = false;
                
                // Load data
                InitializeAsync();
                
                System.Diagnostics.Debug.WriteLine("StudentsViewModel initialized with YearTypes: " + string.Join(", ", _yearTypes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in StudentsViewModel constructor: {ex}");
                MessageBox.Show($"Error initializing view model: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Asynchronously initializes the view model
        /// </summary>
        private async void InitializeAsync()
        {
            try
            {
                IsLoading = true;
                await Task.Run(() =>
                {
                    LoadYears();
                    LoadStudents();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing view model: {ex}");
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Loads year configurations from the repository
        /// </summary>
        private void LoadYears()
        {
            try
            {
                // Load all year configurations
                var years = _repository.GetAllYearConfigurations();
                
                if (years == null || !years.Any())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        YearLevels = new ObservableCollection<string> { "All" };
                        YearTypes = new ObservableCollection<string> { "All" };
                        YearOptions = new ObservableCollection<YearConfig>();
                    });
                    return;
                }

                // Get unique year levels and types
                var yearLevels = years.Select(y => y.YearLevel?.Trim() ?? "")
                    .Where(y => !string.IsNullOrWhiteSpace(y))
                    .Distinct()
                    .OrderBy(y => y)
                    .ToList();
                
                var yearTypes = years.Select(y => y.YearType?.Trim() ?? "")
                    .Where(y => !string.IsNullOrWhiteSpace(y))
                    .Distinct()
                    .OrderBy(y => y)
                    .ToList();

                // Update UI on the UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Initialize collections with "All" option
                    YearLevels = new ObservableCollection<string> { "All" };
                    YearTypes = new ObservableCollection<string> { "All" };

                    // Add unique values to collections
                    foreach (var level in yearLevels)
                    {
                        YearLevels.Add(level);
                    }
                    foreach (var type in yearTypes)
                    {
                        YearTypes.Add(type);
                    }

                    // Set default selections
                    SelectedYearLevel = "All";
                    SelectedYearType = "All";

                    // Store year configurations for reference
                    YearOptions = new ObservableCollection<YearConfig>(years);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading years: {ex}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error loading years: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    // Ensure collections are initialized even in case of error
                    YearLevels = new ObservableCollection<string> { "All" };
                    YearTypes = new ObservableCollection<string> { "All" };
                    YearOptions = new ObservableCollection<YearConfig>();
                });
            }
        }

        /// <summary>
        /// Loads students from the repository
        /// </summary>
        private void LoadStudents()
        {
            try
            {
                // Get students from repository
                var students = _repository.GetAllStudents();
                
                if (students != null)
                {
                    // Load profile images for each student
                    foreach (var student in students)
                    {
                        LoadProfileImage(student);
                    }
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _allStudents = new ObservableCollection<StudentModel>(students);
                        // Apply filters and update the UI
                        FilterStudents();
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _allStudents = new ObservableCollection<StudentModel>();
                        Students = new ObservableCollection<StudentModel>();
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading students: {ex}");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error loading students: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _allStudents = new ObservableCollection<StudentModel>();
                    Students = new ObservableCollection<StudentModel>();
                });
            }
        }

        /// <summary>
        /// Loads the profile image for a student
        /// </summary>
        /// <param name="student">The student to load the profile image for</param>
        private void LoadProfileImage(StudentModel student)
        {
            if (!string.IsNullOrEmpty(student.ProfilePicture))
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProfilePictures", student.ProfilePicture);
                if (File.Exists(imagePath))
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze(); // Make it thread-safe
                        student.ProfileImageSource = bitmap;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading image for student {student.Id}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Filters the students based on the current filter criteria
        /// </summary>
        private void FilterStudents()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Filtering students: YearLevel={SelectedYearLevel}, YearType={SelectedYearType}, Semester={SelectedSemester}, SearchText={SearchText}");
                
                List<StudentModel> filteredStudents = new List<StudentModel>(_allStudents);
                
                // If we have an active filter, apply it
                if (filteredStudents != null)
                {
                    // Filter by year level if selected
                    if (!string.IsNullOrEmpty(SelectedYearLevel) && SelectedYearLevel != "All")
                    {
                        filteredStudents = filteredStudents.Where(s => s.YearLevel == SelectedYearLevel).ToList();
                        System.Diagnostics.Debug.WriteLine($"After year level filter: {filteredStudents.Count} students");
                    }
                    
                    // Filter by year type if selected
                    if (!string.IsNullOrEmpty(SelectedYearType) && SelectedYearType != "All")
                    {
                        filteredStudents = filteredStudents.Where(s => s.YearType == SelectedYearType).ToList();
                        System.Diagnostics.Debug.WriteLine($"After year type filter: {filteredStudents.Count} students");
                    }
                    
                    // Filter by semester if selected
                    if (SelectedSemester > 0)
                    {
                        filteredStudents = filteredStudents.Where(s => s.Semester == SelectedSemester).ToList();
                        System.Diagnostics.Debug.WriteLine($"After semester filter: {filteredStudents.Count} students");
                    }
                    
                    // Filter by search text if provided
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        string searchLower = SearchText.ToLower();
                        filteredStudents = filteredStudents.Where(s => 
                            s.Name.ToLower().Contains(searchLower) || 
                            s.RollNo.ToLower().Contains(searchLower) ||
                            s.Address.ToLower().Contains(searchLower) ||
                            s.Phone.ToLower().Contains(searchLower)
                        ).ToList();
                        System.Diagnostics.Debug.WriteLine($"After search filter: {filteredStudents.Count} students");
                    }
                }
                
                // Update Students collection with filtered results
                Students = new ObservableCollection<StudentModel>(filteredStudents);
                System.Diagnostics.Debug.WriteLine($"Filter complete: {Students.Count} students in final list");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error filtering students: {ex}");
                MessageBox.Show($"Error filtering students: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
