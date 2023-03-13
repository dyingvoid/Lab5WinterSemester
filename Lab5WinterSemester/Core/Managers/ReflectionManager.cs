using System;
using System.Globalization;
using System.Reflection;


namespace Lab5WinterSemester.Core.Managers;

public static class ReflectionManager
{
    public static MethodInfo TryMakeGenericWithType(Type type)
    {
        var methodInfo = ChooseGenericMethodByTypeConstraints(type);
            
        return methodInfo;
    }
    
    public static MethodInfo ChooseGenericMethodByTypeConstraints(Type type)
    {
        //bool does not implement IParsable
        if (type == typeof(bool))
            return typeof(ReflectionManager).GetMethod("ToTypeWithBool").MakeGenericMethod(type);
        if (type is { IsValueType: true, IsEnum: false })
            return typeof(ReflectionManager).GetMethod("ToTypeWithStructConstraint").MakeGenericMethod(type);
        if (type.IsEnum)
            return typeof(ReflectionManager).GetMethod("ToTypeEnumConstraint").MakeGenericMethod(type);

        return typeof(ReflectionManager).GetMethod("ToTypeWithClassConstraint").MakeGenericMethod(type);
    }
    
    public static void TryCastToType(Type type, MethodInfo castGenericMethod, object? element)
    {
        castGenericMethod.Invoke(null, new [] { element });
    }
    
    // ToType methods are used in runtime by reflection ChooseGenericMethodByTypeConstraints()
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

    public static bool? ToTypeWithBool<T>(this string? item)
    {
        if (item == null)
        {
            return null;
        }

        return bool.Parse(item);
    }
}