using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace battleships.MapUtils
{
    public class Vector
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }

        public Vector Mult(int k)
        {
            return new Vector(k * X, k * Y);
        }

        public Vector Add(Vector v)
        {
            return new Vector(v.X + X, v.Y + Y);
        }

        public Vector Sub(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y);
        }

        public static IEnumerable<Vector> Rect(int minX, int minY, int width, int height)
        {
            return Enumerable.Range(minX, width)
                                .Cartesian(Enumerable.Range(minY, height), (x, y) => new Vector(x, y));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Y * 397) ^ X;
            }
        }

        private bool Equals(Vector other)
        {
            return Y == other.Y && X == other.X;
        }
    }
}