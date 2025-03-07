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

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate.HasValue)
            {
                // Refresh the CustomCalendar with the selected date
                CustomCalendarControl.RefreshCalendar(DatePicker.SelectedDate.Value);
            }
        }
        public void LoadAttendanceForDate(DateTime date)
        {
            var viewModel = (AttendanceViewModel)this.DataContext;
            viewModel.LoadAttendanceForDate(date);
        }
    }
}