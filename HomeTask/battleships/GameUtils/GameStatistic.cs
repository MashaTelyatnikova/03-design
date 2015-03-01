namespace battleships.GameUtils
{
    public class GameStatistic
    {
        public int TurnsCount { get; private set; }
        public int BadShots { get; private set; }
        public bool IsCrashed { get; private set; }

        public GameStatistic(int turnsCount, int badShots, bool isCrashed)
        {
            TurnsCount = turnsCount;
            BadShots = badShots;
            IsCrashed = isCrashed;
        }
    }
}
