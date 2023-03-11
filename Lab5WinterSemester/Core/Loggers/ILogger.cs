using System;

namespace Lab5WinterSemester.Core.Loggers;

public interface ILogger
{
    public void Log(Exception exception);
    public void Log(string message);
}