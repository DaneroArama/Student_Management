using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Student_Management.ViewModel;

namespace Student_Management.View
{
    public partial class CustomCalendar : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            nameof(SelectedDate),
            typeof(DateTime),
            typeof(CustomCalendar),
            new PropertyMetadata(DateTime.Now, OnSelectedDateChanged)
        );

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public CustomCalendar()
        {
            InitializeComponent(); // Connects to CustomCalendar.xaml
            LoadCalendar(DateTime.Now); // Load the current month by default
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CustomCalendar)d;
            control.LoadCalendar((DateTime)e.NewValue);
        }

        public void LoadCalendar(DateTime date)
        {
            SelectedDate = new DateTime(date.Year, date.Month, 1); // Set to the first day of the month
            DaysGrid.Children.Clear();

            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startDay = (int)new DateTime(date.Year, date.Month, 1).DayOfWeek;

            // Add empty days for the previous month
            for (int i = 0; i < startDay; i++)
            {
                DaysGrid.Children.Add(new TextBlock { Text = "", Margin = new Thickness(5) });
            }

            // Add days of the current month
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayButton = new Button
                {
                    Content = day,
                    Margin = new Thickness(5),
                    Tag = new DateTime(date.Year, date.Month, day)
                };

                // Highlight weekends
                var currentDate = new DateTime(date.Year, date.Month, day);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayButton.Background = Brushes.Red; // Highlight weekends in red
                }

                // Add click event handler
                dayButton.Click += DayButton_Click;

                // Add the button to the grid
                DaysGrid.Children.Add(dayButton);
            }
        }

        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is DateTime date)
            {
                // Find the AttendanceView in the visual tree
                AttendanceView attendanceView = FindParent<AttendanceView>(this);
                if (attendanceView != null)
                {
                    // Load the AttendanceDetailView
                    var attendanceDetailView = new AttendanceDetailView
                    {
                        DataContext = new AttendanceDetailViewModel
                        {
                            SelectedDate = date // Set the selected date
                        }
                    };

                    // Set the content to AttendanceDetailView
                    attendanceView.ContentControl.Content = attendanceDetailView;
                }
            }
        }

        // Method to refresh the calendar based on the selected date
        public void RefreshCalendar(DateTime date)
        {
            LoadCalendar(date);
        }

        // Helper method to find the parent of a specific type in the visual tree
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }
    }
}
