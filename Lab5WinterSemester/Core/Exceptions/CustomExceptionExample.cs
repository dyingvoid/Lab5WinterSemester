using System;

namespace Lab5WinterSemester.Core.Exceptions;

public class CustomExceptionExample : Exception
{
    public CustomExceptionExample()
    {
    }

    public CustomExceptionExample(string message) : base(message)
    {
        
    }
}