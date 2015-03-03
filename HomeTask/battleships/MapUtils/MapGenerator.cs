using System;
using System.Linq;
using battleships.Enums;
using MoreLinq;

namespace battleships.MapUtils
{
    public class MapGenerator
    {
        private readonly int height;
        private readonly Random random;
        private readonly int[] shipSizes;
        private readonly int width;

        public MapGenerator(Settings settings)
        {
            width = settings.Width;
            height = settings.Height;
            shipSizes = settings.Ships.OrderByDescending(s => s).ToArray();
            random = new Random(settings.RandomSeed);
        }

        public Map GenerateMap()
        {
            var map = new Map(width, height);
            shipSizes.ForEach(size => PlaceShip(map, size));

            return map;
        }
        private void PlaceShip(Map map, int size)
        {
            var cells = Vector.Rect(0, 0, width, height).OrderBy(v => random.Next());

            foreach (var loc in cells)
            {
                var direction = random.Next(2) == 0 ? ShipDirection.Horizontal : ShipDirection.Vertical;
                var invertDirection = direction == ShipDirection.Horizontal ? ShipDirection.Vertical : ShipDirection.Horizontal;

                if (map.TrySetShip(loc, size, direction) || map.TrySetShip(loc, size, invertDirection))
                {
                    return;
                }
            }

            throw new Exception("Can't put next ship on map. No free space");
        }
    }
}