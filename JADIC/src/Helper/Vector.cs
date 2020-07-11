using System;
using System.Drawing;

/// <summary>
/// Helper Vector class. Provides very basic and barebones functionality for 
/// working with 2D vectors.
/// </summary>
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

    /// <summary>
    /// Return the euclidean norm of the vector.
    /// </summary>
    /// <returns>Double representing the norm of the vector.</returns>
    public double Size()
    {
        return Math.Sqrt(DX * DX + DY * DY);
    }

    /// <summary>
    /// Normalize the vector to the unit norm.
    /// </summary>
    public void Normalize()
    {
        var size = Size();
        DX /= size;
        DY /= size;
    }

    /// <summary>
    /// Multiply the vector by given scalar.
    /// </summary>
    /// <param name="alpha">Factor by witch to scale.</param>
    public void Multiply(double alpha)
    {
        DX *= alpha;
        DY *= alpha;
    }

    /// <summary>
    /// Return new Point by adding the given vector.
    /// </summary>
    /// <param name="start">Base Point.</param>
    /// <param name="displacement">The displacement vector.</param>
    /// <returns>New Point.</returns>
    public static Point Add(Point start, Vector displacement)
    {
        int newX = (int)(start.X + displacement.DX);
        int newY = (int)(start.Y + displacement.DY);

        return new Point(newX, newY);
    }

    /// <summary>
    /// Create new vector from polar coordinates.
    /// </summary>
    public static Vector FromPolar(double angle, double size)
    {
        var vec = new Vector(Math.Sin(angle), Math.Cos(angle));
        vec.Multiply(size);
        return vec;
    }
}
