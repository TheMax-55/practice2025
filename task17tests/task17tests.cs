using System.Collections.Concurrent;
using System.Threading;
using task17;

public class ServerThreadTests
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action;
        public ActionCommand(Action action) => _action = action;
        public void Execute() => _action();
    }

    public bool IsThreadRunning(Thread? thread) => thread != null && thread.IsAlive;
    
    [Fact]
    public void ServerThread_HardStop_ShouldStopImmediately()
    {

        var serverThread = new ServerThread();
        bool commandExecuted = false;

        serverThread.Start();
        serverThread.AddToQueue(new HardStopCommand(serverThread));
        serverThread.AddToQueue(new ActionCommand(() => commandExecuted = true));

        Thread.Sleep(200);

        Assert.False(commandExecuted);
        Assert.False(IsThreadRunning(serverThread.Thread));
    }

    [Fact]
    public void ServerThread_SoftStop_ShouldExecuteAllCommandsBeforeStop()
    {
        var serverThread = new ServerThread();
        bool commandExecuted = false;

        serverThread.Start();
        serverThread.AddToQueue(new ActionCommand(() => commandExecuted = true));
        serverThread.AddToQueue(new SoftStopCommand(serverThread));

        Thread.Sleep(200);

        Assert.True(commandExecuted);
        Assert.False(IsThreadRunning(serverThread.Thread));
    }

    [Fact]
    public void ServerThread_HardStop_ShouldThrowException()
    {
        var serverThread = new ServerThread();
        serverThread.Start();

        Assert.Throws<InvalidOperationException>(() => { new HardStopCommand(serverThread).Execute(); });
    }

    [Fact]
    public void ServerThread_SoftStop_ShouldThrowException()
    {
        var serverThread = new ServerThread();
        serverThread.Start();

        Assert.Throws<InvalidOperationException>(() => { new SoftStopCommand(serverThread).Execute(); });
    }
}
