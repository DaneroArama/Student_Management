using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Student_Management.Model;
using Student_Management.Repository;

namespace Student_Management.ViewModel
{
    /// <summary>
    /// ViewModel for editing student information
    /// </summary>
    public class EditStudentViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly StudentRepository _repository;
        private StudentModel _student;
        private string _selectedYearLevel;
        private string _selectedYearType;
        private ObservableCollection<string> _yearLevels;
        private ObservableCollection<string> _yearTypes;
        private ObservableCollection<int> _semesterOptions;
        private ICommand _saveCommand;
        private ICommand _backCommand;
        private ICommand _uploadImageCommand;
        private readonly Action _navigateBack;
        private bool _isLoading;
        private bool _isSaving;
        private string _errorMessage;
        private readonly Action _onSaveCallback;

        #endregion

        #region Public Properties

        /// <summary>
        /// The student being edited
        /// </summary>
        public StudentModel Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged(nameof(Student));
                OnPropertyChanged(nameof(HasProfileImage));
                
                if (_student != null)
                {
                    SelectedYearLevel = _student.YearLevel;
                    SelectedYearType = _student.YearType;
                }
            }
        }

        /// <summary>
        /// The selected year level
        /// </summary>
        public string SelectedYearLevel
        {
            get => _selectedYearLevel;
            set
            {
                _selectedYearLevel = value;
                OnPropertyChanged(nameof(SelectedYearLevel));
                UpdateStudentYearId();
            }
        }

        /// <summary>
        /// The selected year type
        /// </summary>
        public string SelectedYearType
        {
            get => _selectedYearType;
            set
            {
                _selectedYearType = value;
                OnPropertyChanged(nameof(SelectedYearType));
                UpdateStudentYearId();
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

        /// <summary>
        /// Flag indicating if data is currently being saved
        /// </summary>
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged(nameof(IsSaving));
                OnPropertyChanged(nameof(CanSave));
            }
        }

        /// <summary>
        /// Indicates whether the student has a profile image
        /// </summary>
        public bool HasProfileImage => Student?.ProfileImageSource != null;

        /// <summary>
        /// Error message to display
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasError));
            }
        }

        /// <summary>
        /// Flag indicating if there is an error message
        /// </summary>
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Flag indicating if the save command can be executed
        /// </summary>
        public bool CanSave => !IsSaving && !HasError;

        #endregion

        #region Commands

        /// <summary>
        /// Command to save the student
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(SaveStudent, () => CanSave);
            }
        }

        /// <summary>
        /// Command to navigate back
        /// </summary>
        public ICommand BackCommand => _backCommand ??= new RelayCommand(() => _navigateBack?.Invoke());

        /// <summary>
        /// Command to upload a profile image
        /// </summary>
        public ICommand UploadImageCommand => _uploadImageCommand ??= new RelayCommand(UploadImage);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EditStudentViewModel class
        /// </summary>
        /// <param name="student">The student to edit, or null for a new student</param>
        /// <param name="onSaveCallback">Action to execute when saving the student</param>
        public EditStudentViewModel(StudentModel student, Action onSaveCallback)
        {
            _repository = new StudentRepository();
            _onSaveCallback = onSaveCallback;
            _navigateBack = onSaveCallback; // Set the navigate back action to the same callback
            
            // Initialize collections
            YearLevels = new ObservableCollection<string>();
            YearTypes = new ObservableCollection<string> { "Regular", "Honors" };
            SemesterOptions = new ObservableCollection<int> { 1, 2 };
            
            // Load year levels
            LoadYearLevels();
            
            // Initialize student or create a new one
            if (student != null)
            {
                Student = student;
                SelectedYearLevel = student.YearLevel;
                SelectedYearType = student.YearType;
                
                System.Diagnostics.Debug.WriteLine($"Editing student: {student.Name}, YearLevel: {student.YearLevel}, YearType: {student.YearType}, Semester: {student.Semester}");
            }
            else
            {
                Student = new StudentModel
                {
                    RollNo = string.Empty,
                    Name = string.Empty,
                    YearLevel = YearLevels.Count > 0 ? YearLevels[0] : string.Empty,
                    YearType = "Regular",  // Default to Regular
                    Semester = 1,
                    Phone = string.Empty,
                    Address = string.Empty,
                    ProfilePicture = null
                };
                
                if (YearLevels.Count > 0)
                    SelectedYearLevel = YearLevels[0];
                
                SelectedYearType = "Regular";
                
                System.Diagnostics.Debug.WriteLine($"Created new student with default YearType: {SelectedYearType}");
            }
            
            // Initialize commands using field
            _saveCommand = new RelayCommand(SaveStudent, () => CanSave);
            _backCommand = new RelayCommand(GoBack);
            _uploadImageCommand = new RelayCommand(UploadImage);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads year options from the repository
        /// </summary>
        private void LoadYearLevels()
        {
            try
            {
                var years = _repository.GetAllYearConfigurations();
                
                if (years.Count > 0)
                {
                    // Extract unique year levels (without the year type)
                    var yearLevels = years.Select(y => y.YearLevel.Replace(" (Honors)", ""))
                                          .Distinct()
                                          .ToList();
                    YearLevels = new ObservableCollection<string>(yearLevels);
                    
                    // Make sure YearTypes is properly initialized
                    YearTypes = new ObservableCollection<string> { "Regular", "Honors" };
                    
                    System.Diagnostics.Debug.WriteLine($"Loaded {YearLevels.Count} year levels: {string.Join(", ", YearLevels)}");
                    System.Diagnostics.Debug.WriteLine($"YearTypes: {string.Join(", ", YearTypes)}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading year levels: {ex.Message}");
                MessageBox.Show($"Error loading year levels: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the student's year ID based on the selected year level and type
        /// </summary>
        private void UpdateStudentYearId()
        {
            if (string.IsNullOrEmpty(SelectedYearLevel) || string.IsNullOrEmpty(SelectedYearType))
                return;

            try
            {
                var years = _repository.GetAllYearConfigurations();
                
                // Debug the available year configurations
                Debug.WriteLine($"Updating student year ID with Level={SelectedYearLevel}, Type={SelectedYearType}");
                
                // Find the matching year configuration based on both level and type
                var selectedYear = years.FirstOrDefault(y => 
                    y.YearLevel.Equals(SelectedYearLevel, StringComparison.OrdinalIgnoreCase) && 
                    y.YearType.Equals(SelectedYearType, StringComparison.OrdinalIgnoreCase));
                
                if (selectedYear == null)
                {
                    // Try to find by just matching the first word of YearLevel and the YearType
                    string firstWord = SelectedYearLevel.Split(' ')[0]; // Get "First", "Second", etc.
                    selectedYear = years.FirstOrDefault(y => 
                        y.YearLevel.StartsWith(firstWord, StringComparison.OrdinalIgnoreCase) && 
                        y.YearType.Equals(SelectedYearType, StringComparison.OrdinalIgnoreCase));
                    
                    Debug.WriteLine($"Trying to match with first word: {firstWord} and type: {SelectedYearType}");
                }

                if (selectedYear != null)
                {
                    Student.YearId = selectedYear.YearId;
                    Student.YearLevel = selectedYear.YearLevel;
                    Student.YearType = selectedYear.YearType;
                    
                    Debug.WriteLine($"Updated student year: ID={selectedYear.YearId}, Level={selectedYear.YearLevel}, Type={selectedYear.YearType}");
                }
                else
                {
                    Debug.WriteLine($"ERROR: Could not find year configuration for Level={SelectedYearLevel}, Type={SelectedYearType}");
                    ErrorMessage = $"Could not find year configuration for {SelectedYearLevel} ({SelectedYearType})";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating year ID: {ex}");
                ErrorMessage = $"Error updating year: {ex.Message}";
            }
        }

        /// <summary>
        /// Validates the student data
        /// </summary>
        /// <returns>True if the data is valid; otherwise, false</returns>
        private bool ValidateStudent()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(Student.Name))
            {
                ErrorMessage = "Please enter a name for the student.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Student.RollNo))
            {
                ErrorMessage = "Please enter a Roll No for the student.";
                return false;
            }

            if (Student.YearId == 0)
            {
                ErrorMessage = "Please select a year for the student.";
                return false;
            }

            // Clear any previous error
            ErrorMessage = null;
            return true;
        }

        /// <summary>
        /// Saves the student
        /// </summary>
        private async void SaveStudent()
        {
            if (!ValidateStudent())
                return;

            try
            {
                IsSaving = true;
                ErrorMessage = null;

                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(Student.Id))
                    {
                        // This is a new student
                        _repository.AddStudent(Student);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Student added successfully!", "Success", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                    else
                    {
                        // This is an existing student
                        _repository.UpdateStudent(Student);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Student updated successfully!", "Success", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                });

                // Navigate back to the student list
                _onSaveCallback?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving student: {ex}");
                ErrorMessage = $"Error saving student: {ex.Message}";
            }
            finally
            {
                IsSaving = false;
            }
        }

        /// <summary>
        /// Uploads a profile image for the student
        /// </summary>
        private void UploadImage()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                    Title = "Select Profile Image"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var imagePath = openFileDialog.FileName;
                    
                    // Create a unique filename for the profile picture
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagePath)}";
                    string destinationDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProfilePictures");
                    
                    // Ensure the directory exists
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }
                    
                    string destinationPath = Path.Combine(destinationDirectory, fileName);
                    
                    // Copy the file to the destination
                    File.Copy(imagePath, destinationPath, true);
                    
                    // Set the profile picture path
                    Student.ProfilePicture = fileName;
                    
                    // Update the image source
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Student.ProfileImageSource = bitmap;
                    
                    OnPropertyChanged(nameof(HasProfileImage));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error uploading image: {ex.Message}";
            }
        }

        /// <summary>
        /// Navigates back to the previous view
        /// </summary>
        private void GoBack()
        {
            _navigateBack?.Invoke();
        }

        #endregion
    }
}