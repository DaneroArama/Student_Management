using System;
using System.Threading;
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
        private readonly IUserRepository _userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

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

        // Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowStudentViewCommand { get; }
        public ICommand ShowAttendanceViewCommand { get; }
        public ICommand ShowSettingsViewCommand { get; }
        public ICommand ShowMarksViewCommand { get; }
        public ICommand SignOutCommand { get; }

        public MainViewModel()
        {
            try
            {
                _userRepository = new UserRepository();
                CurrentUserAccount = new UserAccountModel();

                // Initialize Commands
                ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
                ShowStudentViewCommand = new ViewModelCommand(ExecuteShowStudentViewCommand);
                ShowAttendanceViewCommand = new ViewModelCommand(ExecuteShowAttendanceViewCommand);
                ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);
                ShowMarksViewCommand = new ViewModelCommand(ExecuteShowMarksViewCommand);
                SignOutCommand = new RelayCommand(SignOut);

                // Default View
                ExecuteShowHomeViewCommand(null);
                LoadCurrentUserData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing MainViewModel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new HomeViewModel();
                Caption = "Dashboard";
                Icon = IconChar.Home;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Home view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowStudentViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new StudentsViewModel();
                Caption = "Students";
                Icon = IconChar.UserGraduate;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Students view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowAttendanceViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new AttendanceViewModel();
                Caption = "Attendance";
                Icon = IconChar.CalendarCheck;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Attendance view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowSettingsViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new AcademicEditViewModel();
                Caption = "Academic";
                Icon = IconChar.BookBookmark;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Academic view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowMarksViewCommand(object obj)
        {
            try
            {
                CurrentChildView = new MarksViewModel();
                Caption = "Marks";
                Icon = IconChar.Marker;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Marks view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCurrentUserData()
        {
            try
            {
                if (Thread.CurrentPrincipal?.Identity?.Name != null)
                {
                    var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
                    if (user != null)
                    {
                        CurrentUserAccount.Username = user.Username;
                        CurrentUserAccount.DisplayName = $"Welcome, {user.Username}";
                        CurrentUserAccount.ProfilePicture = 0;
                    }
                    else
                    {
                        CurrentUserAccount.DisplayName = "Invalid User, not Logged In";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignOut()
        {
            try
            {
                var loginView = new LoginView();
                loginView.Show();
                Application.Current.MainWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during sign out: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
