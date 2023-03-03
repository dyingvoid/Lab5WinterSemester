using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab5WinterSemester.Core.TableClasses;

public class DataBase : IEnumerable, IDataBase
{
    private Dictionary<string, List<object?>> _table;
    private List<Dictionary<string, List<object?>>> _tables;

    public DataBase()
    {
        _table = new Dictionary<string, List<object?>>();
        Types = new Dictionary<string, Type>();
    }

    public DataBase(Dictionary<string, List<object?>> table, 
        Dictionary<string, Type> columnTypes)
    {
        _table = table;
        Types = columnTypes;
    }
    
    public FileInfo SchemaFile { get; set; }
    
    public List<FileInfo> TableFiles { get; set; }
    
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

    public List<object?> GetColumn(int index)
    {
        return _table.ElementAt(index).Value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return _table.GetEnumerator();
    }

    public void Update(IDataBase dataBase)
    {
        SchemaFile = dataBase.SchemaFile;
        Table = dataBase.Table;
        Types = dataBase.Types;
    }
}