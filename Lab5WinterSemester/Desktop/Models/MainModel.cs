using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Lab5WinterSemester.Core.TableClasses;
namespace Lab5WinterSemester.Desktop.Models;

public class MainModel : IMainModel
{
    public ObservableCollection<DataBase> DataBases { get; set; }
    public event EventHandler<DataBaseEventArgs> DataBaseUpdated = delegate { };

    public MainModel()
    {
        DataBases = new ObservableCollection<DataBase>();
    }

    public void UpdateDataBase(IDataBase updatedDataBase)
    {
        GetDataBase(updatedDataBase.SchemaFile)?.Update(updatedDataBase);
        DataBaseUpdated(this, new DataBaseEventArgs(updatedDataBase));  
    }
    
    private DataBase? GetDataBase(FileInfo dataBaseSchemaFile)
    {
        return DataBases.FirstOrDefault(dataBase => dataBase.SchemaFile == dataBaseSchemaFile);
    }
}