/// <summary>
/// Utilities class for Point struct.
/// </summary>
public class PointUtil
{
    /// <summary>
    /// Takes a given array of Points and adds a given offset to them.
    /// This is useful if you have a set of points, like a movement path,
    /// but need to apply it to a unit in a different Point position.
    /// </summary>
    /// <param name="input">An array of points.</param>
    /// <param name="offset">The amount to offset the array of points.</param>
    /// <returns></returns>
    public static Point[] Offset(Point[] input, Point offset)
    {
        Point[] retval = new Point[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            Point pos = input[i];
            retval[i] = new Point(offset.X + pos.X, offset.Y + pos.Y);
        }

        return retval;
    }

    /// <summary>
    /// Rotates each point in an array by a 90 degree increment then adds translates the points by the given offset.
    /// </summary>
    /// <param name="input">An array of points.</param>
    /// <param name="rotation">The amount to rotate the array of points. Must be 0, 90, 180, or 270.</param>
    /// <param name="offset">The amount to translate the rotated array.</param>
    /// <returns></returns>
    public static Point[] Rotate(Point[] input, int rotation, Point offset)
    {
        Point xRot = xRot0;
        Point yRot = yRot0;

        switch (rotation)
        {
            case 0:
                // default
                break;
            case 90:
            case -270:
                xRot = xRot90;
                yRot = yRot90;
                break;
            case 180:
                xRot = xRot180;
                yRot = yRot180;
                break;
            case 270:
            case -90:
                xRot = xRot270;
                yRot = yRot270;
                break;
            default:
                throw new System.Exception("Rotate requires a rotation input that is 0, 90, 180, or 270.");
        }

        Point[] retval = new Point[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            Point pos = input[i];
            int x = pos.X * xRot.X + pos.Y * xRot.Y;
            int y = pos.X * yRot.X + pos.Y * yRot.Y;
            retval[i] = new Point(x + offset.X, y + offset.Y);
        }

        return retval;
    }

    public static Point xRot0 = new Point(1, 0);
    public static Point yRot0 = new Point(0, 1);

    public static Point xRot90 = new Point(0, -1);
    public static Point yRot90 = new Point(1, 0);

    public static Point xRot180 = new Point(-1, 0);
    public static Point yRot180 = new Point(0, -1);

    public static Point xRot270 = new Point(0, 1);
    public static Point yRot270 = new Point(-1, 0);
}
