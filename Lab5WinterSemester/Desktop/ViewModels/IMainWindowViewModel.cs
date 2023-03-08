using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab5WinterSemester.Core.TableClasses;


namespace Lab5WinterSemester.Desktop.ViewModels;

public interface IMainWindowViewModel
{
    ObservableCollection<DataBase> DataBases { get; set; }
    ICommand AddDataBaseCommand { get; }
}