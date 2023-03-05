using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Lab5WinterSemester.Core.TableClasses;
using Microsoft.Win32;

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

    public bool AddDataBase()
    {
        var file = OpenDataBaseFile();
        if (file == null) 
            return false;

        DataBases.Add(CreateDataBase(file));
        return true;
    }

    private FileInfo? OpenDataBaseFile()
    {
        var fileDialog = new OpenFileDialog();
        if (fileDialog.ShowDialog() == true)
        { 
            return new FileInfo(fileDialog.FileName);
        }

        return null;
    }
    
    private DataBase CreateDataBase(FileInfo dataBaseSchemaFile)
    {
        DataBaseSimpleFactory factory = new DataBaseSimpleFactory();
        return factory.CreateDataBase(dataBaseSchemaFile);
    }
}