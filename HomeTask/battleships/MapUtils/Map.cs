using System;
using System.Collections.Generic;
using System.Linq;
using battleships.Enums;

namespace battleships.MapUtils
{
    public class Map
    {
        private static MapCell[,] cells;
        public Ship[,] Ships { get; private set; }
        public List<Ship> AllShips { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MapCell this[Vector cell]
        {
            get
            {
                return IsCorrectCell(cell) ? cells[cell.X, cell.Y] : MapCell.Empty;
            }

            private set
            {
                if (!IsCorrectCell(cell))
                    throw new IndexOutOfRangeException(cell + " is not in the map borders");
                
                cells[cell.X, cell.Y] = value;
            }
        }

        public Map(int width, int height)
        {
            if (width < 0)
                throw new ArgumentException("Width should be > 0.");
            if (height < 0)
                throw new ArgumentException("Height should be > 0.");

            Width = width;
            Height = height;

            cells = new MapCell[width, height];
            Ships = new Ship[width, height];
            AllShips = new List<Ship>();
        }

        public bool TrySetShip(Vector shipStartCell, int shipSize, ShipDirection shipDirection)
        {
            var ship = new Ship(shipStartCell, shipSize, shipDirection);
            var shipCells = ship.GetShipCells().ToList();

            if (!IsPossibleSetShip(shipCells)) return false;

            foreach (var cell in shipCells)
            {
                this[cell] = MapCell.Ship;
                Ships[cell.X, cell.Y] = ship;
            }

            AllShips.Add(ship);
            return true;
        }

        private bool IsPossibleSetShip(List<Vector> shipCells)
        {
            return shipCells.All(AllNeighboursEmpty) && shipCells.All(IsCorrectCell);
        }

        private bool AllNeighboursEmpty(Vector cell)
        {
            return GetNearbyCells(cell).All(IsEmpty);
        }

        private bool IsEmpty(Vector cell)
        {
            return this[cell] == MapCell.Empty;
        }

        public ShotEffect DoShot(Vector target)
        {
            if (IsCorrectCell(target) && this[target] == MapCell.Ship)
            {
                return ShootIntoShip(target);
            }

            return ShootIntoEmptyCell(target);
        }

        private ShotEffect ShootIntoEmptyCell(Vector target)
        {
            this[target] = MapCell.Miss;
            return ShotEffect.Miss;
        }

        private ShotEffect ShootIntoShip(Vector target)
        {
            var woundedShip = Ships[target.X, target.Y];
            woundedShip.KillCell(target);
            this[target] = MapCell.DeadOrWoundedShip;

            return woundedShip.IsAlive ? ShotEffect.Wound : ShotEffect.Kill;
        }

        public IEnumerable<Vector> GetNearbyCells(Vector cell)
        {
            return
                from x in new[] { -1, 0, 1 }
                from y in new[] { -1, 0, 1 }
                let currentCell = cell.Add(new Vector(x, y))
                where IsCorrectCell(currentCell)
                select currentCell;
        }

        private bool IsCorrectCell(Vector cell)
        {
            return cell.X >= 0 && cell.X < Width && cell.Y >= 0 && cell.Y < Height;
        }

        public bool HasAliveShips()
        {
            return AllShips.Any(s => s.IsAlive);
        }
    }
}