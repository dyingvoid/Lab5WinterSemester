using System;
using System.Collections.Generic;
using System.IO;

namespace Lab5WinterSemester.Core.TableClasses;

public interface ITable
{
    FileInfo DataFile { get; set; }
    string Name { get; }
    Dictionary<string, List<object?>> Elements { get; set; }
    public Dictionary<string, Type> Types { get; set; }
    //Tuple(Columns, Strokes)
    public Tuple<int, int> Shape { get; }
    public List<string> Names { get; }
}