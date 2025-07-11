using System;
using System.Reflection;
using task07;
namespace task09;

public class MetadataViewer
{
    public static void PrintInfo(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Укажите путь к .dll");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(path);
        
        foreach (Type type in assembly.GetTypes())
        {
            Console.WriteLine($"Класс: {type.FullName}");

            Console.WriteLine("Методы:");
            foreach (var method in type.GetMethods())
            {
                Console.WriteLine($"{method.Name}");
                foreach (var parameter in method.GetParameters())
                    Console.WriteLine($"Параметр: {parameter.Name} ({parameter.ParameterType.Name})");
            }

            Console.WriteLine("Атрибуты:");
            foreach (var attribute in type.GetCustomAttributes())
                Console.WriteLine($"{attribute.GetType().Name}");

            Console.WriteLine("Конструкторы:");
            foreach (var constructor in type.GetConstructors())
            {
                Console.WriteLine($"{constructor.Name}");
                foreach (var parameter in constructor.GetParameters())
                    Console.WriteLine($"Параметр: {parameter.Name} ({parameter.ParameterType.Name})");
            }
            Console.WriteLine();
        }
    }
}
