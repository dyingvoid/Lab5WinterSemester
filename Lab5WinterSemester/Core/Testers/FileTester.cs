using System.IO;
using Lab5WinterSemester.Core.Loggers;

namespace Lab5WinterSemester.Core.Testers;

public class FileTester : ITester<FileInfo>
{
    private ILogger _logger;
    
    public FileTester(ILogger logger)
    {
        _logger = logger;
    }

    public bool Test(FileInfo tableFile)
    {
        var data = File.ReadAllLines(tableFile.FullName);

        var length = 0;
        if (data.Length > 0)
            length = data[0].Split(",").Length;

        for (var i = 0; i < data.Length; ++i)
        {
            var strArrLength = data[i].Split(",").Length;
            if (length != strArrLength)
            {
                _logger.Log($"File {tableFile.Name} with wrong dimensions. Found {i+1} row of size {strArrLength}, " +
                            $"whereas 1st row of size {length}.\n");
                
                return false;
            }
        }

        return true;
    }
}