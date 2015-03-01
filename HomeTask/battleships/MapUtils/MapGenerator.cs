using System;
using System.Collections.Generic;
using System.Linq;
using battleships.Enums;

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

        public IEnumerable<Map> GenerateMaps()
        {
            while (true)
            {
                yield return GenerateMap();
            }
        }

        private Map GenerateMap()
        {
            var map = new Map(width, height);
            foreach (var size in shipSizes)
                PlaceShip(map, size);
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
                    return;
            }

            throw new Exception("Can't put next ship on map. No free space");
        }
    }
}