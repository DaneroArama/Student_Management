using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Student_Management.Model;
using System.Diagnostics;
using Student_Management.Repository;

namespace Student_Management.ViewModel
{
    public class AcademicEditViewModel : ViewModelBase
    {
        private readonly StudentRepository _repository;
        private ObservableCollection<YearConfig> _years;
        private YearConfig _selectedYear;
        private string _newYearLevel;
        private string _newYearType;
        private ObservableCollection<string> _yearTypes;
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

        public ObservableCollection<string> YearTypes
        {
            get => _yearTypes;
            set
            {
                _yearTypes = value;
                OnPropertyChanged(nameof(YearTypes));
            }
        }

        public string NewYearType
        {
            get => _newYearType;
            set
            {
                _newYearType = value;
                OnPropertyChanged(nameof(NewYearType));
            }
        }

        public string NewYearLevel
        {
            get => _newYearLevel;
            set
            {
                _newYearLevel = value;
                OnPropertyChanged(nameof(NewYearLevel));
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

        public ICommand AddYearCommand { get; private set; }
        public ICommand DeleteYearCommand { get; private set; }
        public ICommand AddClassCommand { get; private set; }
        public ICommand DeleteClassCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }

        public AcademicEditViewModel()
        {
            _repository = new StudentRepository();
            InitializeCollections();
            LoadData();
            InitializeCommands();
        }

        private void InitializeCollections()
        {
            Years = new ObservableCollection<YearConfig>();
            ClassesForSelectedYear = new ObservableCollection<ClassModel>();
            YearTypes = new ObservableCollection<string> { "Regular", "Honors" };
            Semesters = new ObservableCollection<string> { "First Semester", "Second Semester" };
            
            NewYearType = YearTypes.First();
            SelectedSemester = Semesters.First();
        }

        private void LoadData()
        {
            try
            {
                var years = _repository.GetAllYearConfigurations();
                Years = new ObservableCollection<YearConfig>(years);
                if (Years.Any())
                {
                    SelectedYear = Years.First();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading years: {ex.Message}");
                MessageBox.Show("Error loading academic years. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeCommands()
        {
            AddYearCommand = new ViewModelCommand(ExecuteAddYear);
            DeleteYearCommand = new ViewModelCommand(ExecuteDeleteYear);
            AddClassCommand = new ViewModelCommand(ExecuteAddClass);
            DeleteClassCommand = new ViewModelCommand(ExecuteDeleteClass);
            SaveChangesCommand = new ViewModelCommand(ExecuteSaveChanges);
        }

        private void LoadClassesForSelectedYear()
        {
            if (SelectedYear == null) return;

            try
            {
                var classes = _repository.GetClassesForYear(SelectedYear.YearId);
                ClassesForSelectedYear = new ObservableCollection<ClassModel>(classes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading classes: {ex.Message}");
                MessageBox.Show("Error loading classes. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ClassesForSelectedYear = new ObservableCollection<ClassModel>();
            }
        }

        private void ExecuteAddYear(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewYearLevel))
            {
                MessageBox.Show("Please enter a year level.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var yearLevel = NewYearLevel.Trim();
                
                // Format the year level properly
                if (!yearLevel.EndsWith("Year"))
                {
                    yearLevel += " Year";
                }

                // Remove any existing type indicators
                yearLevel = yearLevel.Replace(" (Honors)", "").Replace(" (Regular)", "");

                var newYear = new YearConfig
                {
                    YearLevel = yearLevel,
                    YearType = NewYearType,  // Store the type separately
                    NumberOfSemesters = 2  // Default to 2 semesters
                };

                _repository.AddYear(newYear);
                
                // Only add to the collection if we have a valid YearId
                if (newYear.YearId > 0)
                {
                    Years.Add(newYear);
                    NewYearLevel = string.Empty;
                    MessageBox.Show("Year added successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add year. Please try again.", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding year: {ex.Message}");
                MessageBox.Show("Error adding year. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteYear(object parameter)
        {
            if (parameter is not YearConfig yearToDelete) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this year? This will also delete all associated classes and attendance records.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.DeleteYear(yearToDelete.YearId);
                    Years.Remove(yearToDelete);
                    if (SelectedYear == yearToDelete)
                    {
                        SelectedYear = Years.FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting year: {ex.Message}");
                    MessageBox.Show("Error deleting year. Please try again.", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteAddClass(object parameter)
        {
            if (SelectedYear == null)
            {
                MessageBox.Show("Please select a year first.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewClassName))
            {
                MessageBox.Show("Please enter a class name.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var semesterNumber = SelectedSemester.StartsWith("First") ? 1 : 2;

                var newClass = new ClassModel
                {
                    ClassName = NewClassName.Trim(),
                    YearId = SelectedYear.YearId,
                    Semester = semesterNumber
                };

                _repository.AddClass(newClass);
                ClassesForSelectedYear.Add(newClass);
                NewClassName = string.Empty;

                MessageBox.Show("Class added successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding class: {ex.Message}");
                MessageBox.Show("Error adding class. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteClass(object parameter)
        {
            if (parameter is not ClassModel classToDelete) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this class? This will also delete all associated attendance records.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.DeleteClass(classToDelete.ClassID);
                    ClassesForSelectedYear.Remove(classToDelete);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting class: {ex.Message}");
                    MessageBox.Show("Error deleting class. Please try again.", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteSaveChanges(object parameter)
        {
            MessageBox.Show("All changes have been saved successfully!", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}