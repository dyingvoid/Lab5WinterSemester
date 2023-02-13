using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab5WinterSemester.Core.TableClasses;

public class TableFactory
{
    private List<string> _supportedFileExtensions;
    
    public TableFactory()
    {
        _supportedFileExtensions = new List<string>() { ".csv" };
    }

    public CsvTable? CreateCsvTable(FileInfo tableFile, Dictionary<string, string> configuration)
    {
        try
        {
            List<List<string?>> table = ReadFromFileToList(tableFile);
            var columnTypes = SetColumnTypes(configuration);
            var columnNames = SetColumnNames(table);
            table.RemoveAt(0);
            var shape = SetShape(table, columnNames);
            MakeEmptyAndSpaceElementsNull(table);

            var csvTable = new CsvTable(table, columnTypes, columnNames, shape);

            var tableTester = new TableTester(csvTable);
            tableTester.Test();

            return csvTable;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + $" Could not create CsvTable with {tableFile.Name}.");
            return null;
        }
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
    
    public void MakeEmptyAndSpaceElementsNull(List<List<string>> table)
    {
        foreach (var stroke in table)
        {
            for (var j = 0; j < stroke.Count; ++j)
            {
                if(stroke[j].IsEmptyOrWhiteSpace()) 
                    stroke[j] = null;
            }
        }
    }
    
    private Tuple<long?, long?> SetShape(List<List<string>> table, List<string> columnNames)
    {
        return Tuple.Create<long?, long?>(table.Count, columnNames.Count);
    }
}