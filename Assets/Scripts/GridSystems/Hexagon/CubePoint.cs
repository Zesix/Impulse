namespace Impulse.GridSystems.Hexagon
{
	/// <summary>
	///     A cube coordinate for use in a hexagonal grid,
	///     as defined by http://www.redblobgames.com/grids/hexagons/.
	/// </summary>
	public class CubePoint
	{
		public CubePoint(short x, short y, short z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public static implicit operator FloatCubePoint(CubePoint a)
		{
			return new FloatCubePoint(a.X, a.Y, a.Z);
		}

		public short X { get; private set; }

		public short Y { get; private set; }

		public short Z { get; private set; }

		protected bool Equals(CubePoint other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) { return false; }
			if (ReferenceEquals(this, obj)) { return true; }
			if (obj.GetType() != this.GetType()) { return false; }
			return Equals((CubePoint)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.X;
				hashCode = (hashCode * 397) ^ this.Y;
				hashCode = (hashCode * 397) ^ this.Z;
				return hashCode;
			}
		}

		public static bool operator ==(CubePoint left, CubePoint right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(CubePoint left, CubePoint right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("({0},{1},{2})", this.X, this.Y, this.Z);
		}
	}
}
