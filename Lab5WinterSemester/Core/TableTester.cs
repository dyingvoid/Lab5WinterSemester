using System;
using System.Collections.Generic;
using System.Linq;
using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Core;

public class TableTester : ITester
{
    private AbstractTable _table;

    public TableTester(AbstractTable table)
    {
        _table = table;
    }

    public bool Test()
    {
        CheckStructureEquality(_table.Types, _table.Columns);
        CheckTableDimensionsEquality(_table.Table);
        CheckColumnsDataTypeEquality(_table.Table, _table.Types);
        return false;
    }
    
    private void CheckStructureEquality(Dictionary<string, Type> structure, List<string> columns)
    {
        var structureNames = structure.Keys.ToHashSet();
        var columnNames = columns.ToHashSet();

        if (!structureNames.SetEquals(columnNames))
            throw new Exception("Column names do not match names in json structure.");
    }
    
    private void CheckTableDimensionsEquality(List<List<string?>> table)
    {
        var size = _table.Columns.Count;
        if (size <= 0)
            throw new Exception("Could not find any column in table.");

        for (int i = 0; i < table.Count; ++i)
        {
            if (table[i].Count != size)
            {
                throw new Exception($"Stroke with index {i}(or {i + 2} in file) is size of {table[i].Count}," +
                                    $"when must be {size}");
            }
        }
    }
    
    private void CheckColumnsDataTypeEquality(List<List<string?>> table, Dictionary<string, Type> structure)
    {
        if (table.Count <= 0) return;
        
        // Foreach column
        foreach (var columnName in _table.Columns)
        {
            var column = _table.GetColumnWithName(columnName);
            var columnType = AbstractTable.FindTypeInJsonByColumnName(structure, columnName);
            
            try
            {
                CheckColumnDataTypeEquality(column, columnType);
            }
            catch (Exception)
            {
                Console.WriteLine($"Error in {columnName} column.");
                throw;
            }
        }
    }
    
    private static void CheckColumnDataTypeEquality(List<string?> column, Type type)
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