using System.Collections.Generic;
using System.Linq;
using battleships.Enums;

namespace battleships
{
    public class Ship
    {
        public int Size { get; private set; }
        public ShipDirection Direction { get; private set; }
        public bool IsAlive { get { return aliveCells.Any(); } }
        public Vector Location { get; private set; }
        private readonly HashSet<Vector> aliveCells;

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

            for (var i = 0; i < Size; ++i)
            {
                var shipCell = startShipCell.Mult(i).Add(Location);
                yield return shipCell;
            }
        }

        public IEnumerable<Vector> GetAliveCells()
        {
            return aliveCells;
        } 

        public void KillCell(Vector cell)
        {
            aliveCells.Remove(cell);
        }
    }
}
