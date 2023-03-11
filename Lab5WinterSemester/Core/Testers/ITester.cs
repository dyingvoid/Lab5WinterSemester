using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Core.Testers;

public interface ITester
{
    public bool Test(ITable table);
    public bool Test(IDataBase dataBase);
}