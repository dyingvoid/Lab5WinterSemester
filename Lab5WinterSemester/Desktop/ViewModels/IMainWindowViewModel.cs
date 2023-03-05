using System.Collections.ObjectModel;
using Lab5WinterSemester.Core.TableClasses;


namespace Lab5WinterSemester.Desktop.ViewModels;

public interface IMainWindowViewModel
{
    public void AddDataBase();
    public ObservableCollection<DataBase> DataBases { get; set; }
}