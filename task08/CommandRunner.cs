using CommandLib;
using FileSystemCommands;
using System.Reflection;
namespace CommandRunner;

public class Program
{
    public static void Main()
    {
        var assembly = Assembly.LoadFrom("FileSystemCommands.dll");

        var testDir1 = Path.Combine(Path.GetTempPath(), "TestDir1");
        Directory.CreateDirectory(testDir1);
        File.WriteAllText(Path.Combine(testDir1, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir1, "test2.txt"), "World");

        Type? sizeCommandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");

        if (sizeCommandType != null && Activator.CreateInstance(sizeCommandType, testDir1) is DirectorySizeCommand sizeCommand)
        {
            sizeCommand.Execute();
            Console.WriteLine($"Размер каталога равен {sizeCommand.size}");
        }

        var testDir2 = Path.Combine(Path.GetTempPath(), "TestDir2");
        Directory.CreateDirectory(testDir2);
        File.WriteAllText(Path.Combine(testDir2, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir2, "file2.log"), "Log");

        Type? findFilesType = assembly.GetType("FileSystemCommands.FindFilesCommand");

        if (findFilesType != null && Activator.CreateInstance(findFilesType, testDir2, "*.txt") is FindFilesCommand filesCommand)
        {
            filesCommand.Execute();
            foreach (var file in filesCommand.files)
                Console.WriteLine(file);
        }

        Directory.Delete(testDir1, true);
        Directory.Delete(testDir2, true);
    }
}
