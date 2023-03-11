using System;
using System.Collections.Generic;
using System.Windows;

namespace Lab5WinterSemester.Core.Loggers;

public class Logger : ILogger
{
    private static Logger _instance = new Logger();
    private List<Exception> _exceptions;
    private Logger()
    {
        _exceptions = new List<Exception>();
    }

    public static ILogger GetInstance()
    {
        return _instance;
    }

    public void Log(Exception exception)
    {
        _exceptions.Add(exception);
    }

    public void Log(string message)
    {
        var result = MessageBox.Show(message);
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