using System;
using System.Collections.Generic;
using System.IO;

namespace Lab5WinterSemester.Core.TableClasses;

public interface IDataBase
{
    public FileInfo SchemaFile { get; set; }
    public List<FileInfo> TableFiles { get; set; }
    public Dictionary<string, List<object?>> Table { get; set; }
    public Tuple<int, int> Shape { get; }
    public Dictionary<string, Type> Types { get; set; }
    public List<string> Names { get; }
    public void Update(IDataBase dataBase);
}