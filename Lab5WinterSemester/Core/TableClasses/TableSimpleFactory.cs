using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab5WinterSemester.Core.Testers;

namespace Lab5WinterSemester.Core.TableClasses;

public class TableSimpleFactory
{
    private List<string> _supportedFileExtensions;
    
    public TableSimpleFactory()
    {
        _supportedFileExtensions = new List<string>() { ".csv" };
    }

    private TableBase CreateTable(FileInfo tableFile, Dictionary<string, string> configuration)
    {
        var table = ReadFromFileToDict(tableFile, configuration);
        var columnTypes = SetColumnTypes(configuration);
        MakeEmptyAndSpaceElementsNull(table);

        var tableBase = new TableBase(table, columnTypes);

        var tester = new TableTester(tableBase);
        
        return tester.Test() ? tableBase : new TableBase();
    }

    private List<List<string>> ReadFromFileToList(FileInfo tableFile)
    {
        if (!_supportedFileExtensions.Any(name => tableFile.Name.EndsWith(name)))
            throw new Exception("Can't read non csv file.");
        
        var tempCsvTable = File.ReadAllLines(tableFile.FullName)
            .ToList()
            .PureForEach<List<string>, string, List<List<string>>, List<string>>
                (line => line.Split(',').ToList());
        
        return tempCsvTable;
    }

    private Dictionary<string, List<object?>> ReadFromFileToDict(FileInfo tableFile, Dictionary<string, string> configuration)
    {
        if (!_supportedFileExtensions.Any(name => tableFile.Name.EndsWith(name)))
            throw new Exception("Can't read non csv file.");

        var lines = File.ReadAllLines(tableFile.FullName).ToList();

        return new Dictionary<string, List<object?>>();
    }

    private Dictionary<string, Type> SetColumnTypes(Dictionary<string, string> typesConfig)
    {
        var types = new Dictionary<string, Type>();
        foreach (var (key, value) in typesConfig)
        {
            var type = Type.GetType(value);
            try
            {
                types.Add(key, type);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not set column types.");
                throw;
            }
        }

        return types;
    }
    
    private List<String> SetColumnNames(List<List<string>> table)
    {
        try
        {
            return table[0];
        }
        catch (Exception)
        {
            Console.WriteLine("Could not set table columns.");
            throw;
        }
    }

    public void MakeEmptyAndSpaceElementsNull(Dictionary<string, List<object?>> table)
    {
        foreach (var (key, column) in table)
        {
            for (var i = 0; i < column.Count; i++)
            {
                if (column[i] is not null && column[i].ToString().IsEmptyOrWhiteSpace())
                    column[i] = null;
            }
            
        }
    }
}