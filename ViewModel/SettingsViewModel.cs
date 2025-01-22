using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Student_Management.View;

namespace Student_Management.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _selectedTheme = string.Empty; // Initialize to avoid CS8618
        public List<string> Themes { get; set; }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value) // Check if the value has changed
                {
                    _selectedTheme = value;
                    OnPropertyChanged(nameof(SelectedTheme)); // Pass the property name
                    ChangeTheme(_selectedTheme);
                }
            }
        }

        public SettingsViewModel()
        {
            Themes = new List<string> { "Light", "Dark" };
            SelectedTheme = Themes[0]; // Default to Light theme
        }

        private void ChangeTheme(string theme)
        {
            // Logic to change the application theme
            // This could involve changing resource dictionaries or application styles
            if (theme == "Dark")
            {
                // Set dark theme resources
            }
            else
            {
                // Set light theme resources
            }
        }
    }
} 