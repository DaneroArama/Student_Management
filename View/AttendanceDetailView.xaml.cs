using System;
using System.Windows.Controls;
using Student_Management.ViewModel;
using System.Windows.Data;
using System.Windows;

namespace Student_Management.View
{
    public partial class AttendanceDetailView : UserControl
    {
        private AttendanceDetailViewModel _viewModel;

        public AttendanceDetailView()
        {
            InitializeComponent();
            _viewModel = new AttendanceDetailViewModel(NavigateBack);
            DataContext = _viewModel;
            
            // Initialize data after setting DataContext
            Loaded += UserControl_Loaded;
        }

        /// <summary>
        /// Navigates back to the AttendanceView panel
        /// </summary>
        public void NavigateBack()
        {
            // This method will be passed to the ViewModel as a callback
            // The parent view should handle this navigation
            if (Parent is Grid parentGrid)
            {
                parentGrid.Children.Remove(this);
            }
        }
        
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Subscribe to the Classes collection changed event to update columns
            if (_viewModel != null)
            {
                // Subscribe to property changed events
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                
                // Force initial load of data
                _viewModel.InitializeData();
            }
        }
        
        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Use Dispatcher to ensure UI updates happen on the UI thread
            Dispatcher.Invoke(() => {
                // The UI will automatically update when these properties change
                // because we're using data binding with ItemsControl
                if (e.PropertyName == "Classes")
                {
                    System.Diagnostics.Debug.WriteLine($"Classes collection changed. UI will update automatically.");
                }
                else if (e.PropertyName == "Students")
                {
                    System.Diagnostics.Debug.WriteLine($"Students collection changed. UI will update automatically.");
                }
                
                // When SelectedYearLevel changes
                if (e.PropertyName == "SelectedYearLevel")
                {
                    System.Diagnostics.Debug.WriteLine($"Year level changed: {_viewModel.SelectedYearLevel?.DisplayName}");
                }
                
                // When SelectedSemester changes
                if (e.PropertyName == "SelectedSemester")
                {
                    System.Diagnostics.Debug.WriteLine($"Semester changed: {_viewModel.SelectedSemester}");
                }
            });
        }
    }
}