using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Core.Testers;

public interface ITester<T>
{
    public bool Test(T dataBase);
}