using System;
using System.Collections.Generic;

namespace Lab5WinterSemester.Core.TableClasses;

public interface ITable
{
    string Name { get; set; }
    Dictionary<string, List<object?>> Elements { get; set; }
    public Dictionary<string, Type> Types { get; set; }
    //Tuple(Columns, Strokes)
    public Tuple<int, int> Shape { get; }
    public List<string> Names { get; }
}