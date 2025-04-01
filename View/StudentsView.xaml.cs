using System;
using System.Windows;
using System.Windows.Controls;
using Student_Management.ViewModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Data;
using Student_Management.Model;

namespace Student_Management.View
{
    /// <summary>
    /// Interaction logic for StudentsView.xaml
    /// </summary>
    public partial class StudentsView : UserControl
    {
        private StudentsViewModel _viewModel;

        public StudentsView()
        {
            try
            {
                InitializeComponent();
                _viewModel = new StudentsViewModel();
                DataContext = _viewModel;

                // Add handler for property changes
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;

                this.Loaded += StudentsView_Loaded;
                this.Unloaded += StudentsView_Unloaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing StudentsView: {ex.Message}", 
                    "Initialization Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private void StudentsView_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("StudentsView loaded");
            _viewModel = DataContext as StudentsViewModel;
            if (_viewModel != null)
            {
                Debug.WriteLine("ViewModel found in DataContext");
                Debug.WriteLine($"Initial state - IsEditMode: {_viewModel.IsEditMode}, IsEditVisible: {_viewModel.IsEditVisible}, IsListVisible: {_viewModel.IsListVisible}");
            }
            else
            {
                Debug.WriteLine("ViewModel not found in DataContext");
            }
            
            VerifyDataContext();
            
            // Initialize the DataGrid
            if (_viewModel != null && StudentsDataGrid != null)
            {
                // Force the DataGrid to update with the current data
                StudentsDataGrid.ItemsSource = _viewModel.Students;
                StudentsDataGrid.Items.Refresh();
            }

            // Uncomment this to test the EditStudentView manually
            // TestEditStudentView();
        }

        private void StudentsView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                // Clean up event handlers
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                _viewModel = null;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update the UI when Students collection changes
            if (e.PropertyName == nameof(StudentsViewModel.Students))
            {
                if (_viewModel != null)
                {
                    // Force UI update
                    Application.Current.Dispatcher.Invoke(() => {
                        try
                        {
                            // Ensure the DataGrid is updated with the new collection
                            if (StudentsDataGrid != null)
                            {
                                // Set the ItemsSource directly
                                StudentsDataGrid.ItemsSource = _viewModel.Students;
                                
                                // Force refresh
                                StudentsDataGrid.Items.Refresh();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error updating DataGrid: {ex.Message}");
                        }
                    });
                }
            }
            else if (e.PropertyName == nameof(StudentsViewModel.IsEditMode) || 
                     e.PropertyName == nameof(StudentsViewModel.IsListVisible) ||
                     e.PropertyName == nameof(StudentsViewModel.EditContent))
            {
                // Force UI update for visibility changes
                Application.Current.Dispatcher.Invoke(() => {
                    Debug.WriteLine($"Property changed: {e.PropertyName}");
                    Debug.WriteLine($"Current state - IsEditMode: {_viewModel.IsEditMode}, IsEditVisible: {_viewModel.IsEditVisible}, IsListVisible: {_viewModel.IsListVisible}");
                    Debug.WriteLine($"EditContent is null: {_viewModel.EditContent == null}");
                    
                    // Force a layout update
                    if (this.Parent is FrameworkElement parent)
                    {
                        parent.UpdateLayout();
                    }
                });
            }
            else if (e.PropertyName == nameof(StudentsViewModel.SearchText))
            {
                Debug.WriteLine($"SearchText property changed to: '{_viewModel?.SearchText}'");
                Debug.WriteLine($"SearchTextBox text: '{SearchTextBox?.Text}'");
            }
        }

        // Method to manually refresh the DataGrid
        private void RefreshDataGrid()
        {
            try
            {
                if (StudentsDataGrid != null)
                {
                    // Set the ItemsSource directly
                    if (_viewModel != null)
                    {
                        StudentsDataGrid.ItemsSource = _viewModel.Students;
                    }
                    
                    // Force refresh
                    StudentsDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing DataGrid: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Optional: Add this method to verify the DataContext at any time
        public void VerifyDataContext()
        {
            if (_viewModel != null)
            {
                // Ensure the DataContext is set correctly
                if (this.DataContext != _viewModel)
                {
                    this.DataContext = _viewModel;
                }
            }
        }

        // Test method to manually show the EditStudentView
        private void TestEditStudentView()
        {
            try
            {
                Debug.WriteLine("Testing EditStudentView creation...");
                var editView = new EditStudentView(null, () => 
                {
                    Debug.WriteLine("Test navigate back called");
                    _viewModel.IsEditMode = false;
                });
                
                Debug.WriteLine($"EditStudentView created: {editView != null}");
                
                // Force reset edit mode to ensure UI updates
                if (_viewModel.IsEditMode)
                {
                    Debug.WriteLine("Resetting IsEditMode to false first");
                    _viewModel.IsEditMode = false;
                }
                
                // Set the edit content first
                _viewModel.EditContent = editView;
                Debug.WriteLine($"EditContent set: {_viewModel.EditContent != null}");
                
                // Then set edit mode to true
                _viewModel.IsEditMode = true;
                Debug.WriteLine($"IsEditMode: {_viewModel.IsEditMode}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error testing EditStudentView: {ex}");
                MessageBox.Show($"Error: {ex.Message}", "Test Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
