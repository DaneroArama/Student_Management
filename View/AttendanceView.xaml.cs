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

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            AttendancePopup.IsOpen = false;
        }

        public void LoadAttendanceForDate(DateTime date)
        {
            var viewModel = (AttendanceViewModel)this.DataContext;
            viewModel.LoadAttendanceForDate(date);
            AttendancePopup.IsOpen = true; // Open the pop-up to show attendance details
        }
    }
}