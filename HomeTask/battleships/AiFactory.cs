using battleships.Interfaces;

namespace battleships
{
    class AiFactory : IAiFactory
    {
        public IAi CreateAi(string path, ProcessMonitor monitor)
        {
            return new Ai(path, monitor);
        }
    }
}
