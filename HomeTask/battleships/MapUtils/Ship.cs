using System.Collections.Generic;
using System.Linq;
using battleships.Enums;

namespace battleships.MapUtils
{
    public class Ship
    {
        public int Size { get; private set; }
        public ShipDirection Direction { get; private set; }
        public bool IsAlive { get { return aliveCells.Any(); } }
        public Vector Location { get; private set; }
        public HashSet<Vector> aliveCells { get; private set; }

        public Ship(Vector location, int size, ShipDirection direction)
        {
            Location = location;
            Size = size;
            Direction = direction;
            aliveCells = new HashSet<Vector>(GetShipCells());
        }

        public IEnumerable<Vector> GetShipCells()
        {
            var startShipCell = Direction == ShipDirection.Horizontal ? new Vector(1, 0) : new Vector(0, 1);

            return Enumerable.Range(0, Size).Select(i => startShipCell.Mult(i).Add(Location));
        }

        public void KillCell(Vector cell)
        {
            aliveCells.Remove(cell);
        }
    }
}
