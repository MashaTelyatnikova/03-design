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
