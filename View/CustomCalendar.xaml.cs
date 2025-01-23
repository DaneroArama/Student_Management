using System;
using System.Windows;
using System.Windows.Controls;

namespace Student_Management.View
{
    public partial class CustomCalendar : UserControl
    {
        public DateTime SelectedDate { get; set; }

        public CustomCalendar()
        {
            InitializeComponent(); // Connects to CustomCalendar.xaml
            SelectedDate = DateTime.Now;
            LoadCalendar(SelectedDate);
        }

        public void LoadCalendar(DateTime date)
        {
            // Set the selected date to the first day of the given month
            SelectedDate = new DateTime(date.Year, date.Month, 1);

            // Clear previous children in the grid
            DaysGrid.Children.Clear();

            // Get the number of days in the month and the start day
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startDay = (int)SelectedDate.DayOfWeek;

            // Add empty placeholders for days of the previous month
            for (int i = 0; i < startDay; i++)
            {
                DaysGrid.Children.Add(new TextBlock { Text = "", Margin = new Thickness(5) });
            }

            // Add buttons for each day in the current month
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayButton = new Button
                {
                    Content = day.ToString(),
                    Margin = new Thickness(5),
                    Tag = new DateTime(date.Year, date.Month, day)
                };

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
                MessageBox.Show($"You selected: {date.ToShortDateString()}");

                // Example: Call a method in AttendanceView (if it's the parent control)
                if (this.Parent is AttendanceView attendanceView)
                {
                    attendanceView.LoadAttendanceForDate(date);
                }
            }
        }
    }
}
