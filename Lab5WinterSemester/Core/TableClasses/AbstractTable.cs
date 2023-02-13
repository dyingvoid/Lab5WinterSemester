using System;
using System.Collections.Generic;

namespace Lab5WinterSemester.Core.TableClasses;

public abstract class AbstractTable
{
    protected List<List<string?>> _table;

    protected AbstractTable()
    {
        Types = new Dictionary<string, Type>();
        Columns = new List<string>();
        Shape = new Tuple<long?, long?>(null, null);
    }

    public AbstractTable(List<List<string?>> table, 
        Dictionary<string, Type> columnTypes, List<string> columnNames, Tuple<long?, long?> shape)
    {
        Table = table;
        Types = columnTypes;
        Columns = columnNames;
        Shape = shape;
    }
    
    public List<List<string?>> Table
    {
        get => _table;
        set => _table = value;
    }
    
    /// <summary>
    /// Number of columns, number of strokes
    /// </summary>
    public Tuple<long?, long?> Shape { get; set; }
    
    public Dictionary<string, Type> Types { get; set; }
    
    public List<string> Columns { get; set; }
    
    public List<string?> GetColumnWithName(List<List<string?>> table, List<string> columnNames, string columnName)
    {
        var indexOfColumn = columnNames.FindIndex(name=> name == columnName);
        
        if (indexOfColumn < 0)
            throw new Exception($"Could not find column with name {columnName} in Columns");

        try
        {
            return GetColumnWithIndex(table, indexOfColumn);
        }
        catch (Exception)
        {
            Console.WriteLine($"Could not find data of {columnName} column. Check format of table");
            throw;
        }
    }
    
    public List<string?> GetColumnWithName(string columnName)
    {
        return GetColumnWithName(Table, Columns, columnName);
    }
    
    public List<string?> GetColumnWithIndex(List<List<string?>> table, int index)
    {
        var column = new List<string?>();

        foreach (var stroke in table)
        {
            column.Add(stroke[index]);
        }

        return column;
    }
    
    public static Type FindTypeInJsonByColumnName(Dictionary<string, Type> structure, string? name)
    {
        try
        {
            var type = structure[name];
            return type;
        }
        catch (Exception)
        {
            Console.WriteLine($"Could not find {name} column in json structure.");
            throw;
        }
    }
}