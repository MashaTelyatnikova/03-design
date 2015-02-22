using battleships.Enums;

namespace battleships.Interfaces
{
    public interface IAi
    {
        string Name { get; }
        Vector Init(int width, int height, int[] shipSizes);
        Vector GetNextShot(Vector lastShotTarget, ShotEffect lastShot);
        void Dispose();
    }
}
