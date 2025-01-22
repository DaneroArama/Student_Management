using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Student_Management.View;
using System.Windows;

namespace Student_Management.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _selectedTheme;
        public List<string> Themes { get; set; }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged(nameof(SelectedTheme));
                    ChangeTheme(_selectedTheme);
                }
            }
        }

        public SettingsViewModel()
        {
            Themes = new List<string> { "Purple", "Cool Tone" };
            SelectedTheme = Themes[0]; // Default to Purple theme
        }
        private void ChangeTheme(string theme)
        {
            ResourceDictionary resourceDict = Application.Current.Resources;

            // Clear existing resources
            resourceDict.MergedDictionaries.Clear();

            if (theme == "Cool Tone")
            {
                // Load the cool tone colors
                var coolToneDict = new ResourceDictionary
                {
                    Source = new Uri("Style/CoolToneColors.xaml", UriKind.Relative)
                };
                resourceDict.MergedDictionaries.Add(coolToneDict);
                MessageBox.Show("Cool Tone theme applied."); // Debug message
            }
            else
            {
                // Load the original colors (assuming you have a default colors file)
                var defaultDict = new ResourceDictionary
                {
                    Source = new Uri("Style/UIColours.xaml", UriKind.Relative)
                };
            }
        }
    }
} 