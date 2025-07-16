using Xunit;
using task10;

[PluginLoad]
public class Plugin: IPlugin
{
    public void Execute() => Console.WriteLine("Plugin загружен.");
}

[PluginLoad("Plugin")]
public class PluginWithDependency: IPlugin
{
    public void Execute() => Console.WriteLine("PluginWithDependency загружен.");
}

[PluginLoad("SecondCyclicPlugin")]
public class FirstCyclicPlugin : IPlugin
{
    public void Execute() => Console.WriteLine("FirstCyclicPlugin загружен.");
}

[PluginLoad("FirstCyclicPlugin")]
public class SecondCyclicPlugin : IPlugin
{
    public void Execute() => Console.WriteLine("FirstCyclicPlugin загружен.");
}

public class PluginLoaderTest
{
    [Fact]
    public void Load_PluginWithoutDependency_ShouldExecute()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);

        File.Copy(typeof(Plugin).Assembly.Location, Path.Combine(testDir, "Plugin.dll"));

        var output = new StringWriter();
        Console.SetOut(output);

        PluginLoader.LoadPlugins(testDir);

        Assert.Contains("Plugin загружен.", output.ToString());
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void Load_PluginWithDependency_ShouldExecute()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);

        File.Copy(typeof(Plugin).Assembly.Location, Path.Combine(testDir, "Plugin.dll"));
        File.Copy(typeof(PluginWithDependency).Assembly.Location, Path.Combine(testDir, "PluginWithDependency.dll"));

        var output = new StringWriter();
        Console.SetOut(output);

        PluginLoader.LoadPlugins(testDir);

        Assert.Contains("Plugin загружен.", output.ToString());
        Assert.Contains("PluginWithDependency загружен.", output.ToString());
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void Load_CircularDependency_ShouldThrowException()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);

        File.Copy(typeof(FirstCyclicPlugin).Assembly.Location, Path.Combine(testDir, "FirstCyclicPlugin.dll"));
        File.Copy(typeof(SecondCyclicPlugin).Assembly.Location, Path.Combine(testDir, "SecondCyclicPlugin.dll"));

        var output = new StringWriter();
        Console.SetOut(output);

        PluginLoader.LoadPlugins(testDir);

        Assert.Contains("Невозможно загрузить плагины.", output.ToString());
        Directory.Delete(testDir, true);
    }
}
