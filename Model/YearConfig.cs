using System.ComponentModel;

namespace Student_Management.Model
{
    public class YearConfig : INotifyPropertyChanged
    {
        private int _yearId;
        private string _yearLevel;
        private string _yearType;
        private int _numberOfSemesters;

        public int YearId
        {
            get => _yearId;
            set
            {
                _yearId = value;
                OnPropertyChanged(nameof(YearId));
            }
        }

        public string YearLevel
        {
            get => _yearLevel;
            set
            {
                _yearLevel = value;
                OnPropertyChanged(nameof(YearLevel));
            }
        }

        public string YearType
        {
            get => _yearType;
            set
            {
                _yearType = value;
                OnPropertyChanged(nameof(YearType));
            }
        }

        public int NumberOfSemesters
        {
            get => _numberOfSemesters;
            set
            {
                _numberOfSemesters = value;
                OnPropertyChanged(nameof(NumberOfSemesters));
            }
        }

        public string DisplayName => $"{YearLevel} ({YearType})";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 