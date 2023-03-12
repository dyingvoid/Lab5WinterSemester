using System;
using System.Collections.Generic;
using System.IO;

namespace Lab5WinterSemester.Core.TableClasses;

public interface IDataBase
{
    FileInfo SchemaFile { get; set; }
    public List<Table> Tables { get; set; }
    Dictionary<FileInfo, Dictionary<string, Type>> Config { get; set; }
}