using System;
using System.Collections.Generic;
using System.IO;

namespace Lab5WinterSemester.Core.TableClasses;

public class DataBase : IDataBase
{
    public DataBase()
    {
        Tables = new List<Table>();
    }

    public DataBase(List<Table> tables, Dictionary<FileInfo, Dictionary<string, Type?>> config, FileInfo schemaFile)
    {
        Tables = tables;
        Config = config;
        SchemaFile = schemaFile;
    }

    public FileInfo SchemaFile { get; set; }
    public string SchemaFileName => SchemaFile.Name;
    public List<Table> Tables { get; set; }
    public Dictionary<FileInfo, Dictionary<string, Type?>> Config { get; set; }


    public void Update(IDataBase dataBase)
    {
        Tables = dataBase.Tables;
        Config = dataBase.Config;
    }
}