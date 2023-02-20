using System;
using System.Globalization;
using System.Reflection;

namespace Lab5WinterSemester.Core;

public static class ReflectionManager
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
            return typeof(ReflectionManager).GetMethod("ToTypeWithStructConstraint").MakeGenericMethod(type);
        if (type.IsEnum)
            return typeof(ReflectionManager).GetMethod("ToTypeEnumConstraint").MakeGenericMethod(type);

        return typeof(ReflectionManager).GetMethod("ToTypeWithClassConstraint").MakeGenericMethod(type);
    }
    
    public static void TryCastToType(Type type, MethodInfo castGenericMethod, object? element)
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
    
    // Three ToType methods are used in runtime by reflection in CsvTable
    public static T? ToTypeWithStructConstraint<T>(this string? item) where T : struct, IParsable<T>
    {
        if (item == null)
        {
            return null;
        }
        return T.Parse(item, CultureInfo.InvariantCulture);
    }

    public static TEnum? ToTypeEnumConstraint<TEnum>(this string? item) where TEnum : struct
    {
        if (item == null)
        {
            return null;
        }

        return (TEnum)Enum.Parse(typeof(TEnum), item);
    } 

    public static T? ToTypeWithClassConstraint<T>(this string? item) where T : class, IParsable<T>
    {
        if (item == null)
        {
            return null;
        }

        return T.Parse(item, CultureInfo.InvariantCulture);
    }
}