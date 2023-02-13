using System;
using System.Reflection;

namespace Lab5WinterSemester.Core;

public class ReflectionManager
{
    public static MethodInfo TryMakeGenericWithType(Type type)
    {
        try
        {

            var methodInfo = ChooseGenericMethodByTypeConstraints(type);
            
            return methodInfo;
        }
        catch (Exception)
        {
            Console.WriteLine($"{type} type caused error.");
            throw;
        }
    }
    
    public static MethodInfo ChooseGenericMethodByTypeConstraints(Type type)
    {
        if (type.IsValueType && !type.IsEnum)
            return typeof(Extensions).GetMethod("ToTypeWithStructConstraint").MakeGenericMethod(type);
        if (type.IsEnum)
            return typeof(Extensions).GetMethod("ToTypeEnumConstraint").MakeGenericMethod(type);

        return typeof(Extensions).GetMethod("ToTypeWithClassConstraint").MakeGenericMethod(type);
    }
    
    public static void TryCastToType(Type type, MethodInfo castGenericMethod, string? element)
    {
        try
        {
            castGenericMethod.Invoke(null, new object[] { element });
        }
        catch (Exception)
        {
            Console.WriteLine($"Element '{element}' can't be casted to type {type}.");
            throw;
        }
    }
}