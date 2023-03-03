using System;
using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Desktop.Models;

public class DataBaseEventArgs : EventArgs
{
    public IDataBase DataBase { get; set; }

    public DataBaseEventArgs(IDataBase dataBase)
    {
        DataBase = dataBase;
    }
}