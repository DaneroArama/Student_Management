using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Student_Management.Model;
using System.Windows;
using System.Windows.Controls;
using Student_Management.View;

namespace Student_Management.ViewModel
{
	public class AttendanceViewModel : ViewModelBase
	{
		private SeriesCollection _seriesCollection;
		public SeriesCollection SeriesCollection
		{
			get => _seriesCollection;
			set
			{
				_seriesCollection = value;
				OnPropertyChanged(nameof(SeriesCollection));
			}
		}

		private string[] _labels;
		public string[] Labels
		{
			get => _labels;
			set
			{
				_labels = value;
				OnPropertyChanged(nameof(Labels));
			}
		}

		private ObservableCollection<AttendanceRecord> _recentAttendance;
		public ObservableCollection<AttendanceRecord> RecentAttendance
		{
			get => _recentAttendance;
			set
			{
				_recentAttendance = value;
				OnPropertyChanged(nameof(RecentAttendance));
			}
		}

		public ICommand TakeAttendanceCommand { get; private set; }
		public ICommand ViewAllCommand { get; private set; }

		// Add these properties to your AttendanceViewModel class

		// Academic year information
		public string CurrentAcademicYear { get; set; } = "2023-2024";
		public DateTime AcademicStartDate { get; set; } = new DateTime(2023, 9, 1);
		public DateTime AcademicEndDate { get; set; } = new DateTime(2024, 6, 30);
		public int TotalAcademicDays { get; set; } = 200;

		// Attendance statistics
		public int TotalStudents { get; set; } = 120;
		public int PresentStudents { get; set; } = 102;
		public int AbsentStudents { get; set; } = 18;
		public int PresentPercentage { get; set; } = 85;
		public int AbsentPercentage { get; set; } = 15;

		// Chart period selector
		public ObservableCollection<string> ChartPeriods { get; set; } = new ObservableCollection<string> 
		{ 
		    "Daily", "Weekly", "Monthly", "Yearly" 
		};
		public string SelectedChartPeriod { get; set; } = "Monthly";
		public string ChartXAxisTitle { get; set; } = "Month";

		// Commands for new buttons
		public ICommand AcademicSettingsCommand { get; }
		public ICommand ManageHolidaysCommand { get; }
		public ICommand ViewAllAttendanceCommand { get; }

		// Constructor - add initialization for new commands
		public AttendanceViewModel()
		{
		    // Initialize existing properties and commands
		    TakeAttendanceCommand = new RelayCommand(TakeAttendance);
		    ViewAllCommand = new RelayCommand(() => { /* Implementation */ });
		    
		    // Initialize new commands
		    AcademicSettingsCommand = new RelayCommand(OpenAcademicSettings);
		    ManageHolidaysCommand = new RelayCommand(OpenHolidayManager);
		    ViewAllAttendanceCommand = new RelayCommand(ViewAllAttendance);
		    
		    // Initialize chart data
		    InitializeChartData();
		    
		    // Load initial data
		    LoadAttendanceData();
		    LoadAttendanceRecords();
		}

		private void InitializeChartData()
		{
		    // Sample chart data
		    SeriesCollection = new SeriesCollection
		    {
		        new LineSeries
		        {
		            Title = "Present",
		            Values = new ChartValues<double> { 80, 85, 90, 82, 88, 92 },
		            PointGeometry = DefaultGeometries.Circle,
		            PointGeometrySize = 10,
		            Stroke = System.Windows.Media.Brushes.Green
		        },
		        new LineSeries
		        {
		            Title = "Absent",
		            Values = new ChartValues<double> { 20, 15, 10, 18, 12, 8 },
		            PointGeometry = DefaultGeometries.Square,
		            PointGeometrySize = 10,
		            Stroke = System.Windows.Media.Brushes.Red
		        }
		    };

		    Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
		}

		// Command methods
		private void OpenAcademicSettings()
		{
		    // Open academic year settings dialog
		    // This would allow setting start/end dates
		}

		private void OpenHolidayManager()
		{
		    // Open holiday management dialog
		    // This would allow adding/removing custom holidays
		}

		private void ViewAllAttendance()
		{
		    // Navigate to a full attendance history view
		}

		private void LoadAttendanceData()
		{
		    // Load attendance data from database
		    // Update statistics and chart data
		}

		// Add method to handle chart period changes
		private void UpdateChartPeriod()
		{
		    switch (SelectedChartPeriod)
		    {
		        case "Daily":
		            ChartXAxisTitle = "Day";
		            // Update Labels and SeriesCollection for daily view
		            break;
		        case "Weekly":
		            ChartXAxisTitle = "Week";
		            // Update Labels and SeriesCollection for weekly view
		            break;
		        case "Monthly":
		            ChartXAxisTitle = "Month";
		            // Update Labels and SeriesCollection for monthly view
		            break;
		        case "Yearly":
		            ChartXAxisTitle = "Year";
		            // Update Labels and SeriesCollection for yearly view
		            break;
		    }
		}

		private bool _isEditMode;
		private UserControl _editContent;

		public bool IsEditMode
		{
			get => _isEditMode;
			set
			{
				_isEditMode = value;
				OnPropertyChanged(nameof(IsEditMode));
				OnPropertyChanged(nameof(IsListVisible));
				OnPropertyChanged(nameof(IsEditVisible));
			}
		}

		public bool IsListVisible => !IsEditMode;
		public bool IsEditVisible => IsEditMode;

		public UserControl EditContent
		{
			get => _editContent;
			set
			{
				_editContent = value;
				OnPropertyChanged(nameof(EditContent));
			}
		}

		private void TakeAttendance()
		{
			try
			{
				// Create a new instance of AttendanceDetailViewModel with navigation callback
				var detailViewModel = new AttendanceDetailViewModel(() =>
				{
					IsEditMode = false;
					LoadAttendanceRecords(); // Refresh the data when returning
				});
				
				// Create AttendanceDetailView with the new view model
				var detailView = new AttendanceDetailView();
				detailView.DataContext = detailViewModel;
				
				// Update UI properties
				EditContent = detailView;
				IsEditMode = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error opening attendance detail: {ex.Message}", 
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void LoadAttendanceRecords()
		{
			
		}
	}

	public class AttendanceRecord
	{
		public DateTime Date { get; set; }
		public string Status { get; set; }
		public string StundentName { get; set; }
		public string Class { get; set; }
		public string RollNo { get; set; }
		public string Remarks { get; set; }
	}
}