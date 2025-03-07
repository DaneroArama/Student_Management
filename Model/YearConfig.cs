using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Student_Management.Model;

public class YearConfig : INotifyPropertyChanged
{
    private int _id;
    private string _yearName;
    private int _numberOfSemesters;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public string YearName
    {
        get => _yearName;
        set
        {
            _yearName = value;
            OnPropertyChanged(nameof(YearName));
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 