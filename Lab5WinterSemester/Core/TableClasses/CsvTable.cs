using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab5WinterSemester.Core.TableClasses;

public class CsvTable : AbstractTable, IEnumerable<List<string?>>
{
    public CsvTable(List<List<string?>> table, CsvTable csv)
    {
        Types = csv.Types;
        Columns = csv.Columns;
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
        Shape = Tuple.Create<long?, long?>(Columns.Count, Table.Count);
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
        var thisColumnIndex = Columns.FindIndex(name => name == columnName1);
        var csvColumnIndex = csv.Columns.FindIndex(name => name == columnName2);

        var elementsIntersection = GetColumnWithIndex(Table, thisColumnIndex)
            .Intersect(GetColumnWithIndex(csv.Table, csvColumnIndex));

        Table = CreateMergedTable(csv, elementsIntersection, csvColumnIndex, thisColumnIndex);

        Columns.AddRange(csv.Columns);
        Columns.Remove(columnName2);
        
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

    public void Print()
    {
        if (Table.Count == 0 && Columns.Count == 0)
        {
            Console.WriteLine("Empty");
            return;
        }

        var columnWidths = new List<int>();
        foreach (var column in Columns)
        {
            columnWidths.Add(GetColumnWidth(column));
        }

        PrintColumns(columnWidths);
        PrintContent(columnWidths);
        PrintBorder(columnWidths);
    }

    private void PrintContent(List<int> columnWidths)
    {
        foreach (var stroke in this)
        {
            var enumerator = columnWidths.GetEnumerator();
            enumerator.MoveNext();

            for (var i = 0; i < stroke.Count; ++i)
            {
                if(stroke[i] == null)
                    Console.Write($"|n/a" + new string(' ', enumerator.Current - 3));
                else
                    Console.Write($"|{stroke[i]}" + new string(' ', enumerator.Current - stroke[i].Length));
                
                enumerator.MoveNext();
            }

            Console.WriteLine('|');
        }
    }

    private void PrintBorder(List<int> widths)
    {
        var lens = new List<int>() {0};
        int len = 0;
        
        foreach (var width in widths)
        {
            len += width + 1;
            lens.Add(len);
        }
        
        for (var i = 0; i < widths.Sum() + widths.Count + 1; ++i)
        {
            if(lens.Contains(i))
                Console.Write('|');
            else
                Console.Write('-');
        }
        
        Console.WriteLine();
    }

    private void PrintColumns(List<int> columnWidths)
    {
        int length = 0;
        List<int> lens = new List<int>();
        
        foreach (var (width, name) in columnWidths.Zip(Columns))
        {
            string output = $"|{name}" + new string(' ', width - name.Length);
            length += output.Length;
            lens.Add(length);
            Console.Write(output);
        }
        Console.WriteLine("|");
        Console.Write("|");

        for (var i = 0; i < length; ++i)
        {
            if(lens.Contains(i+1))
                Console.Write('|');
            else
                Console.Write('-');
        }
        Console.WriteLine();
    }
    
    private int GetColumnWidth(string columnName)
    {
        var column = GetColumnWithName(columnName);

        int length = 0;
        foreach (var element in column)
        {
            if (element!= null && element.Length > length)
                length = element.Length;
        }

        try
        {
            var smt = Columns.Find(name => name == columnName).Length;
        }
        catch (Exception)
        {
            Console.WriteLine($"There is no column with name {columnName}");
            throw;
        }

        return Math.Max(length, Columns.Find(name => name == columnName).Length);
    }
}