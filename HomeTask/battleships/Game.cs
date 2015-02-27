using System;
using System.Linq;
using battleships.Enums;
using NLog;

namespace battleships
{
    public class Game
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly Ai ai;

        public Game(Map map, Ai ai)
        {
            Map = map;
            this.ai = ai;
            TurnsCount = 0;
            BadShots = 0;
        }

        public Vector LastTarget { get; private set; }
        public int TurnsCount { get; private set; }
        public int BadShots { get; private set; }
        public Map Map { get; private set; }
        public ShotInfo LastShotInfo { get; private set; }
        public bool AiCrashed { get; private set; }
        public Exception LastError { get; private set; }

        public bool IsOver()
        {
            return !Map.HasAliveShips() || AiCrashed;
        }

        public void MakeStep()
        {
            if (IsOver()) throw new InvalidOperationException("Game is Over");
            if (!UpdateLastTarget()) return;
            if (IsBadShot(LastTarget)) BadShots++;
            var hit = Map.DoShot(LastTarget);
            LastShotInfo = new ShotInfo { Target = LastTarget, Hit = hit };
            if (hit == ShotEffect.Miss)
                TurnsCount++;

        }

        private bool UpdateLastTarget()
        {
            try
            {
                LastTarget = LastTarget == null
                    ? ai.Init(Map.Width, Map.Height, Map.AllShips.Select(s => s.Size).ToArray())
                    : ai.GetNextShot(LastShotInfo.Target, LastShotInfo.Hit);
                return true;
            }
            catch (Exception e)
            {
                AiCrashed = true;
                Log.Info("Ai {0} crashed", ai.Name);
                Log.Error(e);
                LastError = e;
                return false;
            }
        }

        private bool IsBadShot(Vector target)
        {
            var cellWasHitAlready = Map[target] != MapCell.Empty && Map[target] != MapCell.Ship;
            var cellIsNearDestroyedShip = Map.GetNearbyCells(target).Any(c => Map.Ships[c.X, c.Y] != null && !Map.Ships[c.X, c.Y].IsAlive);
            var diagonals = new[] { new Vector(-1, -1), new Vector(-1, 1), new Vector(1, -1), new Vector(1, 1) };
            var cellHaveWoundedDiagonalNeighbour = diagonals.Any(d => Map[target.Add(d)] == MapCell.DeadOrWoundedShip);
            return cellWasHitAlready || cellIsNearDestroyedShip || cellHaveWoundedDiagonalNeighbour;
        }
    }
}