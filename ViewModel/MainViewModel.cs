using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using Student_Management.Model;
using Student_Management.Repository;
using Student_Management.View;

namespace Student_Management.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private UserAccountModel _currentUserAccount;

        private ViewModelBase _currentChildView;

        private string _caption;

        private IconChar _icon;
        
        private IUserRepository _userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                if (_currentUserAccount != value)
                {
                    _currentUserAccount = value;
                    OnPropertyChanged(nameof(CurrentUserAccount));
                }
            }
        }
        //Properties
        public ViewModelBase CurrentChildView 
        { 
            get => _currentChildView;

            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption 
        { 
            get => _caption;

            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }

        }
        public IconChar Icon 
        { 
            get => _icon;

            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        //-->Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowStudentViewCommand { get; }
        public ICommand ShowAttendanceViewCommand { get; }
        public ICommand ShowSettingsViewCommand { get; }
        public ICommand ShowMarksViewCommand { get; }
        public ICommand SignOutCommand { get; }

        public MainViewModel()
        {
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            //Initialize Commands
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowStudentViewCommand = new ViewModelCommand(ExecuteShowStudentViewCommand);
            ShowAttendanceViewCommand = new ViewModelCommand(ExecuteShowAttendanceViewCommand);
            ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);
            ShowMarksViewCommand = new ViewModelCommand(ExecuteShowMarksViewCommand);
            SignOutCommand = new RelayCommand(SignOut);
            //Default View
            ExecuteShowHomeViewCommand(null);
            LoadCurrentUserData();
        }
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        private void ExecuteShowStudentViewCommand(object obj)
        {
            CurrentChildView = new StudentsViewModel();
            Caption = "Students";
            Icon = IconChar.UserGraduate;
        }

        private void ExecuteShowAttendanceViewCommand(object obj)
        {
            CurrentChildView = new AttendanceViewModel();
            Caption = "Attendance";
            Icon = IconChar.CalendarCheck;
        }

        private void ExecuteShowSettingsViewCommand(object obj)
        {
            CurrentChildView = new SettingsViewModel() as ViewModelBase;
            Caption = "Academic";
            Icon = IconChar.BookBookmark;
        }

        private void ExecuteShowMarksViewCommand(object obj)
        {
            CurrentChildView = new MarksViewModel() as ViewModelBase;
            Caption = "Marks";
            Icon = IconChar.Marker;
        }

        private void LoadCurrentUserData()
        {
            var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {

                CurrentUserAccount.Username = user.Username;
                    CurrentUserAccount.DisplayName = $"Welcome, {user.Username}";
                    CurrentUserAccount.ProfilePicture = 0; // Default value for byte

            }
            else
            {
                CurrentUserAccount.DisplayName = "Invaild User, not Logged In";
            }
        }

        private void SignOut()
        {
            // Logic to clear user session data if needed

            // Create a new instance of LoginView
            var loginView = new LoginView();
            loginView.Show(); // Show the login window

            // Close the current main window
            Application.Current.MainWindow.Close();
        }
    }
}
