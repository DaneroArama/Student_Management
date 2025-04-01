using System.Windows.Controls;
using Student_Management.ViewModel;

namespace Student_Management.View
{
    public partial class AttendanceView : UserControl
    {
        private AttendanceViewModel _viewModel;

        public AttendanceView()
        {
            InitializeComponent();
            _viewModel = new AttendanceViewModel();
            DataContext = _viewModel;
        }

        private void AttendanceView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.LoadAttendanceRecords();
        }
    }
} 