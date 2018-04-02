namespace Impulse.GridSystems.Hexagon
{
    using System;

    /// <summary>
    ///     Math operations for use in a hexagonal grid.
    /// </summary>
    /// <devdoc>
    ///     See http://www.redblobgames.com/grids/hexagons/
    /// </devdoc>
    public static class HexagonMath
    {
        /// <summary>
        ///     Determine if two points are aligned on any of the 3 hexagonal axes.
        /// </summary>
        public static bool AreAligned(HexagonPoint a, HexagonPoint b)
        {
            return AreAligned(a.ToCubePoint(), b.ToCubePoint());
        }

        /// <summary>
        ///     Determine if two points are aligned on any of the 3 hexagonal axes.
        /// </summary>
        public static bool AreAligned(CubePoint a, CubePoint b)
        {
            return a.X == b.X || a.Y == b.Y || a.Z == b.Z;
        }

        /// <summary>
        ///     Linearly interpolate between two points.
        /// </summary>
        public static HexagonPoint LerpRound(HexagonPoint a, HexagonPoint b, float t)
        {
            FloatCubePoint p = Lerp(a.ToCubePoint(), b.ToCubePoint(), t);
            return Round(p).ToHexagonPoint();
        }

        /// <summary>
        ///     Linearly interpolate between two points.
        /// </summary>
        public static FloatCubePoint Lerp(FloatCubePoint a, FloatCubePoint b, float t)
        {
            return new FloatCubePoint(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t);
        }

        /// <summary>
        ///     Get the number of hexagons between two points.
        /// </summary>
        public static int HexagonDistance(HexagonPoint a, HexagonPoint b)
        {
            return HexagonDistance(a.ToCubePoint(), b.ToCubePoint());
        }

        /// <summary>
        ///     Get the number of hexagons between two points.
        /// </summary>
        public static int HexagonDistance(CubePoint a, CubePoint b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2;
        }

        /// <summary>
        ///     Get the number of hexagons between (0, 0) and the specified point.
        /// </summary>
        public static int HexagonMagnitude(this HexagonPoint a)
        {
            return a.ToCubePoint().HexagonMagnitude();
        }

        /// <summary>
        ///     Get the number of hexagons between (0, 0) and the specified point.
        /// </summary>
        public static int HexagonMagnitude(this CubePoint a)
        {
            return (Math.Abs(a.X) + Math.Abs(a.Y) + Math.Abs(a.Z)) / 2;
        }

        /// <summary>
        ///     Round a FloatCubePoint to the nearest CubePoint.
        /// </summary>
        public static CubePoint Round(this FloatCubePoint h)
        {
            var xRounded = (short)Math.Round(h.X);
            var yRounded = (short)Math.Round(h.Y);
            var zRounded = (short)Math.Round(h.Z);

            float xDiff = Math.Abs(xRounded - h.X);
            float yDiff = Math.Abs(yRounded - h.Y);
            float zDiff = Math.Abs(zRounded - h.Z);

            if (xDiff > yDiff
                && xDiff > zDiff)
            {
                xRounded = (short)(-yRounded - zRounded);
            }
            else if (yDiff > zDiff)
            {
                yRounded = (short)(-xRounded - zRounded);
            }
            else
            {
                zRounded = (short)(-xRounded - yRounded);
            }

            return new CubePoint(xRounded, yRounded, zRounded);
        }

        /// <summary>
        ///     Convert a HexagonPoint to a CubePoint.
        /// </summary>
        public static CubePoint ToCubePoint(this HexagonPoint hexagonPoint)
        {
            short x = hexagonPoint.Q;
            short z = hexagonPoint.R;
            short y = (short)(-x - z);
            return new CubePoint(x, y, z);
        }

        /// <summary>
        ///     Convert a CubePoint to a HexagonPoint.
        /// </summary>
        public static HexagonPoint ToHexagonPoint(this CubePoint cubePoint)
        {
            short q = cubePoint.X;
            short r = cubePoint.Z;
            return new HexagonPoint(q, r);
        }
    }
}