using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
