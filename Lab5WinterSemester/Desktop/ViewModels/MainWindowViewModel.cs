using System.Collections.ObjectModel;
using System.IO;
using Lab5WinterSemester.Core.TableClasses;
using Lab5WinterSemester.Desktop.Models;
using Microsoft.Win32;

namespace Lab5WinterSemester.Desktop.ViewModels;

public class MainWindowViewModel : Notifier, IMainWindowViewModel
{
    private IMainModel _model;
    private RelayCommand? _addDataBaseCommand;

    public MainWindowViewModel(IMainModel model)
    {
        _model = model;
        GetDataBases();
    }

    public RelayCommand AddDataBaseCommand
    {
        get
        {
            return _addDataBaseCommand ??= new RelayCommand(obj =>
            {
                var fileDialog = new OpenFileDialog();
                var file = new FileInfo(fileDialog.FileName);
                
            });
        }
    }

    public ObservableCollection<IDataBase> DataBases { get; set; }

    private void GetDataBases()
    {
        foreach (IDataBase db in _model.DataBases)
        {
            DataBases.Add(db);
        }
    }

    public void AddDataBase()
    {
        
    }
}