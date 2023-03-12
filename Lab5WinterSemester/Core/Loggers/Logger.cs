using System.Windows;

namespace Lab5WinterSemester.Core.Loggers;

public class Logger : ILogger
{
    private static Logger _instance = new Logger();
    
    private Logger()
    {
    }

    public static ILogger GetInstance()
    {
        return _instance;
    }

    public void Log(string message)
    {
        var result = MessageBox.Show(message);
    }
}