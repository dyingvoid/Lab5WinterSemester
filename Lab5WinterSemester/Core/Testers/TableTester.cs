using System;
using System.Collections.Generic;
using System.Linq;
using Lab5WinterSemester.Core.Loggers;
using Lab5WinterSemester.Core.Managers;
using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Core.Testers;

public class TableTester : ITester
{
    private TableBase _table;

    public TableTester(TableBase table)
    {
        _table = table;
    }

    private delegate TV MyDelegate<TU, out TV>( out TU output);
    
    public bool Test()
    {
        var testList = SelectTests();
        foreach (var test in testList)
        {
            var state = test(out var errorMessage);
            Logger.GetInstance().Log(new Exception(errorMessage));
        }
        
        return false;
    }

    private List<MyDelegate<string, bool>> SelectTests()
    {
        var testList = new List<MyDelegate<string, bool>>
        {
            CheckStructureEquality,
            CheckTableDimensionsEquality,
            CheckColumnsDataTypeEquality
        };

        return testList;
    }

    private bool CheckStructureEquality(out string errorMessage)
    {
        var structureNames = _table.Types.Keys.ToHashSet();
        var columnNames = _table.Names.ToHashSet();

        errorMessage = "";
        return structureNames.SetEquals(columnNames);
    }
    
    private bool CheckTableDimensionsEquality(out string errorMessage)
    {
        var length = _table.Table.First().Value.Count;
        
        foreach (var (key, column) in _table.Table)
        {
            if (column.Count != length)
            {
                errorMessage = $"Wrong size of dimensions. Column '{key}' with size of {column.Count} was met.";
                return false;
            }
        }

        errorMessage = "";
        return true;
    }
    
    private bool CheckColumnsDataTypeEquality(out string errorMessage)
    {
        // Foreach column
        foreach (var (key, column) in _table.Table)
        {
            var state = _table.Types.TryGetValue(key, out var columnType);
            
            try
            {
                CheckColumnDataTypeEquality(column, columnType);
            }
            catch (Exception)
            {
                errorMessage = $"Error in {key} column.";
                return false;
            }
        }

        errorMessage = "";
        return true;
    }
    
    private void CheckColumnDataTypeEquality(List<object?> column, Type type)
    {
        foreach (var element in column)
        {
            if (type != typeof(String))
            {
                var castGenericMethod = ReflectionManager.TryMakeGenericWithType(type);
                ReflectionManager.TryCastToType(type, castGenericMethod, element);
            }
        }
    }
}