using System.Collections.ObjectModel;
using System.ComponentModel;
using Student_Management.Model;
using Student_Management.Repository;
using System.Windows.Input;
using System.Windows;
using Student_Management.View;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Linq;

namespace Student_Management.ViewModel
{
    public class StudentsViewModel : ViewModelBase
    {
        private readonly StudentRepository _repository;
        private ObservableCollection<StudentModel> _allStudents;
        private ObservableCollection<StudentModel> _students;
        private StudentModel _selectedStudent;
        private StudentModel _editingStudent;
        private bool _isEditMode;
        private string _searchText;
        private string _selectedYear;
        private string _selectedSemester;
        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _addCommand;
        private ICommand _clearSearchCommand;
        private ICommand _deleteCommand;
        private ICommand _saveCommand;
        private ICommand _uploadImageCommand;

        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

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

        public StudentModel EditingStudent
        {
            get => _editingStudent;
            set
            {
                _editingStudent = value;
                OnPropertyChanged(nameof(EditingStudent));
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(IsListVisible));
                OnPropertyChanged(nameof(IsEditVisible));
                OnPropertyChanged(nameof(EditButtonText));
            }
        }

        public bool IsListVisible => !IsEditMode;
        public bool IsEditVisible => IsEditMode;
        public string EditButtonText => IsEditMode ? "Save" : "Edit";
        public bool IsStudentSelected => SelectedStudent != null;

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

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                FilterStudents();
            }
        }

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                FilterStudents();
            }
        }

        public ObservableCollection<string> YearOptions { get; private set; }
        public ObservableCollection<string> SemesterOptions { get; private set; }

        public ICommand EditCommand => _editCommand ??= new RelayCommand(ExecuteEditCommand);
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(CancelEdit);
        public ICommand AddCommand => _addCommand ??= new RelayCommand(StartAdd);
        public ICommand ClearSearchCommand => _clearSearchCommand ??= new RelayCommand(ClearSearch);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(DeleteStudent, () => SelectedStudent != null);
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(SaveStudent);
        public ICommand UploadImageCommand => _uploadImageCommand ??= new RelayCommand(UploadImage);

        public StudentsViewModel()
        {
            _repository = new StudentRepository();
            
            // Initialize Year options with "All" and the values from repository
            YearOptions = new ObservableCollection<string>();
            YearOptions.Add("All");
            foreach (var year in _repository.GetAllYears())
            {
                YearOptions.Add(year);
            }

            // Initialize Semester options with "All" and the values from repository
            SemesterOptions = new ObservableCollection<string>();
            SemesterOptions.Add("All");
            foreach (var semester in _repository.GetAllSemesters())
            {
                SemesterOptions.Add(semester);
            }
            
            LoadStudents();
        }

        private void ExecuteEditCommand()
        {
            if (!IsEditMode)
            {
                // Start editing
                if (SelectedStudent == null) return;
                EditingStudent = new StudentModel
                {
                    Id = SelectedStudent.Id,
                    RollNo = SelectedStudent.RollNo,
                    Name = SelectedStudent.Name,
                    Year = SelectedStudent.Year,
                    Semester = SelectedStudent.Semester,
                    Phone = SelectedStudent.Phone,
                    Address = SelectedStudent.Address,
                    ProfilePicture = SelectedStudent.ProfilePicture,
                    ProfileImageSource = SelectedStudent.ProfileImageSource
                };
                IsEditMode = true;
            }
        }

        private void CancelEdit()
        {
            IsEditMode = false;
            EditingStudent = null;
        }

        private void LoadStudents()
        {
            try
            {
                _allStudents = _repository.GetAllStudents();
                foreach (var student in _allStudents)
                {
                    LoadProfileImage(student);
                }
                FilterStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterStudents()
        {
            IEnumerable<StudentModel> filteredStudents = _allStudents;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredStudents = filteredStudents.Where(s => 
                    s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.RollNo.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Only filter by year if not "All"
            if (!string.IsNullOrWhiteSpace(SelectedYear) && SelectedYear != "All")
            {
                int yearNumber;
                switch (SelectedYear)
                {
                    case "First Year": yearNumber = 1; break;
                    case "Second Year": yearNumber = 2; break;
                    case "Third Year": yearNumber = 3; break;
                    case "Fourth Year": yearNumber = 4; break;
                    case "First Year (Honors)": yearNumber = 5; break;
                    case "Second Year (Honors)": yearNumber = 6; break;
                    case "Third Year (Honors)": yearNumber = 7; break;
                    case "Fourth Year (Honors)": yearNumber = 8; break;
                    default: yearNumber = 0; break;
                }

                filteredStudents = filteredStudents.Where(s => 
                    int.TryParse(s.Year, out int studentYear) && studentYear == yearNumber);
            }

            // Only filter by semester if not "All"
            if (!string.IsNullOrWhiteSpace(SelectedSemester) && SelectedSemester != "All")
            {
                int semesterNumber = SelectedSemester.StartsWith("First") ? 1 : 2;
                
                filteredStudents = filteredStudents.Where(s => 
                    int.TryParse(s.Semester, out int studentSemester) && studentSemester == semesterNumber);
            }

            Students = new ObservableCollection<StudentModel>(filteredStudents);
        }

        private void ClearSearch()
        {
            SearchText = string.Empty;
        }

        private void StartAdd()
        {
            EditingStudent = new StudentModel();
            IsEditMode = true;
            SelectedStudent = null;
        }

        private void DeleteStudent()
        {
            if (SelectedStudent == null) return;

            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to delete student {SelectedStudent.Name}?",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    // Convert Id to int since your database uses int for ID
                    if (int.TryParse(SelectedStudent.Id, out int studentId))
                    {
                        _repository.DeleteStudent(studentId);
                        LoadStudents();
                        SelectedStudent = null;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(
                        $"Error deleting student: {ex.Message}",
                        "Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void SaveStudent()
        {
            try
            {
                if (EditingStudent == null) return;

                if (string.IsNullOrEmpty(EditingStudent.Id))
                {
                    // This is a new student
                    _repository.AddStudent(new StudentModel
                    {
                        RollNo = EditingStudent.RollNo,
                        Name = EditingStudent.Name,
                        Year = EditingStudent.Year,
                        Semester = EditingStudent.Semester,
                        Phone = EditingStudent.Phone,
                        Address = EditingStudent.Address,
                        ProfilePicture = EditingStudent.ProfilePicture
                    });
                }
                else
                {
                    // This is an existing student
                    _repository.UpdateStudent(new StudentModel
                    {
                        Id = EditingStudent.Id,
                        RollNo = EditingStudent.RollNo,
                        Name = EditingStudent.Name,
                        Year = EditingStudent.Year,
                        Semester = EditingStudent.Semester,
                        Phone = EditingStudent.Phone,
                        Address = EditingStudent.Address,
                        ProfilePicture = EditingStudent.ProfilePicture
                    });
                }

                LoadStudents();
                IsEditMode = false;
                EditingStudent = null;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error saving student: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void UploadImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, `*.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Create ProfilePictures directory if it doesn't exist
                    string profilePicturesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProfilePictures");
                    Directory.CreateDirectory(profilePicturesPath);

                    // Generate unique filename to avoid overwrites
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(openFileDialog.FileName)}";
                    string destinationPath = Path.Combine(profilePicturesPath, uniqueFileName);

                    // Copy the file
                    File.Copy(openFileDialog.FileName, destinationPath, true);

                    // Update the EditingStudent
                    if (EditingStudent != null)
                    {
                        EditingStudent.ProfilePicture = uniqueFileName;
                        
                        // Create BitmapImage
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(destinationPath);
                        bitmap.EndInit();
                        
                        EditingStudent.ProfileImageSource = bitmap;
                        OnPropertyChanged(nameof(EditingStudent));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading image: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

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
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.EndInit();
                        student.ProfileImageSource = bitmap;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading image for student {student.Id}: {ex.Message}");
                    }
                }
            }
        }
    }
}
