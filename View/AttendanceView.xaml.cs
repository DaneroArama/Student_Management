using System;
using System.Windows;
using System.Windows.Controls;
using Student_Management.ViewModel;

namespace Student_Management.View
{
    public partial class AttendanceView : UserControl
    {
        public AttendanceView()
        {
            InitializeComponent();
        }

        private void AttendanceCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the event here
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // No need for manual refresh; handled by binding
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate.HasValue)
            {
                // Refresh the CustomCalendar with the selected date
                CustomCalendarControl.RefreshCalendar(DatePicker.SelectedDate.Value);
            }
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            AttendancePopup.IsOpen = false; // Close the pop-up
        }

        public void LoadAttendanceForDate(DateTime date)
        {
            var viewModel = (AttendanceViewModel)this.DataContext;
            viewModel.LoadAttendanceForDate(date);
            AttendancePopup.IsOpen = true; // Open the pop-up to show attendance details
        }
    }
}