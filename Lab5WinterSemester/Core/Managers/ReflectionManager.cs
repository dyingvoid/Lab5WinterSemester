using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Automation.Peers;
using Lab5WinterSemester.Core.TableClasses;

namespace Lab5WinterSemester.Core.Managers;

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
        if (type is { IsValueType: true, IsEnum: false })
            return typeof(ReflectionManager).GetMethod("ToTypeWithStructConstraint").MakeGenericMethod(type);
        if (type.IsEnum)
            return typeof(ReflectionManager).GetMethod("ToTypeEnumConstraint").MakeGenericMethod(type);

        return typeof(ReflectionManager).GetMethod("ToTypeWithClassConstraint").MakeGenericMethod(type);
    }
    
    public static void TryCastToType(Type type, MethodInfo castGenericMethod, object? element)
    {
        try
        {
            castGenericMethod.Invoke(null, new [] { element });
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

    public static ObservableCollection<object> CreateType(ITable table)
    {
        // This code creates an assembly that contains one type,
        // named "MyDynamicType", that has a private field, a property
        // that gets and sets the private field, constructors that
        // initialize the private field, and a method that multiplies
        // a user-supplied number by the private field value and returns
        // the result.

        AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
        AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);

        // The module name is usually the same as the assembly name.
        ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
        
        TypeBuilder tb = mb.DefineType(table.Name, TypeAttributes.Public);
        
        // The property "set" and property "get" methods require a special
        // set of attributes.
        MethodAttributes getSetAttr = MethodAttributes.Public |
                                      MethodAttributes.SpecialName | MethodAttributes.HideBySig;
        
        var fieldList = new List<FieldBuilder>();
        var propertyList = new List<PropertyBuilder>();
        var getterList = new List<MethodBuilder>();
        foreach (var (columnName, type) in table.Types)
        {
            // Add a private field of type int (Int32).
            FieldBuilder field = tb.DefineField("_"+columnName, type, FieldAttributes.Private);
            fieldList.Add(field);
            
            // Define a property named Number that gets and sets the private
            // field.
            //
            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use the built-in array with no elements: Type.EmptyTypes)
            PropertyBuilder property = tb.DefineProperty(columnName, 
                PropertyAttributes.HasDefault, 
                type, 
                null);
            propertyList.Add(property);
            
            // Define the "get" accessor method for Number. The method returns
            // an integer and has no arguments. (Note that null could be
            // used instead of Types.EmptyTypes)
            MethodBuilder getAccessor = tb.DefineMethod(
                "get_"+columnName,
                getSetAttr,
                type,
                Type.EmptyTypes);
            getterList.Add(getAccessor);
            
            ILGenerator numberGetIL = getAccessor.GetILGenerator();
            // For an instance property, argument zero is the instance. Load the
            // instance, then load the private field and return, leaving the
            // field value on the stack.
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, field);
            numberGetIL.Emit(OpCodes.Ret);
            
            // Define the "set" accessor method for Number, which has no return
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_"+columnName,
                getSetAttr,
                null,
                new Type[] { typeof(int) });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, field);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the
            // PropertyBuilder. The property is now complete.
            property.SetGetMethod(getAccessor);
            property.SetSetMethod(mbNumberSetAccessor);
        }

        // Define a constructor that takes an integer argument and
        // stores it in the private field.
        //Type[] parameterTypes = { typeof(int) };
        Type[] parameterTypes = table.Types.Values.ToArray();
        ConstructorBuilder ctor1 = tb.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            parameterTypes);

        ILGenerator ctor1IL = ctor1.GetILGenerator();
        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before calling the base
        // class constructor. Specify the default constructor of the
        // base class (System.Object) by passing an empty array of
        // types (Type.EmptyTypes) to GetConstructor.
        ctor1IL.Emit(OpCodes.Ldarg_0);
        ctor1IL.Emit(OpCodes.Call,
            typeof(object).GetConstructor(Type.EmptyTypes));
        // Push the instance on the stack before pushing the argument
        // that is to be assigned to the private field m_number.
        //ctor1IL.Emit(OpCodes.Stfld, fbNumber);
        foreach (var field in fieldList)
        {
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, field);
            ctor1IL.Emit(OpCodes.Ret);
        }

        // Define a default constructor that supplies a default value
        // for the private field. For parameter types, pass the empty
        // array of types or pass null.
        ConstructorBuilder ctor0 = tb.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            Type.EmptyTypes);

        ILGenerator ctor0IL = ctor0.GetILGenerator();
        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before pushing the default
        // value on the stack, then call constructor ctor1.
        ctor0IL.Emit(OpCodes.Ldarg_0);
        ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);
        ctor0IL.Emit(OpCodes.Call, ctor1);
        ctor0IL.Emit(OpCodes.Ret);

        // Finish the type.
        Type t = tb.CreateType();                

        // Because AssemblyBuilderAccess includes Run, the code can be
        // executed immediately. Start by getting reflection objects for
        // the method and the property.
        foreach (var (columnName, type) in table.Types)
        {
            PropertyInfo pi = t.GetProperty(columnName);
        }

        // Create an instance of MyDynamicType using the default
        // constructor.
        object o1 = Activator.CreateInstance(t);

        var collection = new ObservableCollection<object>();

        // Create an instance of MyDynamicType using the constructor
        // that specifies m_Number. The constructor is identified by
        // matching the types in the argument array. In this case,
        // the argument array is created on the fly. Display the
        // property value.
        object o2 = Activator.CreateInstance(t,
            new object[] { 5280 });

        return collection;
    }
}