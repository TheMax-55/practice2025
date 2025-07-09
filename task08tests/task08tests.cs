using Xunit;
using FileSystemCommands;
using CommandRunner;
public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir1");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Assert.Equal(10, command.size);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir2");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        Assert.Single(command.files);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void CommandRunner_ShouldPrintCorrectInformation()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        Program.Main();
        
        Assert.Contains("Размер каталога равен 10", output.ToString());
        Assert.Contains("file1.txt", output.ToString());
    }
}
