namespace Impulse.GridSystems.Hexagon
{
	/// <summary>
	///     A cube coordinate with floating point resolution, 
	///     as defined by http://www.redblobgames.com/grids/hexagons/.
	/// </summary>
	public class FloatCubePoint
	{
		public FloatCubePoint(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public float X { get; private set; }

		public float Y { get; private set; }

		public float Z { get; private set; }

		protected bool Equals(FloatCubePoint other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) { return false; }
			if (ReferenceEquals(this, obj)) { return true; }
			if (obj.GetType() != this.GetType()) { return false; }
			return Equals((FloatCubePoint)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.X.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(FloatCubePoint left, FloatCubePoint right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(FloatCubePoint left, FloatCubePoint right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("({0},{1},{2})", this.X, this.Y, this.Z);
		}
	}
}
