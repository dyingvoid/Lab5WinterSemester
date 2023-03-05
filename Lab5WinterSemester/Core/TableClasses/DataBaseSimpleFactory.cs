using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        var tables = GetTables();
        var dataBase = new DataBase(tables, _config, _dataBaseSchemaFile);


        return dataBase;
    }

    private List<Table> GetTables()
    {
        var list = new List<Table>();
        
        foreach (var (tableFile, types) in _config)
        {
            list.Add(GetTable(tableFile, types));
        }

        return list;
    }

    private Table GetTable(FileInfo tableFile, Dictionary<string, Type> tableConfiguration)
    {
        var table = new Table(tableConfiguration);
        var data = File.ReadAllLines(tableFile.FullName);

        foreach (var str in data)
        {
            var list = str.Split(",").ToList();
            foreach (var (key, value) in Enumerable.Zip(table.Elements.Keys, list))
            {
                table.Elements[key].Add(value);
            }
        }

        //var tester = new TableTester(table);
        
        return table;
    }
}