using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab5WinterSemester.Core.TableClasses;


namespace Lab5WinterSemester.Desktop.ViewModels;

public interface IMainWindowViewModel
{
    public void AddDataBase();
    public ObservableCollection<DataBase> DataBases { get; set; }
    public void AddDataBase(object sender, ExecutedRoutedEventArgs e);
}