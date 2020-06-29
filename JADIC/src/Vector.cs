using System;
using System.Drawing;

public class Vector
{
    private double dx;
    private double dy;

    public double DX
    {
        get => dx;
    }
    public double DY
    {
        get => dy;
    }

    public Vector(double dx, double dy)
    {
        this.dx = dx;
        this.dy = dy;
    }

    public Vector(Point from, Point to)
    {
        dx = to.X - from.X;
        dy = to.Y - from.Y;
    }

    public double Size()
    {
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public void Normalize()
    {
        var size = Size();
        dx = dx / size;
        dy = dy / size;
    }

    public void Multiply(double alpha)
    {
        dx *= alpha;
        dy *= alpha;
    }

    public static Point Add(Point start, Vector displacement)
    {
        int newX = (int)(start.X + displacement.DX);
        int newY = (int)(start.Y + displacement.DY);

        return new Point(newX, newY);
    }
}
