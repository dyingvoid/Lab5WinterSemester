﻿using System;
using System.Collections.ObjectModel;
using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Desktop.Models;

public interface IMainModel
{
    public ObservableCollection<DataBase> DataBases { get; set; }
    event EventHandler<DataBaseEventArgs> DataBaseUpdated;
    void UpdateDataBase(IDataBase updatedDataBase);
}