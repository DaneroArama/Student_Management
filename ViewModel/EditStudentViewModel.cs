using System.Windows;
using System.Windows.Input;
using Student_Management.Model;
using Student_Management.Repository;
using Student_Management.ViewModel;

public class EditStudentViewModel : ViewModelBase
{
    private readonly StudentRepository _repository;
    private readonly StudentModel _student;
    private ICommand _saveCommand;
    private ICommand _cancelCommand;

    public StudentModel Student { get; }

    public EditStudentViewModel(StudentModel student)
    {
        _repository = new StudentRepository();
        Student = new StudentModel
        {
            Id = student.Id,
            RollNo = student.RollNo,
            Name = student.Name,
            Year = student.Year,
            Semester = student.Semester,
            Phone = student.Phone,
            Address = student.Address
        };
    }

    public ICommand SaveCommand => _saveCommand ??= new RelayCommand(Save);
    public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(Cancel);

    private void Save()
    {
        _repository.UpdateStudent(Student);
        CloseWindow(true);
    }

    private void Cancel()
    {
        CloseWindow(false);
    }

    private void CloseWindow(bool result)
    {
        var window = Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.DataContext == this);
        
        if (window != null)
        {
            window.DialogResult = result;
            window.Close();
        }
    }
} 