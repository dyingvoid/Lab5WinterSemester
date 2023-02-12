using System;
using System.Collections.Generic;
using System.Linq;

namespace DBFirstLab;

public class DbStructure
{
    public DbStructure(string jsonString)
    {
        JsonString = jsonString;
    }
    public String JsonString { get; set; }

    public List<string> ToFormat()
    {
        return JsonString.Split(',').ToList();
    }
}