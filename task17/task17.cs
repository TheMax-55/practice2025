using System.Collections.Concurrent;
using System.Threading;
namespace task17;

public interface ICommand
{
    void Execute();
}

public class ServerThread
{
    private ConcurrentQueue<ICommand> commands = new ConcurrentQueue<ICommand>();
    private Thread? thread;
    private bool hardStop;
    private bool softStop;
    private bool isRunning;
    public Thread? Thread => thread;


    public void Start()
    {
        isRunning = true;
        thread = new Thread(Run);
        thread.Start();
    }

    public void AddToQueue(ICommand command)
    {
        if (!isRunning || hardStop || softStop)
            throw new InvalidOperationException("Сервер остановлен.");
        commands.Enqueue(command);
    }

    public void HardStop() => hardStop = true;
    public void SoftStop() => softStop = true;

    private void Run()
    {
        while (!hardStop)
        {
            if (commands.TryDequeue(out var command))
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex, command);
                }
            else
            {
                if (softStop && commands.IsEmpty)
                {
                    isRunning = false;
                    break;
                }
            }
        }
        isRunning = false;
    }
}

public class HardStopCommand : ICommand
{
    private readonly ServerThread _serverThread;

    public HardStopCommand(ServerThread serverThread)
        => _serverThread = serverThread;

    public void Execute()
    {
        if (Thread.CurrentThread == _serverThread.Thread)
            _serverThread.HardStop();
        else
            throw new InvalidOperationException("HardStop не может выполниться в данном потоке.");
    }
}

public class SoftStopCommand : ICommand
{
    private readonly ServerThread _serverThread;

    public SoftStopCommand(ServerThread serverThread)
        => _serverThread = serverThread;

    public void Execute()
    {
        if (Thread.CurrentThread == _serverThread.Thread)
            _serverThread.SoftStop();
        else
            throw new InvalidOperationException("SoftStop не может выполниться в данном потоке.");
    }
}

public class ExceptionHandler
{
    public static void Handle(Exception ex, ICommand command)
        => Console.WriteLine($"Ошибка при выполнении команды {command.GetType().Name}: {ex.Message}");
}
