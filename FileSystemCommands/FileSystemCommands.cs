using CommandLib;
namespace FileSystemCommands;
public class DirectorySizeCommand : ICommand
{
    private readonly string _path;
    public long size { get; private set; }

    public DirectorySizeCommand(string path)
        => _path = path;

    public void Execute()
    {
        DirectoryInfo directory = new DirectoryInfo(_path);
        if (directory.Exists)
            size = directory.GetFiles()
            .Sum(f => f.Length);
    }
}

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _mask;
    public List<string> files { get; private set; } = new List<string>();

    public FindFilesCommand(string path, string mask)
        => (_path, _mask) = (path, mask);

    public void Execute()
    {
        DirectoryInfo directory = new DirectoryInfo(_path);
        if (directory.Exists)
            files = directory.GetFiles(_mask)
            .Select(f => f.Name)
            .ToList();
    }
}
