using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Student_Management.View;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Student_Management.Repository;
using Student_Management.Model;
using System.Data.SqlClient;

namespace Student_Management.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly StudentRepository _repository;
        private ObservableCollection<YearConfig> _years;
        private YearConfig _selectedYear;
        private string _newYearName;
        private string _newClassName;
        private ObservableCollection<ClassModel> _classesForSelectedYear;
        private string _selectedSemester;
        private ObservableCollection<string> _semesters;

        public ObservableCollection<YearConfig> Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }

        public YearConfig SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClassesForSelectedYear();
            }
        }

        public string NewYearName
        {
            get => _newYearName;
            set
            {
                _newYearName = value;
                OnPropertyChanged(nameof(NewYearName));
            }
        }

        public string NewClassName
        {
            get => _newClassName;
            set
            {
                _newClassName = value;
                OnPropertyChanged(nameof(NewClassName));
            }
        }

        public ObservableCollection<ClassModel> ClassesForSelectedYear
        {
            get => _classesForSelectedYear;
            set
            {
                _classesForSelectedYear = value;
                OnPropertyChanged(nameof(ClassesForSelectedYear));
            }
        }

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
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

        public ICommand AddYearCommand { get; }
        public ICommand DeleteYearCommand { get; }
        public ICommand AddClassCommand { get; }
        public ICommand DeleteClassCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public SettingsViewModel()
        {
            _repository = new StudentRepository();
            ClassesForSelectedYear = new ObservableCollection<ClassModel>();
            InitializeSemesters();
            LoadYears();

            AddYearCommand = new ViewModelCommand(param => ExecuteAddYear(param));
            DeleteYearCommand = new ViewModelCommand(param => ExecuteDeleteYear(param));
            AddClassCommand = new ViewModelCommand(param => ExecuteAddClass(param));
            DeleteClassCommand = new ViewModelCommand(param => ExecuteDeleteClass(param));
            SaveChangesCommand = new ViewModelCommand(param => ExecuteSaveChanges(param));
        }

        private void LoadYears()
        {
            Years = new ObservableCollection<YearConfig>(_repository.GetAllYearConfigurations());
        }

        private void InitializeSemesters()
        {
            Semesters = new ObservableCollection<string>
            {
                "First Semester",
                "Second Semester"
            };
            SelectedSemester = Semesters.First();
        }

        private void LoadClassesForSelectedYear()
        {
            if (SelectedYear == null) return;

            int yearNumber = ConvertYearTextToNumber(SelectedYear.YearName);
            
            using (var connection = _repository.GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT ClassID, ClassName, Year, Semester 
                      FROM Classes 
                      WHERE Year = @Year 
                      ORDER BY Semester, ClassName", connection);

                command.Parameters.AddWithValue("@Year", yearNumber);
                
                var classes = new ObservableCollection<ClassModel>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(new ClassModel
                        {
                            ClassID = Convert.ToInt32(reader["ClassID"]),
                            ClassName = reader["ClassName"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            Semester = Convert.ToInt32(reader["Semester"])
                        });
                    }
                }
                ClassesForSelectedYear = classes;
            }
        }

        private void ExecuteAddYear(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewYearName)) return;

            var newYear = new YearConfig
            {
                YearName = NewYearName,
                NumberOfSemesters = 2  // Default to 2 semesters
            };

            _repository.AddYear(newYear);
            Years.Add(newYear);
            NewYearName = string.Empty;
        }

        private void ExecuteDeleteYear(object parameter)
        {
            if (SelectedYear == null) return;

            if (MessageBox.Show("Are you sure you want to delete this year?", "Confirm Delete",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _repository.DeleteYear(SelectedYear.Id);
                Years.Remove(SelectedYear);
            }
        }

        private void ExecuteAddClass(object parameter)
        {
            if (SelectedYear == null || string.IsNullOrWhiteSpace(NewClassName)) return;

            var yearNumber = ConvertYearTextToNumber(SelectedYear.YearName);
            var semesterNumber = SelectedSemester.StartsWith("First") ? 1 : 2;

            var newClass = new ClassModel
            {
                ClassName = NewClassName,
                Year = yearNumber,
                Semester = semesterNumber
            };

            try
            {
                _repository.AddClass(newClass);
                ClassesForSelectedYear.Add(newClass);
                NewClassName = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding class: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteClass(object parameter)
        {
            if (parameter is ClassModel classToDelete)
            {
                try
                {
                    _repository.DeleteClass(classToDelete.ClassID);
                    ClassesForSelectedYear.Remove(classToDelete);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting class: {ex.Message}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteSaveChanges(object parameter)
        {
            try
            {
                MessageBox.Show("Changes saved successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int ConvertYearTextToNumber(string yearText)
        {
            switch (yearText?.Trim())
            {
                case "First Year": return 1;
                case "Second Year": return 2;
                case "Third Year": return 3;
                case "Fourth Year": return 4;
                case "First Year (Honors)": return 5;
                case "Second Year (Honors)": return 6;
                case "Third Year (Honors)": return 7;
                case "Fourth Year (Honors)": return 8;
                default:
                    Console.WriteLine($"Warning: Unknown year text: '{yearText}'");
                    return 1;
            }
        }
    }
}