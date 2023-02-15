using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab5WinterSemester.Core.TableClasses;

public class CsvTable : TableBase<List<List<string?>>>, IEnumerable<List<string?>>
{
    public CsvTable(List<List<string?>> table, CsvTable csv)
    {
        Types = csv.Types;
        Names = csv.Names;
        Shape = new Tuple<long?, long?>(null, null);

        try
        {
            _table = table;
            //GoThroughTests();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + $" Could not create CsvTable with table.");
            _table = new List<List<string?>>();
        }
        
        SetShape();
    }

    public CsvTable(List<List<string?>> table, 
        Dictionary<string, Type> columnTypes, List<string> columnNames, Tuple<long?, long?> shape) 
        : base(table, columnTypes, columnNames, shape)
    {
        
    }

    public object this[int index] => GetColumnWithIndex(Table, index);

    public void SetShape()
    {
        Shape = Tuple.Create<long?, long?>(Names.Count, Table.Count);
    }

    public void MakeEmptyAndSpaceElementsNull()
    {
        foreach (var stroke in _table)
        {
            for (var j = 0; j < stroke.Count; ++j)
            {
                if(stroke[j].IsEmptyOrWhiteSpace())
                    stroke[j] = null;
            }
        }
    }
    
    public List<string?> GetColumnWithName(TContainer table, List<string> columnNames, string columnName)
    {
        var indexOfColumn = columnNames.FindIndex(name=> name == columnName);

        if (indexOfColumn < 0)
        {
            var ex = new ArgumentException();
            ex.Message = $"Could not find column with name {columnName} in Columns";
            throw new ArgumentOutOfRangeException($"Could not find column with name {columnName} in Columns");
        }

        try
        {
            return GetColumnWithIndex(table, indexOfColumn);
        }
        catch (Exception ex)
        {
            Logger.GetInstance().Log(ex, $"Could not find data of {columnName} column. " +
                                         $"Check format of table");
            throw;
        }
    }
    
    public List<string?> GetColumnWithName(string columnName)
    {
        return GetColumnWithName(Table, Names, columnName);
    }
    
    public List<string?> GetColumnWithIndex(TContainer table, int index)
    {
        var column = new List<string?>();

        foreach (var stroke in table)
        {
            column.Add(stroke[index]);
        }

        return column;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public IEnumerator<List<string?>> GetEnumerator()
    {
        var enumerator = _table.GetEnumerator();
        return enumerator;
    }

    public List<string?> At(int index)
    {
        return Table[index];
    }

    // Since method creates new CsvTable, mb it is worth to create Factory pattern.
    public void MergeByColumn(CsvTable csv, string columnName1, string columnName2)
    {
        var thisColumnIndex = Names.FindIndex(name => name == columnName1);
        var csvColumnIndex = csv.Names.FindIndex(name => name == columnName2);

        var elementsIntersection = GetColumnWithIndex(Table, thisColumnIndex)
            .Intersect(GetColumnWithIndex(csv.Table, csvColumnIndex));

        Table = CreateMergedTable(csv, elementsIntersection, csvColumnIndex, thisColumnIndex);

        Names.AddRange(csv.Names);
        Names.Remove(columnName2);
        
        foreach (var (name, type) in csv.Types)
            Types.Add(name, type);
        
        Types.Remove(columnName2);
        
        SetShape();
        //GoThroughTests(); CsvTable must not be reliable for that, there is TableTester.
    }

    private List<List<string?>> CreateMergedTable(CsvTable csv, 
        IEnumerable<string?> elementsIntersection, 
        int csvColumnIndex, 
        int thisColumnIndex)
    {
        var mergedTables = new List<List<string?>>();

        foreach (var element in elementsIntersection)
        {
            if (element != null)
            {
                var csvStrokes = csv.Table
                    .FindAll(stroke => stroke[csvColumnIndex] == element);

                var thisStroke = Table.Find(stroke => stroke[thisColumnIndex] == element);

                foreach (var stroke in csvStrokes)
                {
                    var csvStrokeCopy = new List<string?>(stroke);
                    csvStrokeCopy.RemoveAt(csvColumnIndex);

                    var thisStrokeCopy = new List<string?>(thisStroke);

                    thisStrokeCopy.AddRange(csvStrokeCopy);
                    mergedTables.Add(thisStrokeCopy);
                }
            }
        }

        return mergedTables;
    }
}