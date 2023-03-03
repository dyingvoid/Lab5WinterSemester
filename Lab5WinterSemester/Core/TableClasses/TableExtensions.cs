namespace Lab5WinterSemester.Core.TableClasses;

public static class TableExtensions
{
    public static void MakeEmptyAndSpaceElementsNull(this DataBase data)
    {
        foreach (var pair in data.Table)
        {
            var column = pair.Value;
            for (var i = 0; i < column.Count; ++i)
            {
                if(column[i].IsEmptyOrWhiteSpace())
                    column[i] = null;
            }
        }
    }
    
    public static bool IsEmptyOrWhiteSpace(this object? value)
    {
        var str = value?.ToString();
        return str is "" or " ";
    }
}