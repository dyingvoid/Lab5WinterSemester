using System;
using System.Collections.Generic;

namespace Lab5WinterSemester.Core.Loggers;

public class Logger
{
    private static Logger _instance = new Logger();
    private List<Exception> _exceptions;
    private Logger()
    {
        _exceptions = new List<Exception>();
    }

    public static Logger GetInstance()
    {
        return _instance;
    }

    public void Log(Exception exception)
    {
        _exceptions.Add(exception);
    }

    public void Log(Exception exception, string extraMessage)
    {
        _exceptions.Add(exception);
        Console.WriteLine(extraMessage);
    }

    public void Print()
    {
        foreach (var exception in _exceptions)
        {
            Console.WriteLine(exception.Message);
        }
    }
}