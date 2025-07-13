using System;
using System.Reflection;

namespace task07
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        public DisplayNameAttribute(string displayName) => DisplayName = displayName;

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }
        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }

    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Тестовый метод")]
        public void TestMethod() { }

        [DisplayName("Числовое свойство")]
        public int Number { get; set; }
    }

    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayNameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
                Console.WriteLine($"Отображаемое имя класса: {displayNameAttribute.DisplayName}");

            var versionAttribute = type.GetCustomAttribute<VersionAttribute>();
            if (versionAttribute != null)
                Console.WriteLine($"Версия класса: {versionAttribute.Major}.{versionAttribute.Minor}");

            Console.WriteLine("Список методов:");
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var methodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
                if (methodDisplayName != null)
                    Console.WriteLine($"{method.Name}: {methodDisplayName.DisplayName}");
            }

            Console.WriteLine("Список свойств:");
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyDisplayName = property.GetCustomAttribute<DisplayNameAttribute>();
                if (propertyDisplayName != null)
                    Console.WriteLine($"{property.Name}: {propertyDisplayName.DisplayName}");
            }            
        }
    }
}
