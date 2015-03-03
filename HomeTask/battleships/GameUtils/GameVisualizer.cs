using System;
using System.Linq;
using battleships.Enums;
using battleships.MapUtils;
using MoreLinq;

namespace battleships.GameUtils
{
    public class GameVisualizer
    {
        public void VisualizeCompletedGameStep(Game game)
        {
            Console.Clear();
            Console.WriteLine(MapToString(game));
            Console.WriteLine("Turn: {0}", game.TurnsCount);
            Console.WriteLine("Last target: {0}", game.LastTarget);
            if (game.BadShots > 0)
            {
                Console.WriteLine("Bad shots: " + game.BadShots);
            }
            if (game.IsOver())
            {
                Console.WriteLine("Game is over");
            }

            if (game.AiCrashed)
            {
                Console.WriteLine(game.LastError.Message);
            }

            Console.ReadKey();
        }

        public void VisualizeCompletedGame(Game game)
        {
            Console.WriteLine(
                        "Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
                        game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", game.Index);
        }

        private static string MapToString(Game game)
        {
            var map = game.Map;

            return Enumerable.Range(0, map.Height)
                                .Cartesian(Enumerable.Range(0, map.Width), (x, y) => GetSymbol(map[new Vector(x, y)]))
                                .Batch(map.Width)
                                .Select(x => x.ToDelimitedString(""))
                                .ToDelimitedString("\n");
        }

        private static string GetSymbol(MapCell cell)
        {
            switch (cell)
            {
                case MapCell.Empty:
                    return " ";
                case MapCell.Miss:
                    return "*";
                case MapCell.Ship:
                    return "O";
                case MapCell.DeadOrWoundedShip:
                    return "X";
                default:
                    throw new Exception(cell.ToString());
            }
        }
    }
}