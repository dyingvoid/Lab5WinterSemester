using Lab5WinterSemester.Desktop.Models;

namespace Lab5WinterSemester.Desktop.ViewModels;

public class MainWindowViewModel : Notifier, IMainWindowViewModel
{
    private IMainModel _model;
    
    public MainWindowViewModel(IMainModel model)
    {
        _model = model;
    }
}