using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lab5WinterSemester.Core.Loggers;
using Lab5WinterSemester.Core.Managers;
using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Core.Testers;

public class TableTester : ITester
{
    private ITable _table;

    public TableTester(ILogger logger)
    {
        Logger = logger;
    }

    public bool Test(ITable table)
    {
        _table = table;
        testFailures += table.Name + "\n";
        
        var answer= CheckStructureEquality() &&
               CheckTableDimensionsEquality() &&
               CheckColumnsDataTypeEquality();
        
        if(!answer)
            Logger.Log(testFailures);

        return answer;
    }

    public bool Test(IDataBase dataBase)
    {
        bool answer = true;
        
        foreach (var db in dataBase.Tables)
        {
            answer = answer && Test(db);
        }

        return answer;
    }
    
    public ILogger Logger { get; set; }
    public string testFailures { get; private set; }

    private bool CheckStructureEquality()
    {

        var extraColumns = _table.Elements.Keys.Except(_table.Types.Keys).ToArray();
        var missingColumns = _table.Types.Keys.Except(_table.Elements.Keys).ToArray();

        testFailures += "Extra columns not found in config: " + String.Join(", ", extraColumns) +
                        "\nMissing columns: " + String.Join(", ", missingColumns) + "\n";


        return extraColumns.Length == 0 && missingColumns.Length == 0;
    }
    
    private bool CheckTableDimensionsEquality()
    {
        var wrongSizedColumns = new List<string>();
        
        foreach (var (key, column) in _table.Elements)
        {
            if (column.Count != _table.Shape.Item2)
            {
                wrongSizedColumns.Add(key);
            }
        }

        testFailures += "Columns of wrong size(size is set according to first column): " +
                        String.Join(", ", wrongSizedColumns) + "\n";

        return wrongSizedColumns.Count == 0;
    }
    
    private bool CheckColumnsDataTypeEquality()
    {
        var columnsWithWrongTypeElements = new List<string>();
        List<object?> wrongElements = new List<object?>();
        
        foreach (var (columnName, column) in _table.Elements)
        {
            var state = _table.Types.TryGetValue(columnName, out var columnType);
            
            if(!CheckColumnDataTypeEquality(column, columnType, out wrongElements))
                columnsWithWrongTypeElements.Add(columnName);
        }

        testFailures += "Columns with elements of wrong type: " +
                        String.Join(", ", columnsWithWrongTypeElements) + "\n" +
                        "Wrong elements: " + String.Join(", ", wrongElements) + "\n";

        return columnsWithWrongTypeElements.Count == 0;
    }
    
    private bool CheckColumnDataTypeEquality(List<object?> column, Type type, out List<object?> wrongElements)
    {
        wrongElements = new List<object?>();
        bool answer = true;
        
        foreach (var element in column)
        {
            if (type == typeof(String)) continue;
            
            MethodInfo? castGenericMethod = null;
            try
            {
                castGenericMethod = ReflectionManager.TryMakeGenericWithType(type);
            }
            catch (Exception)
            {
                testFailures += "Unknown type was met: " + type;
                answer = false;
            }

            try
            {
                ReflectionManager.TryCastToType(type, castGenericMethod, element);
            }
            catch (Exception)
            {
                wrongElements.Add(element);
                answer = false;
            }
        }

        return answer;
    }
}