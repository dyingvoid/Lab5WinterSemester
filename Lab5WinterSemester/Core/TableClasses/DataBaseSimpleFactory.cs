using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab5WinterSemester.Core.Loggers;
using Lab5WinterSemester.Core.Managers;
using Lab5WinterSemester.Core.Testers;

namespace Lab5WinterSemester.Core.TableClasses;

public class DataBaseSimpleFactory
{
    private List<string> _supportedFileExtensions;
    private FileInfo _dataBaseSchemaFile;
    private Dictionary<FileInfo, Dictionary<string, Type?>> _config;

    public DataBaseSimpleFactory()
    {
        _supportedFileExtensions = new List<string>() { ".csv" };
    }
    
    public DataBase CreateDataBase(FileInfo dataBaseFile)
    {
        _dataBaseSchemaFile = dataBaseFile;
        _config = ConfigManager.ParseJson(dataBaseFile);
        
        var tables = CreateTables();
        var dataBase = new DataBase(tables, _config, _dataBaseSchemaFile);
        var tester = new TableTester(Logger.GetInstance());
        
        return tester.Test(dataBase) ? dataBase : new DataBase();
    }

    private List<Table> CreateTables()
    {
        var list = new List<Table>();
        
        foreach (var (tableFile, types) in _config)
        {
            list.Add(CreateTable(tableFile, types));
        }

        return list;
    }

    private Table CreateTable(FileInfo tableFile, Dictionary<string, Type> tableConfiguration)
    {
        var table = new Table(tableFile, tableConfiguration);
        var data = File.ReadAllLines(tableFile.FullName);

        foreach (var str in data)
        {
            var list = str.Split(",").ToList();
            foreach (var (key, value) in Enumerable.Zip(table.Elements.Keys, list))
            {
                table.Elements[key].Add(value);
            }
        }

        return table;
    }
}