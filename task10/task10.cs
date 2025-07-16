﻿using System.Reflection;
namespace task10;

public interface IPlugin
{
    public void Execute();
}

[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string[] Dependencies { get; } = Array.Empty<string>();

    public PluginLoadAttribute(params string[] dependencies)
    {
        Dependencies = dependencies;
    }
}

public class PluginLoader
{
    public static void LoadPlugins(string path)
    {
        var plugins = new List<(Type Type, string Name, string[] Dependencies)>();

        foreach (var dllPath in Directory.GetFiles(path, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(dllPath);
            foreach (var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
                if (attribute != null && typeof(IPlugin).IsAssignableFrom(type))
                    plugins.Add((type, type.Name, attribute.Dependencies));
            }
        }

        var loaded = new HashSet<string>();
        var unloaded = new List<(Type Type, string Name, string[] Dependencies)>(plugins);

        while (unloaded.Count > 0)
        {
            var ready = unloaded
                .Where(p => p.Dependencies.All(d => loaded.Contains(d)))
                .ToList();

            if (!ready.Any())
                break;

            foreach (var plugin in ready)
            {
                var instance = (IPlugin)Activator.CreateInstance(plugin.Type)!;
                instance.Execute();
                loaded.Add(plugin.Name);
                unloaded.Remove(plugin);
            }
        }

        if (unloaded.Count > 0)
        {
            Console.WriteLine("Невозможно загрузить плагины.");
            return;
        }
    }
}
