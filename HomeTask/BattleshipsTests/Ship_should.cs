using System.Collections.Generic;
using battleships.Enums;
using NUnit.Framework;

namespace battleships
{
    [TestFixture]
    public class Ship_should
    {
        [Test]
        public void correctly_fill_fields_for_horizontal_direction()
        {
            var ship = new Ship(new Vector(0, 0), 3, ShipDirection.Horizontal);
            var cells = new HashSet<Vector>() { new Vector(0, 0), new Vector(1, 0), new Vector(2, 0) };

            Assert.That(ship.Size, Is.EqualTo(3));
            Assert.That(ship.Direction, Is.EqualTo(ShipDirection.Horizontal));
            Assert.That(ship.Location, Is.EqualTo(new Vector(0, 0)));
            Assert.That(ship.IsAlive, Is.True);
            Assert.That(ship.aliveCells, Is.EqualTo(cells));
        }

        [Test]
        public void correctly_fill_fields_for_vertical_direction()
        {
            var ship = new Ship(new Vector(0, 0), 3, ShipDirection.Vertical);
            var cells = new HashSet<Vector>() { new Vector(0, 0), new Vector(0, 1), new Vector(0, 2) };

            Assert.That(ship.Size, Is.EqualTo(3));
            Assert.That(ship.Direction, Is.EqualTo(ShipDirection.Vertical));
            Assert.That(ship.Location, Is.EqualTo(new Vector(0, 0)));
            Assert.That(ship.IsAlive, Is.True);
            Assert.That(ship.aliveCells, Is.EqualTo(cells));
        }

        [Test]
        public void returns_correct_cells()
        {
            var ship = new Ship(new Vector(0, 0), 3, ShipDirection.Horizontal);
            var expectedCells = new List<Vector>() { new Vector(0, 0), new Vector(1, 0), new Vector(2, 0) };

            Assert.That(ship.GetShipCells(), Is.EqualTo(expectedCells));
        }
    }
}
