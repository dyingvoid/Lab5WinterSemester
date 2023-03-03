using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Lab5WinterSemester.Core.Managers;

public static class ConfigManager
{
    public static int NumberOfFieldsAndProps<TObj>()
    {
        var objType = typeof(TObj);
        int numberFields = objType.GetFields().Length;
        int numberProperties = objType.GetProperties().Length;
        return numberFields + numberProperties;
    }

    public static Dictionary<FileInfo, Dictionary<string, Type?>> ParseJson(FileInfo file)
    {
        using StreamReader stream = new StreamReader(file.FullName);
        string json = stream.ReadToEnd();
        var jsonStringDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

        var jsonFileInfoDict = ConvertDict(jsonStringDict);
        
        return jsonFileInfoDict;
    }

    private static Dictionary<FileInfo, Dictionary<string, Type?>> ConvertDict(
        Dictionary<string, Dictionary<string, string>> dictToConvert)
    {
        var jsonFileInfoDict = new Dictionary<FileInfo, Dictionary<string, Type?>>();

        foreach (var (str, dict) in dictToConvert)
        {
            var fileInfo = new FileInfo(str);
            var typeDict = GetTypes(dict);

            jsonFileInfoDict.TryAdd(fileInfo, typeDict);
        }

        return jsonFileInfoDict;
    }

    private static Dictionary<string, Type?> GetTypes(Dictionary<string, string> dict)
    {
        var types = new Dictionary<string, Type?>();
        foreach (var (name, stringType) in dict)
        {
            var type = Type.GetType(stringType);
            types.TryAdd(name, type);
        }

        return types;
    }
}