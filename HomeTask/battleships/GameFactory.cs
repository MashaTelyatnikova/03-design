using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using battleships.Interfaces;

namespace battleships
{
    class GameFactory : IGameFactory
    {
        public IGame CreateGame(Map map, IAi ai)
        {
            return new Game(map, ai);
        }
    }
}
