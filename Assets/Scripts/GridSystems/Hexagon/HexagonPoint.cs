namespace Impulse.GridSystems.Hexagon
{
    /// <summary>
    ///     An axial coordinate for use in a hexagonal grid,
    ///     as defined by http://www.redblobgames.com/grids/hexagons/.
    /// </summary>
    public class HexagonPoint
    {
        protected bool Equals(HexagonPoint other)
        {
            return this.R == other.R && this.Q == other.Q;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != this.GetType()) { return false; }
            return this.Equals((HexagonPoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (this.R * 397) ^ this.Q; }
        }

        public static bool operator ==(HexagonPoint left, HexagonPoint right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HexagonPoint left, HexagonPoint right)
        {
            return !Equals(left, right);
        }

        public static HexagonPoint operator +(HexagonPoint left, HexagonPoint right)
        {
            return new HexagonPoint((short)(left.Q + right.Q), (short)(left.R + right.R));
        }

        public static HexagonPoint operator -(HexagonPoint left, HexagonPoint right)
        {
            return new HexagonPoint((short)(left.Q - right.Q), (short)(left.R - right.R));
        }

        public static HexagonPoint operator *(HexagonPoint left, int right)
        {
            return new HexagonPoint((short)(left.Q * right), (short)(left.R * right));
        }

        public static HexagonPoint operator /(HexagonPoint left, int right)
        {
            return new HexagonPoint((short)(left.Q / right), (short)(left.R / right));
        }

        public static HexagonPoint operator -(HexagonPoint left)
        {
            return new HexagonPoint((short)(-left.Q), (short)(-left.R));
        }

        public static HexagonPoint operator *(int left, HexagonPoint right)
        {
            return right * left;
        }

        public short Q { get; private set; }

        public short R { get; private set; }

        public static HexagonPoint Zero
        {
            get
            {
                return new HexagonPoint(0, 0);
            }
        }

        public static HexagonPoint Left
        {
            get
            {
                return new HexagonPoint(-1, 0);
            }
        }

        public static HexagonPoint Right
        {
            get
            {
                return new HexagonPoint(1, 0);
            }
        }

        public static HexagonPoint DownLeft
        {
            get
            {
                return new HexagonPoint(0, -1);
            }
        }

        public static HexagonPoint UpRight
        {
            get
            {
                return new HexagonPoint(0, 1);
            }
        }

        public static HexagonPoint UpLeft
        {
            get
            {
                return new HexagonPoint(-1, 1);
            }
        }

        public static HexagonPoint DownRight
        {
            get
            {
                return new HexagonPoint(1, -1);
            }
        }

        public static HexagonPoint[] Directions
        {
            get
            {
                return new[]
                    {
                        HexagonPoint.Right,
                        HexagonPoint.DownRight,
                        HexagonPoint.DownLeft,
                        HexagonPoint.Left,
                        HexagonPoint.UpLeft,
                        HexagonPoint.UpRight
                    };
            }
        }

        public HexagonPoint(short q, short r)
        {
            this.Q = q;
            this.R = r;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", this.Q, this.R);
        }
    }
}
