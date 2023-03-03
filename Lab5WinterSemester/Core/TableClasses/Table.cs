using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab5WinterSemester.Core.TableClasses;

public class Table : ITable, IEnumerable
{
    public Dictionary<string, List<object?>> Elements { get; set; }
    public Dictionary<string, Type> Types { get; set; }
    public Tuple<int, int> Shape => Tuple.Create(Elements.Count, Elements.First().Value.Count);
    public List<string> Names => Elements.Keys.ToList();

    public Table()
    {
        Elements = new Dictionary<string, List<object?>>();
        Types = new Dictionary<string, Type>();
    }

    public Table(Dictionary<string, Type> configuration)
    {
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
}