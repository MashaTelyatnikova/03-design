namespace battleships.Interfaces
{
    public interface IAiFactory
    {
        IAi CreateAi(string path, ProcessMonitor monitor);
    }
}
