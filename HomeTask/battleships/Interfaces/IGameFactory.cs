namespace battleships.Interfaces
{
    public interface IGameFactory
    {
        IGame CreateGame(Map map, IAi ai);
    }
}
