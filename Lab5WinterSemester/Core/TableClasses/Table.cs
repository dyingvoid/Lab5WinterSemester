using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lab5WinterSemester.Core.TableClasses;

public class Table : ITable, IEnumerable
{
    public FileInfo DataFile { get; set; }
    public string Name => DataFile.Name;
    public Dictionary<string, List<object?>> Elements { get; set; }
    public Dictionary<string, Type> Types { get; set; }
    public Tuple<int, int> Shape => Tuple.Create(Elements.Count, Elements.First().Value.Count);
    public List<string> Names => Elements.Keys.ToList();
    public DataView GuiElements => ConvertData();

    public Table()
    {
        Elements = new Dictionary<string, List<object?>>();
        Types = new Dictionary<string, Type>();
    }

    public Table(FileInfo dataFile, Dictionary<string, Type> configuration)
    {
        DataFile = dataFile;
        Elements = new Dictionary<string, List<object?>>();
        Types = configuration;
        

        foreach (var (key, type) in configuration)
        {
            Elements.Add(key, new List<object?>());
        }
    }
    
    public List<object?> GetColumn(string columnName)
    {
        return Elements[columnName];
    }

    public List<object?> GetColumn(int index)
    {
        return Elements.ElementAt(index).Value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return Elements.GetEnumerator();
    }
    
    private DataView ConvertData()
    {
        var dataView = new DataView();
        var dataTable = new DataTable();

        foreach (var (key, value) in Elements)
        {
            dataTable.Columns.Add(key, typeof(object));
        }
        
        foreach (var (name, list) in Elements)
        {
            var row = dataTable.NewRow();
            for (var i = 0; i < list.Count; ++i)
            {
                row[i] = list[i];
            }

            dataTable.Rows.Add(row);
        }

        dataView = dataTable.DefaultView;
        
        return dataView;
    }
}