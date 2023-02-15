using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lab5WinterSemester.Core.Loggers;

namespace Lab5WinterSemester.Core.TableClasses;

public class TableBase : IEnumerable
{
    private Dictionary<string, List<object?>> _table;

    public TableBase()
    {
        Types = new Dictionary<string, Type>();
    }

    public TableBase(Dictionary<string, List<object?>> table, 
        Dictionary<string, Type> columnTypes)
    {
        Table = table;
        Types = columnTypes;
    }
    
    public Dictionary<string, List<object?>> Table
    {
        get => _table;
        set => _table = value;
    }

    /// <summary>
    /// Number of columns, number of strokes
    /// </summary>
    public Tuple<int, int> Shape => Tuple.Create(_table.Count, _table.First().Value.Count);
    
    public Dictionary<string, Type> Types { get; set; }

    public List<string> Names => _table.Keys.ToList();

    public List<object?> GetColumn(string columnName)
    {
        return _table[columnName];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return _table.GetEnumerator();
    }

    public Type FindTypeInJsonByColumnName(string name)
    {
        try
        {
            var type = Types[name];
            return type;
        }
        catch (Exception ex)
        {
            Logger.GetInstance().Log(ex, $"Could not find {name} column in json structure.");
            throw;
        }
    }
}