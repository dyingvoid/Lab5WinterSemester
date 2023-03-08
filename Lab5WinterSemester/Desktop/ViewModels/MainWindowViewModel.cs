using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab5WinterSemester.Core.TableClasses;
using Lab5WinterSemester.Desktop.Models;

namespace Lab5WinterSemester.Desktop.ViewModels;

public class MainWindowViewModel : Notifier, IMainWindowViewModel
{
    private IMainModel _model;
    private ICommand _addDataBaseCommand;

    public ICommand AddDataBaseCommand
    {
        get
        {
            if (_addDataBaseCommand == null)
            {
                _addDataBaseCommand = new RelayCommand(
                    param => AddDataBase(),
                    param => true
                    );
            }
            return _addDataBaseCommand;
        }
    }

    public MainWindowViewModel(IMainModel model)
    {
        _model = model;
        DataBases = _model.DataBases;
    }

    public ObservableCollection<DataBase> DataBases { get; set; }

    private void AddDataBase()
    {
        _model.AddDataBase();
    }
}