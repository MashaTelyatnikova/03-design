using battleships.Enums;
using battleships.MapUtils;
using NUnit.Framework;

namespace BattleshipsTests
{
    [TestFixture]
    public class Map_should
    {
        [Test]
        public void put_ship_inside_map_bounds()
        {
            var map = new Map(100, 10);
            Assert.IsTrue(map.TrySetShip(new Vector(0, 0), 5, ShipDirection.Horizontal));
            Assert.IsTrue(map.TrySetShip(new Vector(95, 9), 5, ShipDirection.Horizontal));
        }

        [Test]
        public void not_put_ship_outside_map()
        {
            var map = new Map(100, 10);
            Assert.IsFalse(map.TrySetShip(new Vector(99, 9), 2, ShipDirection.Horizontal));
            Assert.IsFalse(map.TrySetShip(new Vector(99, 9), 2, ShipDirection.Vertical));
        }

        [Test]
        public void kill_ship()
        {
            var map = new Map(100, 10);
            map.TrySetShip(new Vector(0, 0), 1, ShipDirection.Horizontal);
            Assert.AreEqual(ShotEffect.Kill, map.DoShot(new Vector(0, 0)));
            Assert.AreEqual(MapCell.DeadOrWoundedShip, map[new Vector(0, 0)]);
        }

        [Test]
        public void wound_ship()
        {
            var map = new Map(100, 10);
            map.TrySetShip(new Vector(0, 0), 2, ShipDirection.Horizontal);
            Assert.AreEqual(ShotEffect.Wound, map.DoShot(new Vector(0, 0)));
            Assert.AreEqual(MapCell.DeadOrWoundedShip, map[new Vector(0, 0)]);
            Assert.AreEqual(MapCell.Ship, map[new Vector(1, 0)]);
        }
    }
}