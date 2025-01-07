using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Student_Management.Model;
using Student_Management.Repository;

namespace Student_Management.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
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

        public MainViewModel()
        {
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
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
    }
}
