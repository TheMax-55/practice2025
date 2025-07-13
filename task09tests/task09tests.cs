using System;
using Xunit;
using task09;

public class MetadataViewerTests
{
    [Fact]
    public void MetadataViewer_NoArguments_ShouldPrintErrorMessage()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        MetadataViewer.PrintInfo(String.Empty);
        Assert.Contains("Укажите путь к .dll", output.ToString());
    }

    [Fact]
    public void MetadataViewer_ShouldPrintCorrectClass()
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "task07.dll");

        var output = new StringWriter();
        Console.SetOut(output);

        MetadataViewer.PrintInfo(path);
        Assert.Contains("Класс: task07.SampleClass", output.ToString());
    }

    [Fact]
    public void MetadataViewer_ShouldPrintCorrectMethods()
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "task07.dll");

        var output = new StringWriter();
        Console.SetOut(output);

        MetadataViewer.PrintInfo(path);
        Assert.Contains("Методы:", output.ToString());
        Assert.Contains("TestMethod", output.ToString());
        Assert.Contains("get_Number", output.ToString());
        Assert.Contains("set_Number", output.ToString());
        Assert.Contains("Параметр: value (Int32)", output.ToString());
    }

    [Fact]
    public void MetadataViewer_ShouldPrintCorrectAttributes()
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "task07.dll");

        var output = new StringWriter();
        Console.SetOut(output);

        MetadataViewer.PrintInfo(path);
        Assert.Contains("DisplayNameAttribute", output.ToString());
        Assert.Contains("VersionAttribute", output.ToString());
    }

    [Fact]
    public void MetadataViewer_ShouldPrintCorrectConstructors()
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "task07.dll");

        var output = new StringWriter();
        Console.SetOut(output);

        MetadataViewer.PrintInfo(path);
        Assert.Contains("Конструкторы:", output.ToString());
        Assert.Contains(".ctor", output.ToString());
    }
}
