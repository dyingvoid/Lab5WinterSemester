using System;

namespace Lab5WinterSemester.Core;

public class CustomExceptionExample : Exception
{
    public CustomExceptionExample()
    {
    }

    public CustomExceptionExample(string message) : base(message)
    {
        
    }
}