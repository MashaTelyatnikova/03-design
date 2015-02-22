using System;
using System.Linq;

namespace battleships
{
    public class MapGenerator : IMapGenerator
    {
        private readonly int height;
        private readonly Random random;
        private readonly int[] shipSizes;
        private readonly int width;

        public MapGenerator(int width, int height, int[] shipSizes, Random random) 
        {
            this.width = width;
            this.height = height;
            this.shipSizes = shipSizes.OrderByDescending(s => s).ToArray();
            this.random = random;
        }

        public Map GenerateMap()
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