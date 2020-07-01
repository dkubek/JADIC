using System;
using System.Drawing;

public class Vector
{
    public double DX { get; private set; }
    public double DY { get; private set; }

    public Vector(double dx, double dy)
    {
        DX = dx;
        DY = dy;
    }

    public Vector(Point from, Point to)
    {
        DX = to.X - from.X;
        DY = to.Y - from.Y;
    }

    public double Size()
    {
        return Math.Sqrt(DX * DX + DY * DY);
    }

    public void Normalize()
    {
        var size = Size();
        DX /= size;
        DY /= size;
    }

    public void Multiply(double alpha)
    {
        DX *= alpha;
        DY *= alpha;
    }

    public static Point Add(Point start, Vector displacement)
    {
        int newX = (int)(start.X + displacement.DX);
        int newY = (int)(start.Y + displacement.DY);

        return new Point(newX, newY);
    }

    public static Vector FromPolar(double angle, double size)
    {
        var vec = new Vector(Math.Sin(angle), Math.Cos(angle));
        vec.Multiply(size);
        return vec;
    }
}
